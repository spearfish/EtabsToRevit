using Autodesk.Revit.DB;
using System;
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
            
            Transaction trans = new Transaction( doc, "Edit Type" );
            trans.Start();
            familySymbol symbol = Util.FindElementByName(doc, typeof(FamilySymbol), _typeName) as FamilySymbol; 
            FamilyInstance revitFamilyInstance = revitObject.Get_FamilyInstanceName();
            revitFamilyInstance.Symbol = symbol; 
            trans.Commit();
        }
    }
}
