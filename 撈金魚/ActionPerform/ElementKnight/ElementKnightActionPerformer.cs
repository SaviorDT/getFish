using Hazdryx.Drawing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;
using 撈金魚.ActionPerform.ElementKnight;
using 撈金魚.Analyzer;
using static 撈金魚.structures.WindowPack;

namespace 撈金魚.ActionPerform
{
    internal class ElementKnightActionPerformer
    {
        internal static void ElementKnightKit(WindowSource window, int times)
        {
            //times is not used.

            //1.無盡深淵75次
            //2.莎士摩亞10次
            //3.回復體力
            //4.莎士摩亞24次
            //5.回復體力
            //6.莎士摩亞7次
            //7.?關卡?次
            ElementKnightPlayer.GoInfiniteAbyss(window);
            PlayElementKnight(window, 75);
            ElementKnightPlayer.GoShakesMore(window);
            PlayElementKnight(window, 10);
            ElementKnightPlayer.UseTiliPotion(window);
            ElementKnightPlayer.PressShakesMore(window);
            PlayElementKnight(window, 25);
            ElementKnightPlayer.UseTiliPotion(window);
            ElementKnightPlayer.PressShakesMore(window);
            PlayElementKnight(window, 7);

            ElementKnightPlayer.GoLastDungeon(window);
            PlayElementKnight(window, 31);
        }
        internal static void PlayElementKnight(WindowSource window, int times)
        {
            for (int i = 0; i < times; i++)
            {
                //Console.WriteLine(i);
                StartElementKnightBattle(window);

                //move mouse leave start button, or the button will change color
                //and ImageDetermine.ElementKnightBattleEnd may not work.
                MouseInput.MouseClickForContent(window, 0, 0);

                Thread.Sleep(1000);
                while (!ImageDetermine.ElementKnightBattleEnd(window))
                {
                    Thread.Sleep(50);
                }

                Common.ClickNormalConfirmButton(window, 2);
                //Slept in ClickNormalConfirmButton
                ClickElementKnightGetCardButton(window);
                Thread.Sleep(30);
            }

            CloseElementKnight(window);
            Thread.Sleep(30);
            //Common.GoRestaurant(window);
        }


        private static void ClickElementKnightGetCardButton(WindowSource source)
        {
            MouseInput.MouseClickForMole(source, 480, 410);
        }

        private static void CloseElementKnight(WindowSource source)
        {
            MouseInput.MouseClickForMole(source, 716, 90);
        }

        private static void StartElementKnightBattle(WindowSource source)
        {
            MouseInput.MouseClickForMole(source, 480, 480);
        }
    }
}
