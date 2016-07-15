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
        private Point _Point_Start = new Point();
        private Point _Point_End = new Point();
     
        // Start get and set start points
        public void Set_UniqueID(int UniqueID)
        {
            _Point_Start.UniqueID = UniqueID;
            _Point_End.UniqueID = UniqueID;
        }
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

        public int Get_UniqueID()
        {
            return _Point_Start.UniqueID;            
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
        
        // End get and set start points
        public void Set_End_X(double End_X)
        {
            _Point_End.X = End_X;
        }
        public void Set_End_Y(double End_Y)
        {
            _Point_End.Y = End_Y;
        }
        public void Set_End_Z(double End_Z)
        {
            _Point_End.Z = End_Z;
        }
        public double Get_End_X()
        {
            return _Point_End.X;
        }
        public double Get_End_Y()
        {
            return _Point_End.Y;
        }
        public double Get_End_Z()
        {
            return _Point_End.Z;
        }
        public Point Get_EndPoint()
        {
            return _Point_End;
        }
        
    }
}
