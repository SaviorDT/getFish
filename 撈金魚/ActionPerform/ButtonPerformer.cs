using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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

        private static int actioning = 0;
        internal static void PerformButton(GetProgramWindow windows, int times, ActionKit action)
        {
            if (actioning > 0)
            {
                RejectAction();
            }
            else
            {
                Action<WindowSource, int> action_fun = get_action_fun(action);
                
                for (int i = 0; i < windows.Processes.Length; i++)
                {
                    int tmp = i;
                    new Thread(() => {
                        actioning++;
                        WindowSource window = new(windows, tmp);
                        action_fun(window, times);
                        Click.GoRestaurant(window);
                        actioning--;
                        }).Start();
                }
            }
        }

        private static Action<WindowSource, int> get_action_fun(ActionKit action)
        {
            return action switch
            {
                ActionKit.fish => GoldenFish.PlayFish,
                ActionKit.play_element_knight => ElementKnightActionPerformer.PlayElementKnight,
                ActionKit.buy_nutrient => BuyNutrient.BuyFatNutrient,
                ActionKit.element_knight_kit => ElementKnightActionPerformer.ElementKnightKit,
                ActionKit.dragon => Dragon.PlayDragon,
                _ => throw new ArgumentException("action invalid"),
            };
        }

        private static void RejectAction()
        {
            Message.ShowMessageToUser("正在執行中\n請勿重複按按鈕");
        }
    }
}
