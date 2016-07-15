using Autodesk.Revit.DB;
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
        
        public Algorithm(List<EtabObject> list, string type)
        {
            if (type == "Start")
            {
                Set_StartPoints(list);
            }
            if (type == "End")
            {
                Set_EndPoints(list);
            }
        }

        public void Set_StartPoints(List<EtabObject> EtabPointList)
        {
            foreach (EtabObject p in EtabPointList)
            {
                _points.Add(p.Get_StartPoint());
            }
        }
        public void Set_EndPoints(List<EtabObject> EtabPointList)
        {
            foreach (EtabObject p in EtabPointList)
            {
                _points.Add(p.Get_EndPoint());
            }
        }

        public List<Point> Get_Points()
        {
            return _points;
        }

        public List<Point> Run(XYZ revitObj, List<Point> points)
        {
            return ClosestPair(points, revitObj);
        }

        static List<Point> ClosestPair(IEnumerable<Point> points, XYZ rvtObj)
        {
            var closestPair = new List<Point>();

            // When we start the min distance is the infinity
            var crtMinDist = MaxDistance;

            // Get the points and sort them
            var sorted = new List<Point>();
            sorted.AddRange(points);
            sorted.Sort(XComparer.XCompare);

            sorted.RemoveAll(x => x.X < rvtObj.X - crtMinDist || rvtObj.X + crtMinDist < x.X);
            sorted.RemoveAll(x => x.Y < rvtObj.Y - crtMinDist || rvtObj.Y + crtMinDist < x.Y);
            sorted.RemoveAll(x => x.Z < rvtObj.Z - crtMinDist || rvtObj.Z + crtMinDist < x.Z);
            
            return sorted;
        }
    }
}
