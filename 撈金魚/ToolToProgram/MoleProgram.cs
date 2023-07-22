using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using 撈金魚.ActionPerform;
using 撈金魚.structures;
using 撈金魚.UserInterface;

namespace 撈金魚.ToolToProgram
{
    internal class MoleProgram
    {
        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        private const string MOLE_WEBSITE = "http://mole.61.com.tw/Client.swf";
        private static string flashplayer = "flashplayer_32_sa.exe";

        internal static void Login(WindowPack.WindowSource window)
        {
            ActionPerform.Login.WaitForLogin(window);
            ActionPerform.Login.ClickLoginButton(window);
            ActionPerform.Login.WaitForSelectAccount(window);
            ActionPerform.Login.LoginAccount(window);
            if (!ActionPerform.Login.WaitForSelectServer(window, 5000))
            {
                ActionPerform.Login.WaitForSelectAccount(window);
                ActionPerform.Login.LoginAccount(window);
                if (!ActionPerform.Login.WaitForSelectServer(window, 5000))
                {
                    Message.ShowMessageToUser("摩爾莊園似乎登不進去", "錯誤");
                    return;
                }
            }
            ActionPerform.Login.SelectServer(window);
            if (!Wait.WaitForMainWindow(window, 15000))
            {
                Message.ShowMessageToUser("摩爾莊園似乎登不進去", "錯誤");
            }
        }

        internal static Process ReOpenMole(Process process)
        {
            Process mole = null;
            try
            {
                //mole = Process.Start(flashplayer, MOLE_WEBSITE);
                ProcessStartInfo info = new(flashplayer, MOLE_WEBSITE)
                {
                    WindowStyle = ProcessWindowStyle.Minimized
                };//use this function will cause the program show less time on the screen
                // then directly use Process.Start(...) without WindowStyle argument.
                mole = Process.Start(info);
                mole.WaitForInputIdle();
                SimpleProgramAction.SetForegroundWindowFromMinimized(mole);
                SimpleProgramAction.SetWindowToButtom(mole);
            }
            catch (Exception e) when(e is InvalidOperationException || e is Win32Exception)
            {
                ReOpenMoleExceptionHandler(ref mole);
            }

            return mole;
        }

        private static void ReOpenMoleExceptionHandler(ref Process mole)
        {
            Message.ShowMessageToUser("未發現flashplayer，請指定位置");
            flashplayer = UserInput.SelectFile();

            if (flashplayer != "")
            {
                try
                {
                    mole = Process.Start(flashplayer, MOLE_WEBSITE);
                }
                catch (Exception ex)
                {
                    Message.ShowMessageToUser("啟動檔案失敗，如果你沒有在亂搞，請聯絡作者\n\n" + ex.ToString());
                }
            }
        }
    }
}
