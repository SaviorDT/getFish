using Hazdryx.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 撈金魚.Analyzer
{
    internal class ColorDetermine
    {
        internal static bool IsGray(Color color)
        {

            return color.R == color.G && color.R == color.B;
        }

        internal static bool TestColor(FastBitmap bmp, int color, int x, int y)
        {
            return bmp.GetI(x, y) == color;
        }
        internal static bool TestColorForMole(FastBitmap bmp, int color, int x, int y)
        {
            (x, y) = ProgramPointTranslator.MoleToContent(bmp.Width, bmp.Height, x, y);
            return TestColor(bmp, color, x, y);
        }
    }
}
