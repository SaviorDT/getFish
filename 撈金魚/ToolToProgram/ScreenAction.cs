using Hazdryx.Drawing;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using 撈金魚.structures;
using static 撈金魚.structures.WindowPack;

namespace 撈金魚
{
    internal class ScreenAction
    {
        public static FastBitmap[] GetTwoContentFrame(WindowSource window)
        {
            FastBitmap[] twoContentFrame = new FastBitmap[2];
            twoContentFrame[0] = GetContentShot(window);
            Thread.Sleep(20);
            twoContentFrame[1] = GetContentShot(window);

            //debugOutput(shot0, shot1, fast_shot0, fast_shot1);

            return twoContentFrame;
        }

        public static FastBitmap GetContentShot(WindowSource window)
        {
            Bitmap shot = GetClientImage(window.Process.MainWindowHandle, window.WindowRect);

            shot = shot.Clone(window.WindowRect.Content_rectangle, shot.PixelFormat);
            FastBitmap fast_shot = new(shot);
            shot.Dispose();

            return fast_shot;
        }

        public static Bitmap GetClientImage(IntPtr handle, WindowRect rect)
        {
            Bitmap bmp = new(rect.right, rect.bottom);
            Graphics gfxBmp = Graphics.FromImage(bmp);
            IntPtr hdcBitmap = gfxBmp.GetHdc();

            PrintWindow(handle, hdcBitmap, 1);
            gfxBmp.ReleaseHdc(hdcBitmap);
            gfxBmp.Dispose();

            return bmp;
        }

        [DllImport("user32.dll")]
        public static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);


        //private static void debugOutput(Bitmap shot0, Bitmap shot1, FastBitmap fast_shot0, FastBitmap fast_shot1)
        //{
        //    int[] data0 = fast_shot0.GetDataAsArray();
        //    int[] data1 = fast_shot1.GetDataAsArray();

        //    Bitmap output = new Bitmap(shot0.Width, fast_shot1.Height);
        //    FastBitmap fast_output = new FastBitmap(output);
        //    fast_output.Lock();

        //    for (int i = 0; i < fast_shot0.Height; i++)
        //    {
        //        int x = i * fast_shot0.Width;
        //        for (int j = 0; j < fast_shot0.Width; j++)
        //        {
        //            if (data0[x + j] != data1[x + j])
        //                fast_output.SetPixel(j, i, Color.Blue);
        //        }
        //    }

        //    shot0.Save("shot.png");
        //    shot1.Save("shot1.png");
        //    output.Save("difference.png");
        //}
    }
}
