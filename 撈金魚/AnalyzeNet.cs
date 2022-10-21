using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static 撈金魚.GetProgramWindow;

namespace 撈金魚
{
    internal class AnalyzeNet
    {
        private static int net_radius;
        private static int[] circle;

        public static void refreshRadius(int radius)
        {
            net_radius = radius;
            circle = new int[net_radius];
            for(int i=0; i<radius; i++)
            {
                circle[i] = Convert.ToInt32(Math.Sqrt(radius * radius - i * i)); //內建四捨五入(0.5=0, 0.5+ 1e-28M = 1)
            }
        }

        public static Point CalculateBestPoint(Point[] points, MyRect rect, MainWindow main)
        {
            SortedSet<FixPoint>[] fixes = new SortedSet<FixPoint>[rect.height];
            int max_point = 0;
            List<int> maxes_y = new List<int>();

            for (int i = 0; i < rect.height; i++)
                fixes[i] = new SortedSet<FixPoint>();

            foreach (Point pt in points)
            {
                Addfixes(fixes, pt);
            }

            for (int i = 0; i < rect.height; i++)
            {
                int _pts = getMaxPoints(fixes[i]);
                if (_pts > max_point)
                {
                    max_point = _pts;
                    maxes_y.Clear();
                }
                if(_pts >= max_point)
                    maxes_y.Add(i);
            }

            //if (max_point < 3) return new Point(-1, -1);
            //main.addNum(max_point);
            int best_y = maxes_y[maxes_y.Count / 2];
            int best_x = getBestPoint(fixes[best_y], rect);
            return new Point(best_x, best_y);
        }

        private static int getBestPoint(SortedSet<FixPoint> fixPoints, MyRect rect)
        {
            int max = getMaxPoints(fixPoints), tmp = 0;
            int x1=0, x2 = rect.width;
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

        private static int getMaxPoints(SortedSet<FixPoint> fixPoints)
        {
            int max = 0, tmp=0;
            foreach(FixPoint pt in fixPoints)
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
            }catch (IndexOutOfRangeException) { }
        }

        private struct FixPoint : IComparable<FixPoint>
        {
            public int x;
            public int fix;

            public FixPoint(int x, int fix)
            {
                this.x = x;
                this.fix = fix;
            }

            public int CompareTo(FixPoint to_compare)
            {
                if (x == to_compare.x) return 1;
                return x.CompareTo(to_compare.x);
            }
        }
    }
}
