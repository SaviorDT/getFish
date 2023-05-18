using Hazdryx.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 撈金魚.Analyzer
{
    internal class AnalyzeDragon
    {
        internal static (int x, int y) CanMoveTo(FastBitmap shot)
        {
            (int x_max, int y_max) = ProgramPointTranslator.MoleToContent(shot.Width, shot.Height, 855, 485);
            int x_min = 200, y_min = 220; // x or y > min

            for(int x=0; x<x_max; x++)
            {
                int y0 = 0;
                if(x < x_min)
                {
                    y0 = y_min + 1;
                }

                for (int y = y0; y < y_max; y++)
                {
                    if(ColorDetermine.TestColor(shot, -256, x, y))
                    {
                        // There may be tow side of stone slab is -256
                        // 
                        //int[,] yellows = SearchConnectedColor(shot, -256, x, y);
                        //return FindMidPointOfThirdLine(yellows);
                        //shot.Save(String.Format("{0}.{1}.png", x, y));
                        return (x + 3, y);
                    }
                }
            }

            return (0, 0);
        }

        internal static bool CanRoll(FastBitmap shot)
        {
            return shot.GetI(488, 273) == -2927803;
        }

        internal static bool IsVictory(FastBitmap shot)
        {
            bool loot_frame = ColorDetermine.TestColorForMole(shot, -2777754, 313, 228);
            bool too_many_times = AnalyzeElementImage.FindEndFrame(shot);
            return loot_frame || too_many_times;
        }

        internal static bool CanUseCard(FastBitmap shot)
        {
            int x = 51, y = 221;//221~425
            //221, 272, 323, 374, 425
            for(int i=0; i<5; i++)
            {
                (int cx, int cy) = ProgramPointTranslator.MoleToContent(shot.Width, shot.Height, x, y + i * 51);
                if (!ColorDetermine.IsGray(shot.Get(cx, cy)))
                {
                    //Console.WriteLine(String.Format("x: {0}, y:{1}", cx, cy));
                    return true;
                }
            }
            return false;
        }

        internal static bool GameEnd(FastBitmap shot)
        {
            (int x1, int y1) = ProgramPointTranslator.MoleToContent(shot.Width, shot.Height, 411, 519);
            (int x2, int y2) = ProgramPointTranslator.MoleToContent(shot.Width, shot.Height, 611, 539);

            int sum_r = 0, sum_g = 0, sum_b = 0;
            for(int i = x1; i < x2; i++)
            {
                for(int j = y1; j < y2; j++)
                {
                    Color color = shot.Get(i, j);
                    sum_r += color.R;
                    sum_g += color.G;
                    sum_b += color.B;
                }
            }

            sum_r /= ((x2-x1)*(y2-y1));
            sum_g /= ((x2-x1)*(y2-y1));
            sum_b /= ((x2-x1)*(y2-y1));
            bool r_right = sum_r > 120 && sum_r < 140;
            bool g_right = sum_g > 195 && sum_g < 205;
            bool b_right = sum_b > 110 && sum_b < 225;

            return r_right && g_right && b_right;
        }

        internal static bool CanSelectType(FastBitmap shot)
        {
            return ColorDetermine.TestColorForMole(shot, -466238, 428, 252);
        }
    }
}
