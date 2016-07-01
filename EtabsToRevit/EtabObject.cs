using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSB.RevitTools.EtabsToRevit
{
    class EtabObject
    {
        private string SectionName;
        private double _Start_X;
        private double _Start_Y;
        private double _Start_Z;
        private double _End_X;
        private double _End_Y;
        private double _End_Z;

        public void Set_Start_X(double Start_X)
        {
            _Start_X = Start_X;
        }
    }
}
