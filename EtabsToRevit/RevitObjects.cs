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
        private XYZ _PointStart;
        private XYZ _PointEnd;
        private Family _RevitFamily;
        private string _FamilyInstanceName;
        
        public void Set_AnalyticalModel(AnalyticalModel analyticalModel)
        {
            _AnalyticalModel = analyticalModel;
        }
        public void Set_PointStart(XYZ PointStart)
        {
            _PointStart = PointStart;
        }
        public void Set_PointEnd(XYZ PointEnd)
        {
            _PointEnd = PointEnd;
        }
        public void Set_RevitFamily(Family RevitFamily)
        {
            _RevitFamily = RevitFamily;
        }
        public void Set_FamilyInstanceName(string FamilyInstanceName)
        {
            _FamilyInstanceName = FamilyInstanceName;
        }
        

        //Get Object Elements
        public AnalyticalModel Get_AnalyticalModel()
        {
            return _AnalyticalModel;
        }
        public XYZ Get_PointStart()
        {
            return _PointStart;
        }
        public XYZ Get_PointEnd()
        {
            return _PointEnd;
        }
        public Family Get_RevitFamily()
        {
            return _RevitFamily;
        }
        public string Get_FamilyInstanceName()
        {
            return _FamilyInstanceName;
        }
    }
}

