using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
//Analytical Model in DB.Structure
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
    class RevitModelElements
    {
        public void GetBeamAndColumnSymbols(Document document)
        {
           
            FilteredElementCollector collector = new FilteredElementCollector(document);
            ICollection<Element> elements = collector.OfClass(typeof(Family)).ToElements();

            foreach (Element element in elements)
            {
                Family family = element as Family;
                Category category = family.FamilyCategory;
                if (null != category)
                {
                    ISet<ElementId> familySymbolIds = family.GetFamilySymbolIds();
                    if ((int)BuiltInCategory.OST_StructuralColumns == category.Id.IntegerValue)
                    {
                        foreach (ElementId id in familySymbolIds)
                        {
                            FamilySymbol symbol = family.Document.GetElement(id) as FamilySymbol;
                            AnalyticalModel analyticalModel = symbol.GetAnalyticalModel() as AnalyticalModel;
                        }
                    }
                    else if ((int)BuiltInCategory.OST_StructuralFraming == category.Id.IntegerValue)
                    {
                        foreach (ElementId id in familySymbolIds)
                        {
                            FamilySymbol symbol = family.Document.GetElement(id) as FamilySymbol;

                        }
                    }
                }
            }
        }
    }
}
