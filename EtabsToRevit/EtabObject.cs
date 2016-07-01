using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSB.RevitTools.EtabsToRevit
{
    class EtabObject
    {
        private string _SectionName;
        private string _LabelNumber;
        private double _Start_X;
        private double _Start_Y;
        private double _Start_Z;
        private double _End_X;
        private double _End_Y;
        private double _End_Z;
        private int _UniqueID;

        public void Set_SectionName(string Section_Name)
        {
            _SectionName = Section_Name;
        }
        public void Set_LabelNumber(string LabelName)
        {
            _LabelNumber = LabelName;
        }
        public void Set_Start_X(double Start_X)
        {
            _Start_X = Start_X;
        }
        public void Set_Start_Y(double Start_Y)
        {
            _Start_Y = Start_Y;
        }
        public void Set_Start_Z(double Start_Z)
        {
            _Start_Z = Start_Z;
        }
        public void Set_End_X(double End_X)
        {
            _End_X = End_X;
        }
        public void Set_End_Y(double End_Y)
        {
            _End_Y = End_Y;
        }
        public void Set_End_Z(double End_Z)
        {
            _End_Z = End_Z;
        }
        public void Set_UniqueID(int UniqueID)
        {
            _UniqueID = UniqueID;
        }

        public string Get_SectionName()
        {
            return _SectionName;
        }
        public string Get_LabelNumber()
        {
            return _LabelNumber;
        }
        public double Get_Start_X()
        {
            return _Start_X;
        }
    }
}
