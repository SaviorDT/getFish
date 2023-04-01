using Hazdryx.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 撈金魚.Analyzer
{
    internal class AnalyzeElementImage
    {
        internal static bool FindEndFrame(FastBitmap shot)
        {
            //this function can find first and second common confirm frame
            //and get card frame for elementknight.
            int[] xs = { 388, 580 };
            int[] ys = { 223, 358 };
            foreach (int x in xs)
            {
                foreach (int y in ys)
                {
                    if (!ImageDetermine.IsWhite(shot, x, y))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        internal static bool FindStartButton(FastBitmap shot)
        {
            (int x, int y) = ProgramPointTranslator.MoleToContent(shot.Width, shot.Height, 510, 479);
            return shot.GetI(x, y) == -13312;
        }
    }
}
