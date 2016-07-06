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
        private Family _RevitElement;
        private string _FamilyInstanceName;
        
        public void Set_AnalyticalModel(AnalyticalModel analyticalModel)
        {
            _AnalyticalModel = analyticalModel;
        }
        
        public void Set_RevitElement(Family RevitElement)
        {
            _RevitElement = RevitElement;
        }
        public void Set_FamilyInstanceName(string FamilyInstanceName)
        {
            _FamilyInstanceName = FamilyInstanceName;
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

