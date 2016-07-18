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
        // Divide the points int to a list of start and end points
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

        // set the start points in to a list 
        public void Set_StartPoints(List<EtabObject> EtabPointList)
        {
            foreach (EtabObject p in EtabPointList)
            {
                _points.Add(p.Get_StartPoint());
            }
        }
        // set the end points into a list 
        public void Set_EndPoints(List<EtabObject> EtabPointList)
        {
            foreach (EtabObject p in EtabPointList)
            {
                _points.Add(p.Get_EndPoint());
            }
        }

        // Retrive points from start or end list of points 
        public List<Point> Get_Points()
        {
            return _points;
        }
        // Run algorithm by sending in points and comparing to the Revit element anaylsis curve 
        public List<Point> Run(XYZ revitObj, List<Point> points)
        {
            return ClosestPair(points, revitObj);
        }

        /* Algorithm sorts X value then finds the strip in X then 
        finds a strip in Y then finds a strip in Z making a box. */
        static List<Point> ClosestPair(IEnumerable<Point> points, XYZ rvtObj)
        {
            var closestPair = new List<Point>();

            // When we start the min distance is the infinity
            var crtMinDist = MaxDistance;

            // Get the points and sort them
            var sorted = new List<Point>();
            sorted.AddRange(points);
            sorted.Sort(XComparer.XCompare);
            //create box to search nearest points 
            sorted.RemoveAll(x => x.X < rvtObj.X - crtMinDist || rvtObj.X + crtMinDist < x.X);
            sorted.RemoveAll(x => x.Y < rvtObj.Y - crtMinDist || rvtObj.Y + crtMinDist < x.Y);
            sorted.RemoveAll(x => x.Z < rvtObj.Z - crtMinDist || rvtObj.Z + crtMinDist < x.Z);
            
            return sorted;
        }
    }
}
