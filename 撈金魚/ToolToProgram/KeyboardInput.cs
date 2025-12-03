using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 撈金魚.structures;

namespace 撈金魚.ToolToProgram
{
    internal class KeyboardInput
    {
        private static readonly object sendLock = new object();
        internal static void SendStringToWindow(WindowPack.WindowSource window, string text)
        {
            lock (sendLock)
            {
                SimpleProgramAction.SetForegroundWindow(window.Process);
                foreach (char c in text)
                {
                    System.Windows.Forms.SendKeys.SendWait(c.ToString());
                }
            }
        }
    }
}
