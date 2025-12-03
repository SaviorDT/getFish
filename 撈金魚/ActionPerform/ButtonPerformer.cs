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
            dragon,
            momo_tree,
            help_fish,
            home_farm_buy,
            feed,
            capture_mouse_pos,
            defence_moon_cake,
            lin_wei,
            test
        }

        //private static int actioning = 0;
        internal static void PerformButton(Dictionary<int, WindowSource> windows, int times, ActionKit action, bool go_restaurant = true, object para = null)
        {
            if(times == -2)
            {
                return;
            }

            bool start_something = false;
            foreach( WindowSource window in windows.Values )
            {
                if (!window.Actioning && window.IsEnable)
                {
                    start_something = true;
                    window.Actioning = true;
                    new Thread(() => {
                        //action_fun(window, times);
                        try
                        {
                            GamePlayer player = GetGamePlayer(action, window, times, para);
                            player.Start();
                            if (go_restaurant)
                            {
                                Click.GoRestaurant(window);
                            }
                        }
                        finally
                        {
                            window.Actioning = false;
                        }
                    }).Start();
                }
            }
            if(!start_something)
            {
                ActionFailed();
            }
            //}
        }

        private static GamePlayer GetGamePlayer(ActionKit action, WindowSource window, int times, object para)
        {
            return action switch
            {
                ActionKit.fish => new GoldenFishPlayer(window, times),
                ActionKit.play_element_knight => new ElementKnightPlayer(window, times),
                ActionKit.buy_nutrient => new BuyNutrient(window, times),
                ActionKit.element_knight_kit => new ElementKnightKit(window, times),
                ActionKit.dragon => new DragonPlayer(window, times),
                ActionKit.momo_tree => new MoMoTreeWaterer(window, times, para),
                ActionKit.help_fish => new HelpGoldenFishPlayer(window, times),
                ActionKit.home_farm_buy => new HomeFarmBuyer(window, times),
                ActionKit.feed => new Feeder(window, times),
                ActionKit.test => new TmpPlayer(window, times),
                ActionKit.capture_mouse_pos => new CaptureMousePosPlayer(window, times),
                ActionKit.defence_moon_cake => new DefenceMoonCakePlayer(window, times),
                ActionKit.lin_wei => new LinWeiPlayer(window, para),
                _ => throw new ArgumentException("action invalid"),
            };
        }

        private static void ActionFailed()
        {
            Message.ShowMessageToUser("目前沒有空閒中的視窗");
        }
    }
}
