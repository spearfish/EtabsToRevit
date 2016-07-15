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
        public void changeType(Document doc, List<RevitObject> revitObjectList)
        {
            foreach (RevitObject revitObject in revitObjectList)
            {
                Element element = revitObject.Get_Element();

                Transaction trans = new Transaction( doc, "Edit Type" );
                trans.Start();
                //element.ChangeTypeId( ladderType.Id );
                trans.Commit();
            }
        }
    }
}
