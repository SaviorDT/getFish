using Hazdryx.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using 撈金魚.Analyzer;
using 撈金魚.structures;

namespace 撈金魚.ActionPerform
{
    internal class Login
    {
        internal static void ClickLoginButton(WindowPack.WindowSource window)
        {
            MouseInput.MouseClickForMole(window, 483, 495);
        }

        internal static void LoginAccount(WindowPack.WindowSource window)
        {
            MouseInput.MouseClickForMole(window, 483, 495);
        }

        private static bool SelectingAccount(WindowPack.WindowSource window)
        {
            FastBitmap shot = ScreenAction.GetContentShot(window);
            bool result = ImageDetermine.SelectingAccount(shot);
            shot.Dispose();
            return result;
        }

        internal static void SelectServer(WindowPack.WindowSource window)
        {
            MouseInput.MouseClickForMole(window, 316, 158);
        }

        internal static void WaitForLogin(WindowPack.WindowSource window, int timeout = -1)
        {
            Wait.WaitFor(window, ImageDetermine.CanOpenLoginPage, timeout);
        }

        internal static void WaitForSelectAccount(WindowPack.WindowSource window, int timeout = -1)
        {
            Wait.WaitFor(window, ImageDetermine.SelectingAccount, timeout);
        }

        internal static bool WaitForSelectServer(WindowPack.WindowSource window, int timeout = -1)
        {
            return Wait.WaitFor(window, ImageDetermine.SelectingServer, timeout);
        }

        internal static void WaitForProgramDraw()
        {
            Thread.Sleep(1000);
        }
    }
}
