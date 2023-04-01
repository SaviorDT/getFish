using System;
using System.Windows;
using System.Windows.Forms;
using 撈金魚.structures;

namespace 撈金魚
{
    internal class ProgramPointTranslator
    {
        //Screen.PrimaryScreen.Bounds.Width / SystemParameters.PrimaryScreenWidth
        //is "the window you look" / "the window in logic"
        //this will not be 1 while you change the scale in setting->system->indicator->scale
        //I don't know what's the actually English. In Chinese, it's
        //設定->系統->顯示器->比例
        //some laptop set to 125% in default(recommand). Such as mine.
        public static readonly double ScreenScale = Screen.PrimaryScreen.Bounds.Width / SystemParameters.PrimaryScreenWidth;
        private const int MOLE_W = 960, MOLE_H = 560;
        public static (int x, int y) MoleToProgram(WindowRect rect, int x, int y)
        {
            (x, y) = FixMolePoint(x, y);

            double content_scale = Math.Max(rect.Width / (double)MOLE_W,
                                           rect.Height / (double)MOLE_H);
            int rx = rect.left + (int)(x * content_scale);
            int ry = rect.top + (int)(y * content_scale);
            rx = (int)(rx * ScreenScale);
            ry = (int)(ry * ScreenScale);
            return (rx, ry);
        }
        public static (int x, int y) ContentToProgram(WindowRect rect, int x, int y)
        {
            int rx = rect.left + x;
            int ry = rect.top + y;
            rx = (int)(rx * ScreenScale);
            ry = (int)(ry * ScreenScale);
            return (rx, ry);
        }
        public static (int x, int y) MoleToContent(WindowRect rect, int x, int y)
        {
            //(x, y) = FixMolePoint(x, y);

            //double content_scale = Math.Max(rect.Width / (double)MOLE_W,
            //                               rect.Height / (double)MOLE_H);
            //int rx = (int)(x * content_scale);
            //int ry = (int)(y * content_scale);
            //return (rx, ry);
            return MoleToContent(rect.Width, rect.Height, x, y);
        }
        public static (int x, int y) MoleToContent(int width, int height, int x, int y)
        {
            (x, y) = FixMolePoint(x, y);

            double content_scale = Math.Max(width / (double)MOLE_W,
                                           height / (double)MOLE_H);
            int rx = (int)(x * content_scale);
            int ry = (int)(y * content_scale);
            return (rx, ry);
        }

        public static int MoleToContent(WindowRect rect, int x)
        {
            if (x < 0 || x > MOLE_W)
            {
                throw new ArgumentException("x shoud in [0, width_of_mole_in_default]");
            }

            return MoleToContent(rect.Width, rect.Height, x, 0).x;
        }

        private static (int x, int y) FixMolePoint(int x, int y)
        {
            x %= MOLE_W; y %= MOLE_H;
            if (x < 0) x += MOLE_W;
            if (y < 0) y += MOLE_H;

            return (x, y);
        }
    }
}
