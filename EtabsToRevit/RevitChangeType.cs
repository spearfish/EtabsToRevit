using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSB.RevitTools.EtabsToRevit
{
    class RevitChangeType
    {
        public void changeTypeColumn(Document doc, RevitObject revitObject, EtabObject etabObject, List<FamilySymbol> structuralColumnTypeList)
        {
            string _typeName = etabObject._SectionName;
            FamilySymbol symbol = structuralColumnTypeList.Find(x => x.Name == _typeName);
            // Check to see if element type exist in the project
            if (symbol == null)
            {
                loadFamily(doc, etabObject, structuralColumnTypeList);
            }
            // Transaction to change the element type 
            Transaction trans = new Transaction( doc, "Edit Type" );
            trans.Start();
            try
            {
                FamilyInstance revitFamilyInstance = revitObject.Get_FamilyInstance();
                revitFamilyInstance.Symbol = symbol; 
            }
            catch { }
            trans.Commit();
        }

        public void changeTypeFraming(Document doc, RevitObject revitObject, EtabObject etabObject, List<FamilySymbol> structuralFramingTypeList)
        {
            string _typeName = etabObject._SectionName;
            FamilySymbol symbol = structuralFramingTypeList.Find(x => x.Name == _typeName);

            Transaction trans = new Transaction(doc, "Edit Type");
            trans.Start();
            try
            {
                FamilyInstance revitFamilyInstance = revitObject.Get_FamilyInstance();
                revitFamilyInstance.Symbol = symbol;
            }
            catch { }
            trans.Commit();
        }



        static string FamilyName = "WWF-Welded Wide Flange-Column.rfa";
        const string _familyFolder = @"X:\Revit\Revit Unifi Library\z-Autodesk OOTB Revit 2015 Family Library\Libraries\US Imperial\Structural Columns\Steel";
       // const string _familyFolder = Path.GetDirectoryName(typeof(FamilySymbol).Assembly.Location);
        const string _familyExt = "rfa";
        static string _family_path = null;

        public void loadFamily(Document doc, EtabObject etabObject, List<FamilySymbol> structuralColumnTypeList)
        {
            string _typeName = etabObject._SectionName;

            FilteredElementCollector collector= new FilteredElementCollector(doc);
            collector.OfClass(typeof(Family)).ToElements();
            IEnumerable<Family> nestedFamilies = collector.Cast<Family>();
            Family family = null;
           
            
            string familypath = FilePath; // method file path
            using (Transaction tx = new Transaction(doc))
            {
                tx.Start("Load Family");
                doc.LoadFamily(familypath, out family);
                tx.Commit();
            }
            FamilySymbol symbol = null;

            foreach (FamilySymbol s in structuralColumnTypeList)
            {
                if (s.Name == _typeName)
                {
                    symbol = s;
                }
                break;
            }
        }
        static string FilePath
        {
            get
            {
                if (null == _family_path)
                {
                    _family_path = Path.Combine(_familyFolder, FamilyName);
                    _family_path = Path.Combine(_family_path);
                }
                return _family_path;
            }
        }
    }

    // Loading family type override options
    class FamilyLoadingOverwriteOption : IFamilyLoadOptions
    {
        public bool OnFamilyFound(bool familyInUse, out bool overwriteParameterValues)
        {
            overwriteParameterValues = true;
            return true;
        }

        public bool OnSharedFamilyFound(Family sharedFamily, bool familyInUse, out FamilySource source, out bool overwriteParameterValues)
        {
            source = FamilySource.Family;
            overwriteParameterValues = true;
            return true;
        }
    }
}
