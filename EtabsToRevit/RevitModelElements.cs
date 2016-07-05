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
            List<RevitObject> RevitColumns = new List<RevitObject>();
            List<RevitObject> RevitFraming = new List<RevitObject>();

            ElementClassFilter familyInstanceFilter = new ElementClassFilter(typeof(FamilyInstance));
            ElementCategoryFilter ColumnCategoryfilter = new ElementCategoryFilter(BuiltInCategory.OST_StructuralColumns);
            LogicalAndFilter ColumnInstancesFilter = new LogicalAndFilter(familyInstanceFilter, ColumnCategoryfilter);
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            ICollection<Element> elements = collector.WherePasses(ColumnInstancesFilter).ToElements();

            foreach (Element e in elements)
            {
                // if the element is structural footing
                FamilyInstance familyInstance = e as FamilyInstance;

                if (null != familyInstance && familyInstance.StructuralType == StructuralType.Column)
                {
                    string familyInstanceName = familyInstance.Name;
                    var type = familyInstance.GetType();
                    Family family = familyInstance.Symbol.Family;
                    AnalyticalModel model = familyInstance.GetAnalyticalModel();
                }
            }
        }
    }
}
