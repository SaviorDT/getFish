using Hazdryx.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using 撈金魚.Analyzer;
using 撈金魚.structures;

namespace 撈金魚.ActionPerform
{
    internal class Wait
    {
        internal static void WaitForMainWindow(WindowPack.WindowSource window)
        {
            WaitFor(window, ImageDetermine.IsMainFrame);
        }

        internal static void WaitFor(WindowPack.WindowSource window, Func<FastBitmap, bool> Satisfy)
        {
            while (true)
            {
                FastBitmap shot = ScreenAction.GetContentShot(window);
                if (Satisfy(shot))
                {
                    shot.Dispose();
                    break;
                }
                shot.Dispose();
                Thread.Sleep(1000);
            }
        }

        internal static void WaitForNormalYesNoDialog(WindowPack.WindowSource window)
        {
            WaitFor(window, ImageDetermine.FindNormalYesNoDialog);
        }

        internal static void WaitForMainMapOpen(WindowPack.WindowSource window)
        {
            WaitFor(window, ImageDetermine.MainMapOpen);
        }

        internal static void WaitForBlackForestMapOpen(WindowPack.WindowSource window)
        {
            WaitFor(window, ImageDetermine.BlackForestMapOpen);
        }

        internal static void WaitForPlaceChange(WindowPack.WindowSource window)
        {
            WaitFor(window, (FastBitmap img) => {
                return !ImageDetermine.LoadingScene(img) && ImageDetermine.IsMainFrame(img);
                });
        }
    }
}
