using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace 撈金魚.Updater
{
    internal static class UpdateManager
    {
        private const string MANIFEST_URL = "https://raw.githubusercontent.com/SaviorDT/getFish/refs/heads/master/manifest.json";
        private static readonly JsonSerializerOptions MANIFEST_JSON_OPTIONS = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public static string PendingUpdateVersion { get; private set; }
        public static string PendingUpdateUrl { get; private set; }
        //true when scheduled by the startup auto-check, so it must still honor the AutoUpdate toggle at close time
        public static bool PendingUpdateIsAutomatic { get; private set; }
        public static bool HasPendingUpdate => !string.IsNullOrEmpty(PendingUpdateUrl);

        public static void SchedulePendingUpdate(string version, string url, bool isAutomatic = false)
        {
            PendingUpdateVersion = version;
            PendingUpdateUrl = url;
            PendingUpdateIsAutomatic = isAutomatic;
        }

        public static void ClearPendingUpdate()
        {
            PendingUpdateVersion = null;
            PendingUpdateUrl = null;
            PendingUpdateIsAutomatic = false;
        }

        //returns the newer manifest entry, or null when the current version is already up to date (or the check failed)
        public static ManifestEntry CheckForUpdate()
        {
            try
            {
                using (HttpClient client = new HttpClient { Timeout = TimeSpan.FromSeconds(5) })
                {
                    string json = client.GetStringAsync(MANIFEST_URL).ConfigureAwait(false).GetAwaiter().GetResult();
                    Manifest manifest = JsonSerializer.Deserialize<Manifest>(json, MANIFEST_JSON_OPTIONS);
                    ManifestEntry latest = manifest?.Versions?.LastOrDefault();
                    return latest != null && IsNewerThanCurrent(latest.Version) ? latest : null;
                }
            }
            catch
            {
                return null;
            }
        }

        public static bool IsNewerThanCurrent(string candidateVersion)
        {
            try
            {
                return new Version(candidateVersion) > new Version(AppVersion.Current);
            }
            catch
            {
                return false;
            }
        }

        //downloads the new exe next to the running one, then hands off to a helper script that
        //waits for this process to exit, swaps the files, restarts the app and deletes itself
        public static void DownloadAndInstall(string url, bool startImmediately = false)
        {
            string exe_path = Process.GetCurrentProcess().MainModule.FileName;
            string directory = Path.GetDirectoryName(exe_path);
            string new_exe_path = Path.Combine(directory, "update_new.exe");
            string script_path = Path.Combine(directory, "update_apply.ps1");

            using (HttpClient client = new HttpClient())
            {
                byte[] data = client.GetByteArrayAsync(url).ConfigureAwait(false).GetAwaiter().GetResult();
                File.WriteAllBytes(new_exe_path, data);
            }

            //cmd.exe's batch parser reads/compares text using the console's ANSI/OEM code page, which
            //mangles Chinese paths and the "撈金魚.exe" image name; PowerShell handles Unicode natively,
            //and matching the process by PID avoids needing to compare the (Chinese) process name at all
            int pid = Process.GetCurrentProcess().Id;
            string script_content =
$@"$targetPid = {pid}
while (Get-Process -Id $targetPid -ErrorAction SilentlyContinue) {{
    Start-Sleep -Milliseconds 500
}}
Move-Item -LiteralPath ""{new_exe_path}"" -Destination ""{exe_path}"" -Force
{(startImmediately ? "Start-Process -FilePath \"" + exe_path + "\"" : "")}
Remove-Item -LiteralPath $MyInvocation.MyCommand.Path -Force
";
            //a UTF-8 BOM is required so PowerShell reads the embedded Chinese paths correctly
            File.WriteAllText(script_path, script_content, new UTF8Encoding(true));

            Process.Start(new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-NoProfile -ExecutionPolicy Bypass -WindowStyle Hidden -File \"{script_path}\"",
                CreateNoWindow = true,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden
            });
        }
    }
}
