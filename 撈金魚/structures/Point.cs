using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 撈金魚.structures
{
    public class Points
    {
        public struct FixPoint : IComparable<FixPoint>
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
