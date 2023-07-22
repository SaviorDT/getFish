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
        internal static bool IsGrayForMole(FastBitmap bmp, int x, int y)
        {
            (x, y) = ProgramPointTranslator.MoleToContent(bmp.Width, bmp.Height, x, y);
            return IsGray(bmp.Get(x, y));
        }
        internal static bool TestColor(FastBitmap bmp, int color, int x, int y)
        {
            return bmp.GetI(x, y) == color;
        }
        internal static bool TestColorForSimilar(FastBitmap bmp, Color color, int x, int y, int difference = 32)
        {
            Color color2 = bmp.Get(x, y);
            return NumberSimilar(color2.R, color.R, difference)
                && NumberSimilar(color2.G, color.G, difference)
                && NumberSimilar(color2.B, color.B, difference);
        }

        private static bool NumberSimilar(byte b1, byte b2, int difference)
        {
            if (difference < 0) difference = -difference;
            int i1 = b1, i2 = b2;
            int dif = i1 - i2;
            return dif > -difference && dif < difference;
        }

        internal static bool TestColorForMole(FastBitmap bmp, int color, int x, int y)
        {
            (x, y) = ProgramPointTranslator.MoleToContent(bmp.Width, bmp.Height, x, y);
            return TestColor(bmp, color, x, y);
        }
        internal static bool IsWhite(FastBitmap shot, int x, int y)
        {
            return IsWhite(shot.GetI(x, y));
        }
        internal static bool IsWhiteForMole(FastBitmap bmp, int x, int y)
        {
            (x, y) = ProgramPointTranslator.MoleToContent(bmp.Width, bmp.Height, x, y);
            return IsWhite(bmp.GetI(x, y));
        }
        internal static bool IsWhite(int color)
        {
            return color == -1;
        }

        internal static bool IsTransparent(FastBitmap shot, int x, int y)
        {
            return IsTransparent(shot.GetI(x, y));
        }
        internal static bool IsTransparent(int color) 
        {
            return (color & 0xff000000) == 0;
        }

        internal static bool IsSimilarForMole(FastBitmap img, Color color, int x, int y, int difference = 32)
        {
            (x, y) = ProgramPointTranslator.MoleToContent(img.Width, img.Height, x, y);
            return TestColorForSimilar(img, color, x, y);
        }

        internal static bool IsTwoColorMixedForMole(FastBitmap img, Color color1, Color color2, int x, int y, double tolerance = 32)
        {
            (x, y) = ProgramPointTranslator.MoleToContent(img.Width, img.Height, x, y);
            Color test_color = img.Get(x, y);
            byte R = test_color.R;
            double rate = (double)(R - color1.R) / (color2.R - color1.R);
            if(rate < -0.15 || rate > 1.15)
            {
                return false;
            }
            byte G = (byte)(color1.G + rate * (color2.G - color1.G));
            byte B = (byte)(color1.B + rate * (color2.B - color1.B));

            return NumberSimilar(G, test_color.G, (int)tolerance)
                && NumberSimilar(B, test_color.B, (int)tolerance);
        }
    }
}
