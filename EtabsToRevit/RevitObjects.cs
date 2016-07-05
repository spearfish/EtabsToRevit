using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
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
        public AnalyticalModel Get_AnalyticalModel()
        {
            return _AnalyticalModel;
        }
        public FamilySymbol Get__RevitElement()
        {
            return _RevitElement;
        }
    }
}

