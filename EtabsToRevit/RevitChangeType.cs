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

        // const string _familyFolder = Path.GetDirectoryName(typeof(FamilySymbol).Assembly.Location);
        const string _familyFolderColumnSteel = @"X:\Revit\Revit Unifi Library\z-Autodesk OOTB Revit 2015 Family Library\Libraries\US Imperial\Structural Columns\Steel";
        const string _familyFolderFramingSteel = @"";
        const string _family_folder = null;
        static string _family_path = null;
        static string _familyfile_name;

        public FamilySymbol loadFamily(Document doc, EtabObject etabObject)
        {
            //using regular expression to search for the abbriviation of shape
            var typeNameRegex_Expression = @"^[A-Z]*\D";
            var typeNameRegex = Regex.Match(etabObject._SectionName, typeNameRegex_Expression);
            string returnFamilyFileName;
            string returnFamilyFolder;
            // test to see if Etab object is a column 
            if(etabObject._LabelNumber.Contains("C"))
            {
                ColumnDictionary().TryGetValue(typeNameRegex.ToString(), out returnFamilyFileName, out returnFamilyFolder);
                // Retrive file name from dictionary using the appriviation
                _familyfile_name = returnFamilyFileName;
                _family_folder = returnFamilyFolder;
            }
            // test to see if Etab object is a beam
            if(etabObject._LabelNumber.Contains("B"))
            {
                FramingDictionary().TryGetValue(typeNameRegex.ToString(), out returnFamilyFileName, out returnFamilyFolder);
                // Retrive file name from dictionary using the appriviation
                _familyfile_name = returnFamilyFileName;
                _family_folder = returnFamilyFolder;
            }

            FamilySymbol familySymbol = null; // Empty Family Symbol which will return the loaded family
            string familypath = FilePath; // method file path

            // Transaction to load a family and list all its types 
            using (Transaction tx = new Transaction(doc))
            {
                tx.Start("Load Family");
                Family family = null;
                string symbName = null;
                if (doc.LoadFamily(familypath, new FamilyLoadingOverwriteOption(), out family))
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
                   doc.LoadFamilySymbol(familypath, symbName, out familySymbol); // load only the family type 
                }
                catch{}
                transNew.Commit();
            }
            return familySymbol;
        }

        // Dictionary to reference Etab section type and Revit family file. 
        private static Dictionary <string, string, string> ColumnDictionary()
        {
            Dictionary<String, String, String> ColumnName = new Dictionary<string, string, string>(StringComparer.OrdinalIgnoreCase);
            ColumnName.Add("W", "W-Wide Flange-Column.rfa", _familyFolderColumnSteel);
            ColumnName.Add("HSS", "HSS-Hollow Structural Section-Column.rfa", _familyFolderColumnSteel);
            return ColumnName;
        }
        private static Dictionary <string, string, string> FramingDictionary()
        {
            Dictionary<String, String, String> FramingName = new Dictionary<string, string, string>(StringComparer.OrdinalIgnoreCase);
            FramingName.Add("W", "W-Wide Flange.rfa", _familyFolderFramingSteel);
        }

        // Combine file path and file name. 
        static string FilePath
        {
            get
            {
                if (null == _family_path)
                {
                    _family_path = Path.Combine(_family_folder, _familyfile_name);
                }
                return _family_path;
            }
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
