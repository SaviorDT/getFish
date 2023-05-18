using Hazdryx.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using 撈金魚.structures;
using static 撈金魚.GetProgramWindow;
using static 撈金魚.structures.WindowPack;

namespace 撈金魚.Analyzer
{
    internal class ImageDetermine
    {
        public static bool FishEnded(FastBitmap fastBitmap, WindowRect rect)
        {
            return IsMainFrame(fastBitmap);
        }
        internal static bool IsMainFrame(FastBitmap fastBitmap)
        {
            (int x, int y) = ProgramPointTranslator.MoleToContent(fastBitmap.Width, fastBitmap.Height, 260, 547);

            return fastBitmap.GetI(x, y) == -1 || fastBitmap.GetI(x, y) == -3487030;
        }
        public static bool ElementKnightBattleEnd(WindowSource source)
        {
            FastBitmap shot = ScreenAction.GetContentShot(source);

            bool is_end = AnalyzeElementImage.FindEndFrame(shot) &&
                AnalyzeElementImage.FindStartButton(shot);

            shot.Dispose();
            return is_end;
        }
        public static bool AllLineWhite(int start_x, int start_y, int end_x, int end_y, FastBitmap shot)
        {
            if (start_x < 0 || start_y < 0 || end_x >= shot.Width || end_y >= shot.Height) return true;
            int color = -1;
            for (int x = start_x; x <= end_x; x++)
            {
                for (int y = start_y; y <= end_y; y++)
                {
                    color &= shot.GetI(x, y);
                }
            }

            return ColorDetermine.IsWhite(color) || ColorDetermine.IsTransparent(color);
        }

        internal static bool SelectingServer(FastBitmap img)
        {
            return ColorDetermine.TestColorForMole(img, -8559, 478, 247)//(RGB)=(255, 222, 145)
                && ColorDetermine.TestColorForMole(img, -791, 497, 474);//255, 252, 233
        }

        internal static bool CanOpenLoginPage(FastBitmap img)
        {
            return ColorDetermine.TestColorForMole(img, -276736, 613, 447)//(RGB)=(251, 199, 0)
                && ColorDetermine.TestColorForMole(img, -14079658, 242, 176);//41, 41, 86
        }

        internal static bool SelectingAccount(FastBitmap img)
        {
            return ColorDetermine.TestColorForMole(img, -1, 245, 323)//(RGB)=(255, 255, 255)
                && ColorDetermine.TestColorForMole(img, -71490, 107, 82);//254, 232, 190
        }
    }
}
