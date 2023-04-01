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
        internal static void SetForegroundWindow(Process process)
        {
            SetForegroundWindow(process.MainWindowHandle);
        }
    }
}
