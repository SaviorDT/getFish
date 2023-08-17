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
    internal class Feeder : GamePlayer
    {
        private int slot_x, slot_y;
        private static bool buying = false;
        public Feeder(WindowPack.WindowSource window, int times, string title = "計數器") : base(window, times, title)
        {
        }

        public override void Start()
        {
            SetProperty();
            if(!Anti_87())
            {
                return;
            }
            base.Start();
        }

        private bool Anti_87()
        {
            (int, int)[] recommands = {(0, 1), (1, 3), (2, 0) };
            foreach(var command in recommands)
            {
                if(slot_y == command.Item1 && slot_x == command.Item2)
                {
                    return true;
                }
            }
            return UserInterface.Message.ShowYesNoToUser("作者推薦欄位為\n" +
                "1-2 魚餌、 2-4 精緻火腿、 3-1 寵物脆脆酥\n" +
                String.Format("檢測到你填入欄位為：{0}-{1}\n", slot_y + 1, slot_x + 1) +
                "請確認是否繼續？", "警告");
        }

        private void SetProperty()
        {
            slot_x = total_times % 10 - 1;
            total_times /= 10;
            slot_y = total_times % 10 - 1;
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
            Add999Feed();
            return true;
        }

        protected override void StartGame() { }

        protected override void StopGame()
        {
            Thread.Sleep(50);
        }
        private void Add999Feed()
        {
            if (!ClickFeedSlot())
            {
                should_stop = true;
                UserInterface.Message.ShowMessageToUser("飼料不足");
                return;
            }
            Add();

            Click.ClickNormalYesNoDialog(window, true);
            //MouseInput.MouseClickForMole(window, 527, 341);
            Thread.Sleep(100);
            //Click.ClickNormalConfirmButton(window);
            //MouseInput.MouseClickForMole(window, 480, 340);
        }

        private void Add()
        {
            MouseInput.MouseClickForMole(window, 514, 276);
            Thread.Sleep(50);

            InputSimulator input = new();

            input.Keyboard.KeyPress(VirtualKeyCode.NUMPAD9);
            Thread.Sleep(50);
            input.Keyboard.KeyPress(VirtualKeyCode.DELETE);
            Thread.Sleep(50);
            input.Keyboard.KeyPress(VirtualKeyCode.NUMPAD9);
            Thread.Sleep(50);
            input.Keyboard.KeyPress(VirtualKeyCode.NUMPAD9);
            Thread.Sleep(50);
            //key_input(9, del, 9, 9)
        }

        private bool ClickFeedSlot()
        {
            const int x0 = 220, y0 = 145, dx = 96, dy = 103;
            MouseInput.MouseClickForMole(window, x0 + slot_x * dx, y0 + slot_y * dy);

            return Wait.WaitForNormalYesNoDialog(window, 2000);
        }
    }
}
