using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using 撈金魚.ActionPerform.FatHouse;
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
                        action_fun(new WindowSource(windows, tmp), times);
                        Common.GoRestaurant(new WindowSource(windows, tmp));
                        actioning--;
                        }).Start();
                }
            }
        }

        private static Action<WindowSource, int> get_action_fun(ActionKit action)
        {
            switch (action)
            {
                case ActionKit.fish:
                    return GoldenFish.PlayFish;
                case ActionKit.play_element_knight:
                    return ElementKnightActionPerformer.PlayElementKnight;
                case ActionKit.buy_nutrient:
                    return BuyNutrient.BuyFatNutrient;
                case ActionKit.element_knight_kit:
                    return ElementKnightActionPerformer.ElementKnightKit;
                case ActionKit.dragon:
                    return Dragon.PlayDragon;
                default:
                    throw new ArgumentException("action invalid");
            }
        }

        private static void RejectAction()
        {
            string message = "正在執行動作中...\n請勿重複按按鈕";
            string caption = "提示";
            //MessageBoxButton buttons = MessageBoxButton.OK;
            MessageBox.Show(message, caption);
        }
    }
}
