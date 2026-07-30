using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace 撈金魚.UserInterface
{
    /// <summary>
    /// ProcessSelectWindow.xaml 的互動邏輯
    /// </summary>
    public partial class ProcessSelectWindow : Window
    {
        private class ProcessItem
        {
            public string Name { get; set; }
            public int ProcessId { get; set; }
            public ImageSource Icon { get; set; }
        }

        public string SelectedProcessName { get; private set; }

        public ProcessSelectWindow()
        {
            InitializeComponent();
            LoadForegroundProcesses();
        }

        private void LoadForegroundProcesses()
        {
            List<ProcessItem> items = new();
            foreach (Process process in Process.GetProcesses())
            {
                try
                {
                    //a process can exit mid-enumeration; any property access below may then throw
                    if (process.MainWindowHandle == IntPtr.Zero || string.IsNullOrWhiteSpace(process.MainWindowTitle))
                        continue;

                    items.Add(new ProcessItem
                    {
                        Name = process.ProcessName,
                        ProcessId = process.Id,
                        Icon = GetIcon(process)
                    });
                }
                catch
                {
                    //skip processes that exited or refused access while we inspected them
                }
            }

            process_list.ItemsSource = items
                .GroupBy(i => i.Name)
                .OrderBy(g => g.Key)
                .Select(g => g.First())
                .ToList();
        }

        private static ImageSource GetIcon(Process process)
        {
            IntPtr window_icon = GetWindowIcon(process.MainWindowHandle);
            if (window_icon != IntPtr.Zero)
            {
                try
                {
                    BitmapSource source = Imaging.CreateBitmapSourceFromHIcon(window_icon, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                    source.Freeze();
                    return source;
                }
                catch
                {
                    //fall through to the exe's associated icon below
                }
            }

            try
            {
                string path = process.MainModule?.FileName;
                if (string.IsNullOrEmpty(path))
                    return null;

                using System.Drawing.Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(path);
                if (icon == null)
                    return null;

                BitmapSource source = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                source.Freeze();
                return source;
            }
            catch
            {
                //some processes (elevated, different bitness, packaged app under WindowsApps) can't be inspected
                return null;
            }
        }

        //asking the window/class for its own icon works even when the exe path can't be read
        //(e.g. packaged Win11 apps under WindowsApps, or elevated processes)
        private static IntPtr GetWindowIcon(IntPtr hwnd)
        {
            if (hwnd == IntPtr.Zero)
                return IntPtr.Zero;

            IntPtr icon = SendIconMessage(hwnd, ICON_BIG);
            if (icon == IntPtr.Zero)
                icon = SendIconMessage(hwnd, ICON_SMALL2);
            if (icon == IntPtr.Zero)
                icon = SendIconMessage(hwnd, ICON_SMALL);
            if (icon == IntPtr.Zero)
                icon = GetClassLongPtr(hwnd, GCL_HICON);
            if (icon == IntPtr.Zero)
                icon = GetClassLongPtr(hwnd, GCL_HICONSM);

            return icon;
        }

        private static IntPtr SendIconMessage(IntPtr hwnd, int icon_type)
        {
            bool replied = SendMessageTimeout(hwnd, WM_GETICON, (IntPtr)icon_type, IntPtr.Zero, SMTO_ABORTIFHUNG, 200, out IntPtr result) != IntPtr.Zero;
            return replied ? result : IntPtr.Zero;
        }

        private static IntPtr GetClassLongPtr(IntPtr hwnd, int index)
        {
            return IntPtr.Size > 4 ? GetClassLongPtr64(hwnd, index) : new IntPtr(GetClassLong32(hwnd, index));
        }

        private const uint WM_GETICON = 0x007F;
        private const uint SMTO_ABORTIFHUNG = 0x0002;
        private const int ICON_SMALL = 0;
        private const int ICON_BIG = 1;
        private const int ICON_SMALL2 = 2;
        private const int GCL_HICON = -14;
        private const int GCL_HICONSM = -34;

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessageTimeout(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam, uint fuFlags, uint uTimeout, out IntPtr lpdwResult);
        [DllImport("user32.dll", EntryPoint = "GetClassLongPtr")]
        private static extern IntPtr GetClassLongPtr64(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll", EntryPoint = "GetClassLong")]
        private static extern uint GetClassLong32(IntPtr hWnd, int nIndex);

        private void PreviewToolTipOpening(object sender, ToolTipEventArgs e)
        {
            if (sender is not ListBoxItem { DataContext: ProcessItem item, ToolTip: ToolTip { Content: System.Windows.Controls.Image preview_image } })
                return;

            try
            {
                preview_image.Source = CaptureWindowPreview(Process.GetProcessById(item.ProcessId));
            }
            catch
            {
                //process may have exited, or the window refused to render; leave the tooltip blank
                preview_image.Source = null;
            }
        }

        private static ImageSource CaptureWindowPreview(Process process)
        {
            IntPtr handle = process.MainWindowHandle;
            if (handle == IntPtr.Zero || !GetWindowRect(handle, out RECT rect))
                return null;

            int width = rect.Right - rect.Left;
            int height = rect.Bottom - rect.Top;
            if (width <= 0 || height <= 0)
                return null;

            using Bitmap bitmap = new(width, height);
            using Graphics graphics = Graphics.FromImage(bitmap);
            IntPtr hdc = graphics.GetHdc();
            bool success = PrintWindow(handle, hdc, PW_RENDERFULLCONTENT);
            graphics.ReleaseHdc(hdc);
            if (!success)
                return null;

            IntPtr hbitmap = bitmap.GetHbitmap();
            try
            {
                BitmapSource source = Imaging.CreateBitmapSourceFromHBitmap(hbitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                source.Freeze();
                return source;
            }
            finally
            {
                DeleteObject(hbitmap);
            }
        }

        private const int PW_RENDERFULLCONTENT = 2;

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT rect);
        [DllImport("user32.dll")]
        private static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);
        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        private void ConfirmButtonClick(object sender, RoutedEventArgs e)
        {
            if (process_list.SelectedItem is not ProcessItem selected)
            {
                Message.ShowMessageToUser("請選擇一個應用程式", "錯誤");
                return;
            }
            SelectedProcessName = selected.Name;
            DialogResult = true;
            Close();
        }
    }
}
