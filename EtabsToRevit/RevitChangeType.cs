using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSB.RevitTools.EtabsToRevit
{
    class RevitChangeType
    {
        public void changeType(Document doc, UIDocument uidoc, RevitObject revitObject, EtabObject etabObject)
        {
            string _typeName = etabObject._SectionName;

            var structuralColumnType = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol)).OfCategory(BuiltInCategory.OST_StructuralColumns);
            List<FamilySymbol> structuralColumnTypeList = structuralColumnType.ToElements().Cast<FamilySymbol>().ToList();

            Transaction trans = new Transaction( doc, "Edit Type" );
            trans.Start();
            try
            {
                FamilySymbol symbol = structuralColumnTypeList.Find(x => x.Name == _typeName);
                FamilyInstance revitFamilyInstance = revitObject.Get_FamilyInstance();
                revitFamilyInstance.Symbol = symbol; 
            }
            catch { }
            trans.Commit();
        }
    }
}
