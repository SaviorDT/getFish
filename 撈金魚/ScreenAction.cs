using FastBitmapLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace 撈金魚
{
    internal class ScreenAction
    {
        public static Point[] GetFish(GetProgramWindow window)
        {
            //tmp_fish_count = 0;
            FastBitmap[] shot = getScreenShot(window);
            if (isEnded(shot[1]))
                return new Point[7];
            Point[] fish = AnalyzeFish(shot);
            return fish;
        }

        private static bool isEnded(FastBitmap fastBitmap)
        {
            int x = 260 * fastBitmap.Width / MainWindow.MOLE_W;
            int y = 547 * fastBitmap.Height / MainWindow.MOLE_H;
            return fastBitmap.GetPixelInt(x, y) == -1 || fastBitmap.GetPixelInt(x, y) == -3487030;
        }

        private static Point[] AnalyzeFish(FastBitmap[] shot)
        {
            int bucket_x = MainWindow.BUCKET_WATER_X * shot[0].Width / MainWindow.MOLE_W;
            int bucket_y = MainWindow.BUCKET_WATER_Y * shot[0].Height / MainWindow.MOLE_H;

            Size size = new Size(shot[0].Width, shot[0].Height);
            Point cdot = new Point(size.Width / 2 -1, size.Height / 2 -1);
            int max_radius = size.Width / 2 + size.Height / 2 -1;
            int radius_base = 3;
            ArrayList points = new ArrayList();
            bool[,] scanned = new bool[shot[0].Width, shot[0].Height];
            for (int i = 0; i < size.Width; i++)
                for(int j=0; j < size.Height; j++)
                    scanned[i, j] = false;

            for (int radius = 0; radius<max_radius && points.Count<6; radius += radius_base)
            {
                int right_bound = Math.Min(cdot.X + radius, size.Width);
                int x = cdot.X - radius, y_len = 0;
                if (x < 0)
                {
                    y_len -= x;
                    x = 0;
                }

                while (x < right_bound)
                {
                    int y = cdot.Y - y_len;
                    if ((!scanned[x, y]) && shot[0].GetPixelInt(x, y) != shot[1].GetPixelInt(x, y))
                    {
                        points.Add(ScanFish(shot, x, y, scanned));
                        if (points.Count >= 6)
                            break;
                    }

                    y = cdot.Y + y_len;
                    if (x < bucket_x && y > bucket_y) { }
                    else if ((!scanned[x, y]) && shot[0].GetPixelInt(x, y) != shot[1].GetPixelInt(x, y))
                    {
                        points.Add(ScanFish(shot, x, y, scanned));
                        if (points.Count >= 6)
                            break;
                    }

                    x += radius_base;
                    y_len += (x < size.Width/2) ? radius_base : -radius_base;

                    if(y_len >= size.Height / 2)
                    {
                        x = size.Width - x;
                        x += radius_base;
                        y_len -= radius_base;
                    }
                }
            }


            return points.ToArray(typeof(Point)) as Point[];
        }


        private static Point ScanFish(FastBitmap[] shot, int x, int y, bool[,] scanned)
        {
            ArrayList points = new ArrayList();
            Queue<Point> to_scan = new Queue<Point>();

            points.Add(new Point(x, y));
            scanned[x, y] = true;
            markAround(to_scan, x, y, scanned);

            while(to_scan.Count > 0)
            {
                Point pt = to_scan.Dequeue();
                if (shot[0].GetPixelInt(pt.X, pt.Y) != shot[1].GetPixelInt(pt.X, pt.Y))
                {
                    points.Add(pt);
                    markAround(to_scan, pt.X, pt.Y, scanned);
                }
            }

            //savePoints(points, shot);
            return calculateCenterPoint(points);
        }

        private static void markAround(Queue<Point> to_scan, int x, int y, bool[,] scanned)
        {
            for(int i=x-2; i<=x+2; i++)
            {
                for(int j=y-2; j<=y+2; j++)
                {
                    try
                    {
                        if (!scanned[i,j])
                        {
                            to_scan.Enqueue(new Point(i,j));
                            scanned[i, j] = true;
                        }
                    } catch (System.IndexOutOfRangeException) { }
                }
            }
        }

        private class PointComparer : IComparer<Point>
        {
            public int Compare(Point x, Point y)
            {
                if (x.X == y.X)
                    return x.Y.CompareTo(y.Y);
                return x.X.CompareTo(y.X);
            }
        }

        //private static int tmp_fish_count = 0;
        //private static void savePoints(ArrayList points, FastBitmap[] shot)
        //{
        //    Bitmap bitmap = new Bitmap(shot[0].Width, shot[0].Height);
        //    FastBitmap fast = new FastBitmap(bitmap);
        //    fast.Lock();

        //    foreach(Point p in points)
        //    {
        //        fast.SetPixel(p.X, p.Y, Color.Blue);
        //    }

        //    bitmap.Save(Convert.ToString(++tmp_fish_count) + ".png");
        //}

        private static Point calculateCenterPoint(ArrayList points)
        {
            int cut = points.Count;

            long x = 0L, y = 0L;
            foreach (Point point in points)
            {
                x += point.X;
                y += point.Y;
            }
            x /= cut;
            y /= cut;

            return new Point(Convert.ToInt32(x), Convert.ToInt32(y));
        }

        private static FastBitmap[] getScreenShot(GetProgramWindow window)
        {
            Size size = new Size(window.rect.width, window.rect.height);
            Bitmap shot0 = new Bitmap(size.Width, size.Height);
            Graphics gf0 = Graphics.FromImage(shot0);
            gf0.CopyFromScreen(window.rect.left, window.rect.top, 0, 0, size);

            Thread.Sleep(20);

            Bitmap shot1 = new Bitmap(size.Width, size.Height);
            Graphics gf1 = Graphics.FromImage(shot1);
            gf1.CopyFromScreen(window.rect.left, window.rect.top, 0, 0, size);


            FastBitmap fast_shot0 = new FastBitmap(shot0);
            FastBitmap fast_shot1 = new FastBitmap(shot1);
            fast_shot0.Lock();
            fast_shot1.Lock();

            //debugOutput(shot0, fast_shot0, fast_shot1);

            return new FastBitmap[]{fast_shot0, fast_shot1};
        }

        //private static void debugOutput(Bitmap shot0, FastBitmap fast_shot0, FastBitmap fast_shot1)
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
        //    output.Save("difference.png");
        //}
    }
}
