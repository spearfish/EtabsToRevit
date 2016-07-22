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
using System.Text.RegularExpressions;

namespace DSB.RevitTools.EtabsToRevit
{
    class RevitChangeType
    {
        public void changeType(Document doc, RevitObject revitObject, EtabObject etabObject, List<FamilySymbol> structuralTypeList)
        {
            string _typeName = etabObject._SectionName;
            FamilySymbol symbol = structuralTypeList.Find(x => x.Name == etabObject._SectionName);

            // Check to see if element type exist in the project
            if (symbol == null)
            {
                // if element doesn't exist in project method will try to load family. 
                symbol = loadFamily(doc, etabObject);
            }

            // Transaction to change the element type 
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

        // const string _familyFolder = Path.GetDirectoryName(typeof(FamilySymbol).Assembly.Location);
        const string _familyFolderColumnSteel = @"X:\Revit\Revit Unifi Library\z-Autodesk OOTB Revit 2015 Family Library\Libraries\US Imperial\Structural Columns\Steel";
        const string _familyFolderFramingSteel = @"X:\Revit\Revit Unifi Library\z-Autodesk OOTB Revit 2015 Family Library\Libraries\US Imperial\Structural Framing\Steel";
        static string _family_path = null;

        public FamilySymbol loadFamily(Document doc, EtabObject etabObject)
        {
            //using regular expression to search for the abbriviation of shape
            //var typeNameRegex_Expression = @"^[A-Z]*\D";
            //var typeNameRegex = Regex.Match(etabObject._SectionName, typeNameRegex_Expression);
            string returnFamilyPath;

            // test to see if Etab object is a column 
            if (etabObject._LabelNumber.Contains("C"))
            {
                ColumnDictionary().TryGetValue(etabObject._SectionType, out returnFamilyPath);
                // Retrive file name from dictionary using the appriviation
                _family_path = returnFamilyPath;
            }
            // test to see if Etab object is a beam
            if (etabObject._LabelNumber.Contains("B"))
            {
                FramingDictionary().TryGetValue(etabObject._SectionType, out returnFamilyPath);
                // Retrive file name from dictionary using the appriviation
                _family_path = returnFamilyPath;
            }

            FamilySymbol familySymbol = null; // Empty Family Symbol which will return the loaded family

            // Transaction to load a family and list all its types 
            using (Transaction tx = new Transaction(doc))
            {
                tx.Start("Load Family");
                Family family = null;
                string symbName = null;
                if (doc.LoadFamily(_family_path, new FamilyLoadingOverwriteOption(), out family))
                {
                    foreach (ElementId fsids in family.GetFamilySymbolIds())
                    {
                        ElementType elemtype = doc.GetElement(fsids) as ElementType;
                        FamilySymbol symb = elemtype as FamilySymbol;
                        // test to see if family symbol is the same type as Etabs Object. 
                        if (symb.Name == etabObject._SectionName)
                        {
                            symbName = symb.Name;
                        }
                    }
                }
                tx.RollBack(); // Roll back to only select the type that we want. 

                // Transaction to load the the family type. 
                Transaction transNew = new Transaction(doc, "RealLoading");
                transNew.Start();
                try
                {
                    doc.LoadFamilySymbol(_family_path, symbName, out familySymbol); // load only the family type 
                }
                catch { }
                transNew.Commit();
            }
            return familySymbol;
        }

        // Dictionary to reference Etab section type and Revit family file. 
        private static Dictionary<string, string> ColumnDictionary()
        {
            Dictionary<String, String> ColumnName = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            ColumnName.Add("Steel I/Wide Flange", Path.Combine(_familyFolderColumnSteel, "W-Wide Flange-Column.rfa"));
            ColumnName.Add("Steel Tube", Path.Combine(_familyFolderColumnSteel, "HSS-Hollow Structural Section-Column.rfa"));
            ColumnName.Add("Steel Pipe", Path.Combine(_familyFolderColumnSteel, "HSS-Round Hollow Structural Section-Column.rfa"));
            return ColumnName;
        }
        private static Dictionary<string, string> FramingDictionary()
        {
            Dictionary<String, String> FramingName = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            FramingName.Add("Steel I/Wide Flange", Path.Combine(_familyFolderFramingSteel, "W-Wide Flange.rfa"));
            FramingName.Add("Steel Tube", Path.Combine(_familyFolderFramingSteel, "HSS-Hollow Structural Section.rfa"));
            FramingName.Add("Steel Pipe", Path.Combine(_familyFolderFramingSteel, "HSS-Round Structural Tubing.rfa"));
            return FramingName;
        }
    }

    // Class for loading family type override options
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
