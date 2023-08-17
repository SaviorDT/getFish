using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowsInput.Native;
using WindowsInput;
using 撈金魚.ActionPerform.Common;
using 撈金魚.structures;
using System.Configuration;
using System.Windows.Markup;
using 撈金魚.ToolToProgram;
using System.Windows;

namespace 撈金魚.ActionPerform
{
    internal class HomeFarmBuyer : GamePlayer
    {
        private int slot;
        private static bool buying = false;
        public HomeFarmBuyer(WindowPack.WindowSource window, int times, string title = "計數器") : base(window, times, title)
        {
        }

        public override void Start()
        {
            SetProperty();
            base.Start();
        }

        private void SetProperty()
        {
            slot = total_times % 10 - 1;
            total_times /= 10;

            Reset(total_times);
        }

        protected override void GoToGameRegion() 
        {
            while (buying)
            {
                Thread.Sleep(50);
            }
            buying = true;
            SimpleProgramAction.SetForegroundWindow(window.Process);
        }

        protected override void LeaveGameRegion() 
        {
            buying = false;
        }

        protected override bool PlayGame()
        {
            Buy99Goods();
            return true;
        }

        protected override void StartGame() { }

        protected override void StopGame()
        {
            Thread.Sleep(50);
        }
        private void Buy99Goods()
        {
            ClickShopSlot();
            Buy();

            Click.ClickNormalYesNoDialog(window, true);
            //MouseInput.MouseClickForMole(window, 527, 341);
            Thread.Sleep(100);
            //Click.ClickNormalConfirmButton(window);
            MouseInput.MouseClickForMole(window, 480, 340);
        }

        private void Buy()
        {
            MouseInput.MouseClickForMole(window, 516, 234);
            Thread.Sleep(50);

            InputSimulator input = new();

            input.Keyboard.KeyPress(VirtualKeyCode.NUMPAD9);
            Thread.Sleep(50);
            input.Keyboard.KeyPress(VirtualKeyCode.DELETE);
            Thread.Sleep(50);
            input.Keyboard.KeyPress(VirtualKeyCode.NUMPAD9);
            Thread.Sleep(50);
            //key_input(9,del, 9)
        }

        private void ClickShopSlot()
        {
            const int x0 = 245, y0 = 239, dx = 141, dy = 180;
            int x = slot % 4;
            int y = slot / 4;
            MouseInput.MouseClickForMole(window, x0 + x * dx, y0 + y * dy);
            Thread.Sleep(50);
        }
    }
}
