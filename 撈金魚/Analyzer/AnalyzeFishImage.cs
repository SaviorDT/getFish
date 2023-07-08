using Hazdryx.Drawing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using 撈金魚.ActionPerform;
using 撈金魚.structures;
using static 撈金魚.structures.Points;
using static 撈金魚.structures.WindowPack;
using Point = System.Drawing.Point;

namespace 撈金魚.Analyzer
{
    internal class AnalyzeFishImage
    {
        private static int net_radius;
        private static int[] circle;

        internal static bool FishSucessed(WindowSource source)
        {
            FastBitmap shot = ScreenAction.GetContentShot(source);
            bool successed = ImageDetermine.FindNormalConfirmDialog(shot);

            shot.Dispose();
            return successed;
        }
        public static void RefreshRadius(int radius)
        {
            if (net_radius == radius)
                return;
            net_radius = radius;
            circle = new int[net_radius];
            for (int i = 0; i < radius; i++)
            {
                circle[i] = Convert.ToInt32(Math.Sqrt(radius * radius - i * i)); //內建四捨五入(0.5=0, 0.5+ 1e-28M = 1)
            }
        }

        public static Point CalculateBestPoint(Point[] points, WindowRect rect)
        {
            SortedSet<FixPoint>[] fixes = new SortedSet<FixPoint>[rect.Height];
            int max_point = 0;
            List<int> maxes_y = new();

            for (int i = 0; i < rect.Height; i++)
                fixes[i] = new SortedSet<FixPoint>();

            foreach (Point pt in points)
            {
                Addfixes(fixes, pt);
            }

            for (int i = 0; i < rect.Height; i++)
            {
                int _pts = GetMaxPoints(fixes[i]);
                if (_pts > max_point)
                {
                    max_point = _pts;
                    maxes_y.Clear();
                }
                if (_pts >= max_point)
                    maxes_y.Add(i);
            }

            //if (max_point < 3) return new Point(-1, -1);
            //main.addNum(max_point);
            int best_y = maxes_y[maxes_y.Count / 2];
            int best_x = GetBestPoint(fixes[best_y], rect);
            return new Point(best_x, best_y);
        }

        private static int GetBestPoint(SortedSet<FixPoint> fixPoints, WindowRect rect)
        {
            int max = GetMaxPoints(fixPoints), tmp = 0;
            int x1 = 0, x2 = rect.Width;
            bool found = false;

            foreach (FixPoint pt in fixPoints)
            {
                if (found)
                {
                    x2 = pt.x;
                    break;
                }
                tmp += pt.fix;
                if (tmp == max)
                {
                    found = true;
                    x1 = pt.x;
                }
            }

            return (x1 + x2) / 2;
        }

        private static int GetMaxPoints(SortedSet<FixPoint> fixPoints)
        {
            int max = 0, tmp = 0;
            foreach (FixPoint pt in fixPoints)
            {
                tmp += pt.fix;
                if (tmp > max)
                    max = tmp;
            }

            return max;
        }

        private static void Addfixes(SortedSet<FixPoint>[] fixes, Point pt)
        {
            try
            {
                fixes[pt.Y].Add(new FixPoint(pt.X - net_radius, 1));
                fixes[pt.Y].Add(new FixPoint(pt.X + net_radius, -1));
                for (int i = 1; i < net_radius; i++)
                {
                    int dy = i, dx = circle[i];
                    fixes[pt.Y - dy].Add(new FixPoint(pt.X - dx, 1));
                    fixes[pt.Y - dy].Add(new FixPoint(pt.X + dx, -1));
                    fixes[pt.Y + dy].Add(new FixPoint(pt.X - dx, 1));
                    fixes[pt.Y + dy].Add(new FixPoint(pt.X + dx, -1));
                }
            }
            catch (IndexOutOfRangeException) { }
        }
        public static Point[] FindFish(FastBitmap[] shot)
        {
            (int bucket_x, int bucket_y) = ProgramPointTranslator.MoleToContent(
                shot[0].Width, shot[0].Height,
                GoldenFishPlayer.BUCKET_NET_X, GoldenFishPlayer.BUCKET_NET_Y);
            //int bucket_x = MainWindow.BUCKET_WATER_X * shot[0].Width / MainWindow.MOLE_W;
            //int bucket_y = MainWindow.BUCKET_WATER_Y * shot[0].Height / MainWindow.MOLE_H;

            Size size = new(shot[0].Width, shot[0].Height);
            Point cdot = new(size.Width / 2 - 1, size.Height / 2 - 1);
            int max_radius = size.Width / 2 + size.Height / 2 - 1;
            int radius_base = 3;
            ArrayList points = new();
            bool[,] scanned = new bool[shot[0].Width, shot[0].Height];

            for (int i = 0; i < size.Width; i++)
                for (int j = 0; j < size.Height; j++)
                    scanned[i, j] = false;

            for (int radius = 0; radius < max_radius && points.Count < 6; radius += radius_base)
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
                    if ((!scanned[x, y]) && shot[0].GetI(x, y) != shot[1].GetI(x, y))
                    {
                        points.Add(ScanFish(shot, x, y, scanned));
                        if (points.Count >= 6)
                            break;
                    }

                    y = cdot.Y + y_len;
                    if (x < bucket_x && y > bucket_y) { }
                    else if ((!scanned[x, y]) && shot[0].GetI(x, y) != shot[1].GetI(x, y))
                    {
                        points.Add(ScanFish(shot, x, y, scanned));
                        if (points.Count >= 6)
                            break;
                    }

                    x += radius_base;
                    y_len += (x < size.Width / 2) ? radius_base : -radius_base;

                    if (y_len >= size.Height / 2)
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
            ArrayList points = new();
            Queue<Point> to_scan = new();

            points.Add(new Point(x, y));
            scanned[x, y] = true;
            MarkAround(to_scan, x, y, scanned);

            while (to_scan.Count > 0)
            {
                Point pt = to_scan.Dequeue();
                if (shot[0].GetI(pt.X, pt.Y) != shot[1].GetI(pt.X, pt.Y))
                {
                    points.Add(pt);
                    MarkAround(to_scan, pt.X, pt.Y, scanned);
                }
            }

            //savePoints(points, shot);
            return CalculateCenterPoint(points);
        }

        private static void MarkAround(Queue<Point> to_scan, int x, int y, bool[,] scanned)
        {
            for (int i = x - 2; i <= x + 2; i++)
            {
                for (int j = y - 2; j <= y + 2; j++)
                {
                    try
                    {
                        if (!scanned[i, j])
                        {
                            to_scan.Enqueue(new Point(i, j));
                            scanned[i, j] = true;
                        }
                    }
                    catch (System.IndexOutOfRangeException) { }
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

        private static Point CalculateCenterPoint(ArrayList points)
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
    }
}
