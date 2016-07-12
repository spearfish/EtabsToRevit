using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSB.RevitTools.EtabsToRevit
{
    public class YComparer : IComparer<Point>
    {
        public int Compare(Point p1, Point p2)
        {
            return YCompare(p1, p2);
        }

        public static int YCompare(Point p1, Point p2)
        {
            if (p1.Y < p2.Y) return -1;
            if (p1.Y > p2.Y) return 1;
            if (p1.X < p2.X) return -1;
            if (p1.X > p2.X) return 1;
            return 0;
        }
    }
}
