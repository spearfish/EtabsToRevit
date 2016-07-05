using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSB.RevitTools.EtabsToRevit
{
    class RevitObject
    {
        private AnalyticalModel _AnalyticalModel;
        private FamilySymbol _RevitElement;
        
        public void Set_AnalyticalModel(AnalyticalModel AnalyticalModel)
        {
            _AnalyticalModel = AnalyticalModel;
        }
        
        public void Set_RevitElement(FamilySymbol RevitElement)
        {
            _RevitElement = RevitElement;
        }
        public Get_AnalyticalModel()
        {
            return _AnalyticalModel;
        }
        public Get__RevitElement()
        {
            return _RevitElement;
        }
    }
}

