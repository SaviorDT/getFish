using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace 撈金魚.ToolToProgram
{
    internal class SimpleProgramAction
    {
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd); 
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
        
        internal static void SetForegroundWindow(Process process)
        {
            SetForegroundWindow(process.MainWindowHandle);
        }
        internal static void SetForegroundWindowFromMinimized(Process process)
        {
            ShowWindowAsync(process.MainWindowHandle, 9);
        }
        internal static void SetWindowToButtom(Process process)
        {
            SetWindowPos(process.MainWindowHandle, new IntPtr(1), 0, 0, 0, 0, 0x0001 | 0x0002 | 0x0010);
        }
    }
}
