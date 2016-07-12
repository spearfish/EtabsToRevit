using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSB.RevitTools.EtabsToRevit
{
    public class Point : IComparable
    {
        private static int _counter;
        public int Id { get; private set; }
        public double X { get; set; } // x pos
        public double Y { get; set; } // y pos
        public double Z { get; set; } // y pos
        public Point()
        {
            Id = ++_counter;
        }
        public override string ToString() { return string.Format("Id: {0}; X: {1}; Y: {2}", Id, X, Y); }
        public override bool Equals(object obj)
        {
            var other = obj as Point;
            if (other == null) { return false; }
            return Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
        public int CompareTo(object obj)
        {
            var o = obj as Point;
            if (o == null) return -1;

            return XComparer.XCompare(this, o); // default
        }

        public double Distance(Point p)
        {
            var dx = p.X - X;
            var dy = p.Y - Y;

            var dist = (dx * dx) + (dy * dy);
            dist = Math.Sqrt(dist); // can be deleted

            return dist;
        }
    }
}
