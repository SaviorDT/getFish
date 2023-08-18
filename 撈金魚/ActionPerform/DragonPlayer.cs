using Hazdryx.Drawing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using 撈金魚.ActionPerform.Common;
using 撈金魚.Analyzer;
using 撈金魚.UserInterface;
using static 撈金魚.ActionPerform.Common.MapMove;
using static 撈金魚.structures.WindowPack;

namespace 撈金魚.ActionPerform
{
    internal class DragonPlayer : GamePlayer
    {
        private static bool find_slot_not_proper = false;
        private DateTime start_time = DateTime.Now;
        private DragonProperty property;
        private readonly int restart_time = 900;
        protected override void StartGame()
        {
            SelectMob();
            Thread.Sleep(1000);
        }

        protected override bool PlayGame()
        {
            return DragonFight();
        }

        protected override void StopGame() { }

        protected override void GoToGameRegion()
        {
            MapMove.MapMoveTo(new MapPlace(MapType.Black_Forest, (int)PlaceInBlackForest.VineForest), window);
            start_time = DateTime.Now;
            StartDragon();
            Wait.WaitForDragonFamePanel(window);
            //Thread.Sleep(300);
            SelectPage();
        }

        protected override void LeaveGameRegion()
        {
            CloseDragon();
            Thread.Sleep(500);
        }

        public override void Start()
        {
            if (!SetProperty())
            {
                return;
            }
            base.Start();
        }


        public DragonPlayer(WindowSource window, int times) : base(window, times) 
        {
        }

        private bool SetProperty()
        {
            property.slot = total_times % 10;

            total_times /= 10;
            property.page = total_times % 10;
            total_times /= 10;

            Reset(total_times);


            if (property.slot <= 0 || property.slot > 5)
            {
                if (!find_slot_not_proper)
                {
                    find_slot_not_proper = true;
                    Message.ShowMessageToUser("位置必須介於1~5之間");
                    Thread.Sleep(10);
                    find_slot_not_proper = false;
                }
                return false;
            }
            return true;
        }
        private void LetDragonNotLag()
        {
            window.ReOpen();
            //MapMove.MapMoveTo(new MapPlace(MapType.Black_Forest, (int)PlaceInBlackForest.VineForest), window);
            //start_time = DateTime.Now;
            GoToGameRegion();
        }

        private bool DragonFight()
        {
            while (true)
            {
                FastBitmap shot = ScreenAction.GetContentShot(window);


                if (!AnalyzeDragon.GameEnd(shot))
                {
                    if (AnalyzeDragon.CanRoll(shot))
                    {
                        Roll();
                    }
                    else if (AnalyzeDragon.CanSelectType(shot))
                    {
                        SelectRandomType();
                    }
                    else if(AnalyzeDragon.CanUseCard(shot))
                    {
                        UseRandomCard();
                    }
                    else
                    {
                        (int x, int y) = AnalyzeDragon.CanMoveTo(shot);
                        if(x != 0)
                        {
                            MouseInput.MouseClickForContent(window, x, y);
                        }
                    }

                    if (AnalyzeElementImage.FindEndFrame(shot))
                    {
                        Click.ClickNormalConfirmButton(window);
                    }

                    shot.Dispose();


                    if (DateTime.Now - start_time > TimeSpan.FromSeconds(restart_time))
                    {
                        LetDragonNotLag();
                    }

                    Thread.Sleep(500);
                }
                else
                {
                    bool victory = AnalyzeDragon.IsVictory(shot);
                    if (victory)
                    {
                        CloseLootFrame();
                    }
                    shot.Dispose();
                    return victory;
                }
            }
        }

        private void SelectRandomType()
        {
            int type = Math.Abs((int)window.Process.TotalProcessorTime.Ticks % 3);
            MouseInput.MouseClickForMole(window, 426 + type * 90, 333, 100);
        }

        private void UseRandomCard()
        {
            int x = 65, y = 221;//221~425
            for(int i= 0; i<5; i++)
            {
                MouseInput.MouseClickForMole(window, x, y + i * 51);
            }
        }

        private void Roll()
        {
            MouseInput.MouseClickForMole(window, 528, 235);
        }

        private void StartDragon()
        {
            MouseInput.MouseClickForMole(window, 823, 169);
            Wait.WaitForNormalYesNoDialog(window);
            Click.ClickNormalYesNoDialog(window, true);
            Thread.Sleep(1000);
            MouseInput.MouseClickForMole(window, 633, 494);
        }

        private void SelectPage()
        {
            for(int i=0; i<3; i++)
            {
                GoPageLeft();
                Thread.Sleep(300);
            }
            for(int i=1; i<property.page; i++)
            {
                GoPageRight();
                Thread.Sleep(300);
            }
        }

        private void GoPageLeft()
        {
            MouseInput.MouseClickForMole(window, 74, 290);
        }

        private void GoPageRight()
        {
            MouseInput.MouseClickForMole(window, 886, 290);
        }

        private void SelectMob()
        {
            int[] x = {0, 129, 788, 459, 166, 769};
            int[] y = {0, 129, 113, 310, 426, 397};
            MouseInput.MouseClickForMole(window, x[property.slot], y[property.slot]);
        }

        private void CloseLootFrame()
        {
            Click.ClickNormalConfirmButton(window);
            MouseInput.MouseClickForMole(window, 484, 429);
            Thread.Sleep(50);
            MouseInput.MouseClickForMole(window, 484, 450);
        }

        private void CloseDragon()
        {
            MouseInput.MouseClickForMole(window, 912, 32);
        }
    }
    struct DragonProperty
    {
        internal int page;
        internal int slot;
    }
}
