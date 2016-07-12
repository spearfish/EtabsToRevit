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
        const double MaxDistance = 5;

        public Algorithm(List<EtabObject> EtabList)
        {
            foreach (EtabObject EtabObj in EtabList)
            {
                _points.Add(EtabObj.Get_StartPoint());
            }
            
            _points.Sort();
        }

        public Point[] Run(RevitObject revitObj)
        {
            return ClosestPair(_points, revitObj);
        }
        public Point[] Run2()
        {
            return NaiveClosestPair(_points);
        }

        static Point[] ClosestPair(IEnumerable<Point> points, RevitObject revitObj)
        {
            var closestPair = new Point[2];

            // When we start the min distance is the infinity
            var crtMinDist = MaxDistance;

            // Get the points and sort them
            var sorted = new List<Point>();
            sorted.AddRange(points);
            sorted.Sort(XComparer.XCompare);

            // When we start the left most candidate is the first one
            var leftMostCandidateIndex = 0;

            // Vertically sorted set of candidates            
            var candidates = new TreeSet<Point>(new YComparer()); // C5 data structure

            // For each point from left to right
            foreach (var current in sorted)
            {
                // Shrink the candidates (current.X
                while (revitObj.Get_PointStart().X - sorted[leftMostCandidateIndex].X > crtMinDist)
                {
                    candidates.Remove(sorted[leftMostCandidateIndex]);
                    leftMostCandidateIndex++;
                }

                // Compute the y head and the y tail of the candidates set
                var head = new Point { X = revitObj.Get_PointStart().X, Y = checked(revitObj.Get_PointStart().Y - crtMinDist), Z = revitObj.Get_PointStart().Z };
                var tail = new Point { X = revitObj.Get_PointStart().X, Y = checked(revitObj.Get_PointStart().Y + crtMinDist), Z = revitObj.Get_PointStart().Z };

                // We take only the interesting candidates in the y axis                
                var subset = candidates.RangeFromTo(head, tail);

                foreach (var point in subset)
                {
                    var distance = current.Distance(point);
                    if (distance < 0) throw new ApplicationException("number overflow");

                    // Simple min computation
                    if (distance < crtMinDist)
                    {
                        crtMinDist = distance;
                        closestPair[0] = current;
                        closestPair[1] = point;
                    }
                }

                // The current point is now a candidate
                candidates.Add(current);
            }

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
