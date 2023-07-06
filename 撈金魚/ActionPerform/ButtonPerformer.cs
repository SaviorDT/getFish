using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using 撈金魚.ActionPerform.Common;
using 撈金魚.ActionPerform.ElementKnight;
using 撈金魚.ActionPerform.FatHouse;
using 撈金魚.UserInterface;
using static 撈金魚.structures.WindowPack;

namespace 撈金魚.ActionPerform
{
    internal class ButtonPerformer
    {
        internal enum ActionKit
        {
            fish,
            buy_nutrient,
            play_element_knight,
            element_knight_kit,
            dragon
        }

        //private static int actioning = 0;
        internal static void PerformButton(GetProgramWindow windows, int times, ActionKit action)
        {
            //if (actioning > 0)
            //{
            //    RejectAction();
            //}
            //else
            //{
            //Action<WindowSource, int> action_fun = get_action_fun(action);
            bool start_something = false;
            for (int i = 0; i < windows.Processes.Length; i++)
            {
                WindowSource window = new(windows, i);
                if(window.IsEnable)
                {
                    start_something = true;
                    window.IsEnable = false;
                    new Thread(() => {
                        //action_fun(window, times);
                        GamePlayer player = GetGamePlayer(action, window, times);
                        player.Start();
                        Click.GoRestaurant(window);
                    }).Start();
                }
            }
            if(!start_something)
            {
                ActionFailed();
            }
            //}
        }

        private static GamePlayer GetGamePlayer(ActionKit action, WindowSource window, int times)
        {
            return action switch
            {
                ActionKit.fish => new GoldenFishPlayer(window, times),
                ActionKit.play_element_knight => new ElementKnightPlayer(window, times),
                ActionKit.buy_nutrient => new BuyNutrient(window, times),
                ActionKit.element_knight_kit => new ElementKnightKit(window, times),
                ActionKit.dragon => new DragonPlayer(window, times),
                _ => throw new ArgumentException("action invalid"),
            };
        }

        private static void ActionFailed()
        {
            Message.ShowMessageToUser("目前沒有空閒中的視窗");
        }
    }
}
