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
        internal static bool WaitForMainWindow(WindowPack.WindowSource window, int timeout = -1)
        {
            return WaitFor(window, ImageDetermine.MainFrameLoadDone, timeout);
        }

        internal static bool WaitFor(WindowPack.WindowSource window, Func<FastBitmap, bool> Satisfy, int timeout)
        {
            DateTime out_time = DateTime.Now + TimeSpan.FromMilliseconds(timeout);
            while (true)
            {
                //To use timeout, Some code should be modified first to handle timeout event.

                if(timeout > 0 && DateTime.Now > out_time)
                {
                    return false;
                }
                FastBitmap shot = ScreenAction.GetContentShot(window);
                if (Satisfy(shot))
                {
                    shot.Dispose();
                    break;
                }
                shot.Dispose();
                Thread.Sleep(100);
            }
            return true;
        }

        internal static bool WaitForNormalYesNoDialog(WindowPack.WindowSource window, int timeout = -1)
        {
            return WaitFor(window, ImageDetermine.FindNormalYesNoDialog, timeout);
        }

        internal static bool WaitForMainMapOpen(WindowPack.WindowSource window, int timeout = -1)
        {
            return WaitFor(window, ImageDetermine.MainMapOpen, timeout);
        }

        internal static bool WaitForBlackForestMapOpen(WindowPack.WindowSource window, int timeout = -1)
        {
            return WaitFor(window, ImageDetermine.BlackForestMapOpen, timeout);
        }

        internal static bool WaitForPlaceChange(WindowPack.WindowSource window, int timeout = -1)
        {
            return WaitFor(window, ImageDetermine.MainFrameLoadDone, timeout);
        }

        internal static bool WaitForMemoryBook(WindowPack.WindowSource window, int timeout = 10000)
        {
            return WaitFor(window, ImageDetermine.MemoryBookOpen, timeout);
        }

        internal static bool WaitForFishGamePanel(WindowPack.WindowSource window, int timeout = 10000)
        {
            return WaitFor(window, ImageDetermine.FishGamePanelOpen, timeout);
        }
    }
}
