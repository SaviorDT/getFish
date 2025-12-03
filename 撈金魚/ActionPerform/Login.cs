using Hazdryx.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using 撈金魚.Analyzer;
using 撈金魚.structures;
using 撈金魚.ToolToProgram;

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

        internal static void SelectOtherAccount(WindowPack.WindowSource window)
        {
            MouseInput.MouseClickForMole(window, 83, 525);
        }

        internal static void RememberAccount(WindowPack.WindowSource window)
        {
            MouseInput.MouseClickForMole(window, 592, 193);
        }

        internal static void LoginOtherAccount(WindowPack.WindowSource window)
        {
            MouseInput.MouseClickForMole(window, 487, 342);
        }

        internal static void InputAccount(WindowPack.WindowSource window, string account)
        {
            MouseInput.MouseClickForMole(window, 447, 192);
            KeyboardInput.SendStringToWindow(window, account);
        }

        internal static void InputPassword(WindowPack.WindowSource window, string password)
        {
            MouseInput.MouseClickForMole(window, 443, 234);
            KeyboardInput.SendStringToWindow(window, password);
        }

        internal static void WaitForInputOtherAccount(WindowPack.WindowSource window)
        {
            Thread.Sleep(1000);
        }
    }
}
