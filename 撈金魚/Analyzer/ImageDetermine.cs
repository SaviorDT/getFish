using Hazdryx.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 撈金魚.structures;
using static 撈金魚.GetProgramWindow;
using static 撈金魚.structures.WindowPack;

namespace 撈金魚.Analyzer
{
    internal class ImageDetermine
    {
        public static bool IsWhite(FastBitmap shot, int x, int y)
        {
            return IsWhite(shot.GetI(x, y));
            // white or transparent
        }
        public static bool IsWhite(int color)
        {
            return (color == -1) || ((color & 0xff000000) == 0);
            // white or transparent
        }
        public static bool FishEnded(FastBitmap fastBitmap, WindowRect rect)
        {
            (int x, int y) = ProgramPointTranslator.MoleToContent(rect, 260, 547);

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

            return ImageDetermine.IsWhite(color);
        }
    }
}
