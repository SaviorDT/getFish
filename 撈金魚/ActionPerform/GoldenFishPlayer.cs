using Hazdryx.Drawing;
using System;
using System.Numerics;
using System.Threading;
using System.Windows;
using 撈金魚.ActionPerform.Common;
using 撈金魚.Analyzer;
using 撈金魚.structures;
using static 撈金魚.structures.WindowPack;
using Point = System.Drawing.Point;

namespace 撈金魚.ActionPerform
{
    internal class GoldenFishPlayer : GamePlayer
    {
        public const int BUCKET_NET_X = 150, BUCKET_NET_Y = -10, BUCKET_WATER_X = 235, BUCKET_WATER_Y = 415, NET_FIX_X = 50, NET_FIX_Y = 45;

        public GoldenFishPlayer(WindowSource window, int times) : base(window, times) { }
        protected override void StartGame()
        {
            StartFish();
        }

        protected override bool PlayGame()
        {
            FishFish();
            while(!Wait.WaitForMainWindow(window, 300))
            {
                MouseInput.MouseClickForMole(window, 1, 1);
            }
            return EndFish();
        }
        
        //EndFish function check whether the fish done succesfully yet.
        //If Sucseed, do stop game, otherwise do nothing.
        protected override void StopGame() { }

        protected override void GoToGameRegion() { }

        protected override void LeaveGameRegion() { }

        private void StartFish()
        {
            MouseInput.MouseClickForMole(window, 838, 32);
            if (!Wait.WaitForMemoryBook(window, 10000))
            {
                return;
            }
            MouseInput.MouseClickForMole(window, 788, 443);
            Wait.WaitForFishGamePanel(window, 10000);
            MouseInput.MouseClickForMole(window, 474, 485);
        }

        private bool EndFish()
        {
            if (AnalyzeFishImage.FishSucessed(window))
            {
                Click.ClickNormalConfirmButton(window);
                //Thread.Sleep(500);
                return true;
            }
            //Thread.Sleep(500);
            return false;
        }
        private void FishFish()
        {
            bool failed = false;
            for (int i = 0; i < 25; i++)
            {
                if (failed)
                {
                    break;
                }
                Thread.Sleep(50);

                Point net_center = new(-1, -1);

                do
                {
                    AnalyzeFishImage.RefreshRadius(ProgramPointTranslator.MoleToContent(window.WindowRect, 100));
                    Point[] fish = FindDiferences();
                    if (fish.Length == 7)
                    {
                        if(i < 12)
                        {
                            if(Wait.WaitForSceneLoaded(window, 10000))
                            {
                                continue;
                            }
                        }
                        failed = true;
                        break;
                    }
                    net_center = AnalyzeFishImage.CalculateBestPoint(fish,window.WindowRect);
                } while (net_center.X == -1);
                if (!failed)
                {
                    MouseInput.FishClickKit(window, net_center.X, net_center.Y);
                }
            }
        }
        private Point[] FindDiferences()
        {
            Point[] fish = GetFishPoints();
            if (fish.Length < 3)
            {
                DateTime start = DateTime.Now;
                while (fish.Length < 3)
                {
                    if (DateTime.Now - start >= TimeSpan.FromSeconds(5))
                    {
                        return new Point[7];
                    }
                    fish = GetFishPoints();
                }
            }

            return fish;
        }
        private Point[] GetFishPoints()
        {
            //tmp_fish_count = 0;
            FastBitmap[] shots = ScreenAction.GetTwoContentFrame(window);
            Point[] fish;
            if (ImageDetermine.FishEnded(shots[1], window.WindowRect))
            {
                fish = new Point[7];
            }
            else
            {
                fish = AnalyzeFishImage.FindFish(shots);
            }

            foreach(FastBitmap shot in shots)
            {
                shot.Dispose();
            }
            return fish;
        }
        //private static (int x, int y) AdjustNetPoint(WindowRect rect, int x, int y)
        //{
        //    (int adjx, int adjy) = ProgramPointTranslator.MoleToContent(rect, x, y);
        //    return (adjx + x, adjy + y);
        //}
    }
}
