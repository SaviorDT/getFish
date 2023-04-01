using System;
using System.Runtime.InteropServices;
using System.Threading;
using 撈金魚.ActionPerform;
using 撈金魚.structures;
using static 撈金魚.structures.WindowPack;
using Point = System.Drawing.Point;

namespace 撈金魚
{
    internal static class MouseInput
    {
        [DllImport("User32.dll")]
        internal static extern Int32 SendMessage(IntPtr hWnd, int Msg, int wParam, IntPtr lParam);
        private static IntPtr CreateMouseParam(Point point)
        {
            return (IntPtr)((point.Y << 16) | (point.X & 0xffff));
        }

        private static void MoveMouse(WindowSource source, int x, int y)
        {
            Point point = new(x, y);
            //if (for_content)
            //{
            //    point = source.WindowRect.Content_to_client;
            //    point.X += x;
            //    point.Y += y;
            //}
            //else
            //{
            //    point = new Point(x, y);
            //}

            SendMessage(source.Process.MainWindowHandle, 0x200, 0x00000000, CreateMouseParam(point));
        }
        private static void ClickLeftMouseButton(WindowSource source, int x, int y)
        {
            //if (for_content)
            //{
            //    Point point = source.WindowRect.Content_to_client;
            //    point.X += x;
            //    point.Y += y;
            //    SendMessage(source.Process.MainWindowHandle, 0x201, 0x00000000, CreateMouseParam(point));
            //    SendMessage(source.Process.MainWindowHandle, 0x202, 0x00000000, CreateMouseParam(point));
            //}
            //else
            //{
            Point point = new(x, y);
            SendMessage(source.Process.MainWindowHandle, 0x201, 0x00000000, CreateMouseParam(point));
            SendMessage(source.Process.MainWindowHandle, 0x202, 0x00000000, CreateMouseParam(point));
            //}
        }

        public static void MouseClickForMole(WindowSource source, int x, int y, int delay = 0)
        {
            WindowRect rect = source.WindowRect;
            (x, y) = ProgramPointTranslator.MoleToProgram(source.WindowRect, x, y);

            if(delay > 0)
            {
                MoveMouse(source, x, y);
                Thread.Sleep(delay);
            }
            ClickLeftMouseButton(source, x, y);
        }

        //public static void MouseClickForMole(GetProgramWindow window, int x, int y)
        //{
        //    //x %= MainWindow.MOLE_W; y %= MainWindow.MOLE_H;
        //    //if (x < 0) x += MainWindow.MOLE_W;
        //    //if (y < 0) y += MainWindow.MOLE_H;

        //    for (int i = 0; i < window.processes.Length; i++)
        //    {
        //        window.UpdateRect(i);
        //        if (!window.Rects_of_client[i].is_enable) 
        //            continue;
        //        MouseClickForMole(new WindowSource(window, i), x, y);
        //    }
        //}

        public static void MouseClickForContent(WindowSource source, int x, int y, int delay = 0)
        {
            //WindowRect rect = source.WindowRect;
            //x %= rect.Width; y %= rect.Height;
            //if (x < 0) x += rect.Width;
            //if (y < 0) y += rect.Height;
            (x, y) = ProgramPointTranslator.ContentToProgram(source.WindowRect, x, y);

            MoveMouse(source, x, y);
            Thread.Sleep(delay);
            ClickLeftMouseButton(source, x, y);
        }

        //public static void MouseClickForContent(GetProgramWindow window, int x, int y)
        //{
        //    for (int i = 0; i < window.processes.Length; i++)
        //    {
        //        window.UpdateRect(i);
        //        if (!window.Rects_of_client[i].is_enable) continue;
        //        MouseClickForContent(new WindowSource(window, i), x, y);
        //    }
        //}

        private static bool clicking = false;
        public static void FishClickKit(WindowSource source, int x, int y)
        {
            //x += MainWindow.NET_FIX_X * source.WindowRect.Width / MainWindow.MOLE_W;
            //y += MainWindow.NET_FIX_Y * source.WindowRect.Height / MainWindow.MOLE_H;
            while (clicking) { }

            clicking = true;
            int delay = 80;
            MouseClickForContent(source, x, y, delay);
            MouseClickForMole(source, GoldenFish.BUCKET_NET_X, GoldenFish.BUCKET_NET_Y, delay);
            clicking = false;
            // I forgotten why I wrote the line bellow.
            //while (clicking) { }
        }
    }
}