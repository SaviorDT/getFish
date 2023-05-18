using Hazdryx.Drawing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using 撈金魚.Analyzer;
using 撈金魚.UserInterface;
using static 撈金魚.structures.WindowPack;

namespace 撈金魚.ActionPerform
{
    internal class Dragon
    {
        private static bool find_slot_not_proper = false;
        internal static void PlayDragon(WindowSource window, int properties)
        {
            int slot = properties % 10;
            properties /= 10;
            int page = properties % 10;
            int times = properties / 10;


            if (slot <= 0 || slot > 5)
            {
                if (find_slot_not_proper)
                {
                    return;
                }
                find_slot_not_proper = true;
                Message.ShowMessageToUser("位置必須介於1~5之間");
                Thread.Sleep(10);
                find_slot_not_proper = false;
                return;
            }

            StartDragon(window);
            Thread.Sleep(300);
            SelectPage(window, page);
            for(int i=0; i < times;)
            {
                SelectMob(window, slot);
                Thread.Sleep(1000);
                i += DragonFight(window) ? 1 : 0;
                Thread.Sleep(300);
            }
            CloseDragon(window);
            Thread.Sleep(500);
        }

        private static bool DragonFight(WindowSource window)
        {
            while (true)
            {
                FastBitmap shot = ScreenAction.GetContentShot(window);


                if (!AnalyzeDragon.GameEnd(shot))
                {
                    //string debug = "gaming:\n";
                    if (AnalyzeDragon.CanRoll(shot))
                    {
                        //debug += "canroll";
                        Roll(window);
                    }
                    else if (AnalyzeDragon.CanSelectType(shot))
                    {
                        //debug += "can select type";
                        SelectRandomType(window);
                    }
                    else if(AnalyzeDragon.CanUseCard(shot))
                    {
                        //debug += "can use card";
                        UseRandomCard(window);
                    }
                    else
                    {
                        (int x, int y) = AnalyzeDragon.CanMoveTo(shot);
                        if(x != 0)
                        {
                            //debug += String.Format("can move to: {0}, {1}", x, y);
                            MouseInput.MouseClickForContent(window, x, y);
                        }
                    }

                    //shot.Save("test.png");
                    //StreamWriter sw = new StreamWriter("test.txt");
                    //sw.Write(debug);
                    //sw.Close();

                    if (AnalyzeElementImage.FindEndFrame(shot))
                    {
                        Click.ClickNormalConfirmButton(window);
                    }

                    shot.Dispose();
                    Thread.Sleep(500);
                }
                else
                {
                    bool victory = AnalyzeDragon.IsVictory(shot);
                    if (victory)
                    {
                        CloseLootFrame(window);
                    }
                    shot.Dispose();
                    return victory;
                }
            }
        }

        private static void SelectRandomType(WindowSource window)
        {
            int type = Math.Abs((int)window.Process.TotalProcessorTime.Ticks % 3);
            MouseInput.MouseClickForMole(window, 426 + type * 90, 333, 100);
        }

        private static void UseRandomCard(WindowSource window)
        {
            int x = 65, y = 221;//221~425
            for(int i= 0; i<5; i++)
            {
                MouseInput.MouseClickForMole(window, x, y + i * 51);
            }
        }

        private static void Roll(WindowSource window)
        {
            MouseInput.MouseClickForMole(window, 528, 235);
        }

        private static void StartDragon(WindowSource source)
        {
            Click.ClickNormalYesNoDialog(source, true);
            Thread.Sleep(1000);
            MouseInput.MouseClickForMole(source, 633, 494);
        }

        private static void SelectPage(WindowSource source, int page)
        {
            for(int i=0; i<3; i++)
            {
                GoPageLeft(source);
                Thread.Sleep(300);
            }
            for(int i=1; i<page; i++)
            {
                GoPageRight(source);
                Thread.Sleep(300);
            }
        }

        private static void GoPageLeft(WindowSource source)
        {
            MouseInput.MouseClickForMole(source, 74, 290);
        }

        private static void GoPageRight(WindowSource source)
        {
            MouseInput.MouseClickForMole(source, 886, 290);
        }

        private static void SelectMob(WindowSource source, int slot)
        {
            int[] x = {0, 129, 788, 459, 166, 769};
            int[] y = {0, 129, 113, 310, 426, 397};
            MouseInput.MouseClickForMole(source, x[slot], y[slot]);
        }

        private static void CloseLootFrame(WindowSource source)
        {
            Click.ClickNormalConfirmButton(source);
            MouseInput.MouseClickForMole(source, 484, 429);
            Thread.Sleep(50);
            MouseInput.MouseClickForMole(source, 484, 450);
        }

        private static void CloseDragon(WindowSource source)
        {
            MouseInput.MouseClickForMole(source, 912, 32);
        }
    }
}
