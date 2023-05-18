using Hazdryx.Drawing;
using System;
using System.Threading;
using System.Windows;
using 撈金魚.Analyzer;
using 撈金魚.structures;
using static 撈金魚.structures.WindowPack;
using Point = System.Drawing.Point;

namespace 撈金魚.ActionPerform
{
    internal class GoldenFish
    {
        public const int BUCKET_NET_X = 150, BUCKET_NET_Y = -10, BUCKET_WATER_X = 235, BUCKET_WATER_Y = 415, NET_FIX_X = 50, NET_FIX_Y = 45;
        //private static int playing_fish = 0;
        //public static void PlayFish(GetProgramWindow windows, int times)
        //{
        //    if (playing_fish > 0)
        //    {
        //        //untested
        //        RejectAction();
        //    }
        //    else
        //    {
        //        for (int i = 0; i < windows.processes.Length; i++)
        //        {
        //            new Thread(() => PlayFish(new WindowSource(windows, i), times)).Start();
        //        }
        //    }
        //}

        internal static void PlayFish(WindowSource source, int times)
        {
            int complete = 0;
            while(complete < times) 
            {
                Thread.Sleep(10);
                if (complete == 0)
                    StartFish(source, 2500);
                else
                    StartFish(source, 300);
                Thread.Sleep(50);
                FishFish(source);
                Thread.Sleep(50);
                EndFish(source, ref complete);
            }
            //Common.GoRestaurant(source);
        }

        private static void StartFish(WindowSource source, int delay)
        {
            MouseInput.MouseClickForMole(source, 838, 32);
            Thread.Sleep(delay);
            MouseInput.MouseClickForMole(source, 788, 443);
            Thread.Sleep(delay);
            MouseInput.MouseClickForMole(source, 474, 485);
        }

        private static void EndFish(WindowSource source, ref int complete_times)
        {
            if (AnalyzeFishImage.FishSucessed(source))
            {
                complete_times++;
                Click.ClickNormalConfirmButton(source);
            }
            //DoInput.MouseClickForMole(source, 480, 358);
        }
        private static void FishFish(WindowSource source)
        {
            bool failed = false;
            for (int i = 0; i < 25; i++)
            {
                if (failed)
                {
                    break;
                }
                Thread.Sleep(50);
                //if (!source.WindowRect.is_enable) break;

                Point net_center = new(-1, -1);

                do
                {
                    AnalyzeFishImage.RefreshRadius(ProgramPointTranslator.MoleToContent(source.WindowRect, 100));
                    Point[] fish = FindDiferences(source);
                    if (fish.Length == 7)
                    {
                        failed = true;
                        break;
                    }
                    net_center = AnalyzeFishImage.CalculateBestPoint(fish,source.WindowRect);
                } while (net_center.X == -1);
                if (!failed)
                {
                    MouseInput.FishClickKit(source, net_center.X, net_center.Y);
                }
            }
        }
        private static Point[] FindDiferences(WindowSource source)
        {
            Point[] fish = GetFishPoints(source);
            if (fish.Length < 3)
            {
                DateTime start = DateTime.Now;
                while (fish.Length < 3)
                {
                    if (DateTime.Now - start >= TimeSpan.FromSeconds(5))
                    {
                        return new Point[7];
                    }
                    fish = GetFishPoints(source);
                }
            }

            return fish;
        }
        private static Point[] GetFishPoints(WindowSource window)
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
        private static (int x, int y) AdjustNetPoint(WindowRect rect, int x, int y)
        {
            (int adjx, int adjy) = ProgramPointTranslator.MoleToContent(rect, x, y);
            return (adjx + x, adjy + y);
        }
    }
}
