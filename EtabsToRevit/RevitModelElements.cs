using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
//Analytical Model in DB.Structure
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSB.RevitTools.EtabsToRevit
{
    class RevitModelElements
    {
        private List<RevitObject> _RevitColumnsList = new List<RevitObject>();
        private List<RevitObject> _RevitFramingList = new List<RevitObject>();

        public void Get_ColumnSymbols(Document doc)
        {
            ElementClassFilter familyInstanceFilter = new ElementClassFilter(typeof(FamilyInstance));
            // Category filter 
            ElementCategoryFilter ColumnCategoryfilter = new ElementCategoryFilter(BuiltInCategory.OST_StructuralColumns);
            // Instance filter 
            LogicalAndFilter ColumnInstancesFilter = new LogicalAndFilter(familyInstanceFilter, ColumnCategoryfilter);
            
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            // Colletion Array of Elements
            ICollection<Element> ColumnElements = collector.WherePasses(ColumnInstancesFilter).ToElements();

            foreach (Element e in ColumnElements)
            {
                FamilyInstance familyInstance = e as FamilyInstance;

                if (null != familyInstance && familyInstance.StructuralType == StructuralType.Column)
                {
                    Element element = e;
                    AnalyticalModel analyticalModel = familyInstance.GetAnalyticalModel();
                    Curve analyticalCurve = analyticalModel.GetCurve();
                    XYZ pointStart = analyticalCurve.GetEndPoint(0);
                    XYZ pointEnd = analyticalCurve.GetEndPoint(1);
                    Family family = familyInstance.Symbol.Family;
                    
                    //Create Revit Object and Set Methods 
                    RevitObject revitObject = new RevitObject();
                    revitObject.Set_Element(element);
                    revitObject.Set_AnalyticalModel(analyticalModel);
                    revitObject.Set_PointStart(pointStart);
                    revitObject.Set_PointEnd(pointEnd);
                    revitObject.Set_RevitFamily(family);
                    revitObject.Set_FamilyInstance(familyInstance);
                    _RevitColumnsList.Add(revitObject);
                }
            }
        }

        public void Get_BeamSymbols(Document doc)
        {
            ElementClassFilter familyInstanceFilter = new ElementClassFilter(typeof(FamilyInstance));
            // Category filter 
            ElementCategoryFilter FramingCategoryfilter = new ElementCategoryFilter(BuiltInCategory.OST_StructuralFraming);
            // Instance filter 
            LogicalAndFilter FramingInstancesFilter = new LogicalAndFilter(familyInstanceFilter, FramingCategoryfilter);

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            // Colletion Array of Elements
            ICollection<Element> FramingElements = collector.WherePasses(FramingInstancesFilter).ToElements();

            foreach (Element e in FramingElements)
            {
                FamilyInstance familyInstance = e as FamilyInstance;

                if (null != familyInstance && familyInstance.StructuralType == StructuralType.Beam)
                {
                    Element element = e;
                    AnalyticalModel analyticalModel = familyInstance.GetAnalyticalModel();
                    Curve analyticalCurve = analyticalModel.GetCurve();
                    XYZ pointStart = analyticalCurve.GetEndPoint(0);
                    XYZ pointEnd = analyticalCurve.GetEndPoint(1);
                    Family family = familyInstance.Symbol.Family;
                    
                    //Create Revit Object and Set Methods 
                    RevitObject revitObject = new RevitObject();
                    revitObject.Set_Element(element);
                    revitObject.Set_AnalyticalModel(analyticalModel);
                    revitObject.Set_PointStart(pointStart);
                    revitObject.Set_PointEnd(pointEnd);
                    revitObject.Set_RevitFamily(family);
                    revitObject.Set_FamilyInstance(familyInstance);
                    _RevitFramingList.Add(revitObject);
                }
            }
        }

        public List<RevitObject> Get_RevitColumnsList()
        {
            return _RevitColumnsList;
        }
        public List<RevitObject> Get_RevitFramingList()
        {
            return _RevitFramingList;
        }
    }
}
