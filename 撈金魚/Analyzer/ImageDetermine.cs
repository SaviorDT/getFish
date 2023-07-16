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
        internal static bool IsMainFrame(FastBitmap img)
        {
            //(int x, int y) = ProgramPointTranslator.MoleToContent(img.Width, img.Height, 260, 547);

            //return img.GetI(x, y) == -1 || img.GetI(x, y) == -3487030 &&
            //I don't remember why I check if it is -3487030
            return ColorDetermine.IsGrayForMole(img, 135, 547)
                && ColorDetermine.IsGrayForMole(img, 275, 547); // only check chat box at left button
                //&& 
                //(!ColorDetermine.IsGrayForMole(img, 800, 233)
                //|| !ColorDetermine.IsGrayForMole(img, 100, 347));
        }
        internal static bool FindUpper(FastBitmap img)
        {
            return ColorDetermine.TestColorForMole(img, -531019, 690, 32);//247, 229, 181
        }
        internal static bool MainFrameLoadDone(FastBitmap img)
        {
            return IsMainFrame(img) && FindUpper(img);
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
            return ColorDetermine.IsWhiteForMole(img, 245, 323)//(RGB)=(255, 255, 255)
                && ColorDetermine.TestColorForMole(img, -71490, 107, 82);//254, 232, 190
        }

        internal static bool FindNormalYesNoDialog(FastBitmap img)
        {
            return FindNormalConfirmDialog(img)
                && ColorDetermine.IsWhiteForMole(img, 479, 347);
        }
        internal static bool FindNormalConfirmDialog(FastBitmap img)
        {
                //this function is suitable for one or two normal confirm dialog
            return ColorDetermine.IsWhiteForMole(img, 388, 223)
                && ColorDetermine.IsWhiteForMole(img, 582, 223)
                && ColorDetermine.IsSimilarForMole(img, Color.FromArgb(255, 242, 147, 4), 355, 386);
        }

        internal static bool MainMapOpen(FastBitmap img)
        {
            return ColorDetermine.TestColorForMole(img, -4694, 180, 26)//255, 237, 170
            && ColorDetermine.IsWhiteForMole(img, 704, 23);
        }

        internal static bool BlackForestMapOpen(FastBitmap img)
        {
            return ColorDetermine.TestColorForMole(img, -10138003, 576, 284)//101, 78, 109
            && ColorDetermine.TestColorForMole(img, -14733809, 444, 529);//31, 46, 15
        }

        //internal static bool LoadingScene(FastBitmap img)
        //{
        //    return (ColorDetermine.TestColorForMole(img, -12013311, 368, 36)//72, 177, 1
        //    && ColorDetermine.TestColorForMole(img, -3556708, 107, 82))//201, 186, 156
        //    || (ColorDetermine.IsWhiteForMole(img, 489, 196)
        //    && ColorDetermine.TestColorForMole(img, -71490, 107, 82));//254, 232, 190
        //}

        internal static bool MemoryBookOpen(FastBitmap img)
        {
            return ColorDetermine.TestColorForMole(img, -37964, 893, 485) // 255, 107, 180
                || ColorDetermine.TestColorForMole(img, -69702, 379, 199); // 254, 239, 186
        }

        internal static bool FindMoMoTreeOpen(FastBitmap img)
        {
            return ColorDetermine.TestColorForMole(img, -1126138, 740, 122) // 238, 209, 6
                || ColorDetermine.TestColorForMole(img, -12500575, 849, 219); // 65, 65, 161
        }
    }
}
