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
        public void GetBeamAndColumnSymbols(Document doc)
        {
            List<RevitObject> RevitColumnsList = new List<RevitObject>();
            List<RevitObject> RevitFramingList = new List<RevitObject>();

            ElementClassFilter familyInstanceFilter = new ElementClassFilter(typeof(FamilyInstance));
            // Category filter 
            ElementCategoryFilter ColumnCategoryfilter = new ElementCategoryFilter(BuiltInCategory.OST_StructuralColumns);
            ElementCategoryFilter FramingCategoryfilter = new ElementCategoryFilter(BuiltInCategory.OST_StructuralFaming);
            // Instance filter 
            LogicalAndFilter ColumnInstancesFilter = new LogicalAndFilter(familyInstanceFilter, ColumnCategoryfilter);
            LogicalAndFilter FramingInstancesFilter = new LogicalAndFilter(familyInstanceFilter, FramingCategoryfilter);
            
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            // Colletion Array of Elements
            ICollection<Element> ColumnElements = collector.WherePasses(ColumnInstancesFilter).ToElements();
            ICollection<Element> FramingElements = collector.WherePasses(FramingInstancesFilter).ToElements();

            foreach (Element e in ColumnElements)
            {
                FamilyInstance familyInstance = e as FamilyInstance;

                if (null != familyInstance && familyInstance.StructuralType == StructuralType.Column)
                {
                    AnalyticalModel analyticalModel = familyInstance.GetAnalyticalModel();
                    Family family = familyInstance.Symbol.Family;
                    string familyInstanceName = familyInstance.Name;
                    var type = familyInstance.GetType();
                    //Create Revit Object and Set Methods 
                    RevitObject revitObject = new RevitObject();
                    revitObject.Set_AnalyticalModel(analyticalModel);
                    revitObject.Set_RevitElement(family);
                    revitObject.Set_FamilyInstanceName(familyInstanceName);
                    RevitColumnsList.Add(revitObject);
                }
            }
            
            foreach (Element e in FramingElements)
            {
                FamilyInstance familyInstance = e as FamilyInstance;

                if (null != familyInstance && familyInstance.StructuralType == StructuralType.Framing)
                {
                    AnalyticalModel analyticalModel = familyInstance.GetAnalyticalModel();
                    Family family = familyInstance.Symbol.Family;
                    string familyInstanceName = familyInstance.Name;
                    var type = familyInstance.GetType();
                    //Create Revit Object and Set Methods 
                    RevitObject revitObject = new RevitObject();
                    revitObject.Set_AnalyticalModel(analyticalModel);
                    revitObject.Set_RevitElement(family);
                    revitObject.Set_FamilyInstanceName(familyInstanceName);
                    RevitFramingList.Add(revitObject);
                }
            }
        }
    }
}
