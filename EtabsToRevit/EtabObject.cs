using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSB.RevitTools.EtabsToRevit
{
    class EtabObject
    {
        public string _SectionName { get; set; }
        public string _LabelNumber { get; set; }
        public int _UniqueID {get; set;}

        private Point _Point_Start = new Point();
        public void Set_Start_X(double Start_X)
        {
            _Point_Start.X = Start_X;
        }
        public void Set_Start_Y(double Start_Y)
        {
            _Point_Start.Y = Start_Y;
        }
        public void Set_Start_Z(double Start_Z)
        {
            _Point_Start.Z = Start_Z;
        }
        public double Get_Start_X()
        {
            return _Point_Start.X;
        }
        public double Get_Start_Y()
        {
            return _Point_Start.Y;
        }
        public double Get_Start_Z()
        {
            return _Point_Start.Z;
        }
        public Point Get_StartPoint()
        {
            return _Point_Start;
        }

        public double _End_X {get; set;}
        public double _End_Y {get; set;}
        public double _End_Z {get; set;}
        

        
    }
}
