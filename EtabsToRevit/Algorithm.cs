using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C5;

namespace DSB.RevitTools.EtabsToRevit
{
    class Algorithm
    {
        readonly List<Point> _points = new List<Point>();
        public int Distance { get; private set; }
        const double MaxDistance = 2;

        public Algorithm(List<EtabObject> EtabList)
        {
            foreach (EtabObject EtabObj in EtabList)
            {
                _points.Add(EtabObj.Get_StartPoint());
            }
            
            _points.Sort();
        }
        
        public Point Run(RevitObject revitObj)
        {
            return ClosestPair(_points, revitObj);
        }
        public Point[] Run2()
        {
            return NaiveClosestPair(_points);
        }
        
        static Point ClosestPair(IEnumerable<Point> points, RevitObject revitObj)
        {
            var closestPair = new Point();

            // When we start the min distance is the infinity
            var crtMinDist = MaxDistance;

            // Get the points and sort them
            var sorted = new List<Point>();
            sorted.AddRange(points);
            sorted.Sort(XComparer.XCompare);

            // TODO Replace this with XYZ coord 
            Point current = new Point();
            current.X = revitObj.Get_PointStart().X;
            current.Y = revitObj.Get_PointStart().Y;
            current.Z = revitObj.Get_PointStart().Z;
            sorted.RemoveAll(x => x.X < current.X - crtMinDist || current.X + crtMinDist < x.X);
            sorted.RemoveAll(x => x.Y < current.Y - crtMinDist || current.Y + crtMinDist < x.Y);
            sorted.RemoveAll(x => x.Z < current.Z - crtMinDist || current.Z + crtMinDist < x.Z);

            var sortedX = sorted;
            
            return closestPair;
        }
    

        static Point[] NaiveClosestPair(IEnumerable<Point> points)
        {
            var min = MaxDistance;

            var closestPair = new Point[2];

            foreach (var p1 in points)
            {
                foreach (var p2 in points)
                {
                    if (p1.Equals(p2)) continue;

                    var dist = p1.Distance(p2);
                    if (dist < min)
                    {
                        min = dist;
                        closestPair[0] = p1;
                        closestPair[1] = p2;
                    }
                }
            }
            return closestPair;
        }
    }
}
