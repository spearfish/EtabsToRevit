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
        public void changeTypeColumn(Document doc, RevitObject revitObject, EtabObject etabObject, List<FamilySymbol> structuralColumnTypeList)
        {
            string _typeName = etabObject._SectionName;
            FamilySymbol symbol = structuralColumnTypeList.Find(x => x.Name == etabObject._SectionName);
            
            // Check to see if element type exist in the project
            if (symbol == null)
            {
                loadFamily(doc, etabObject);
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

        const string _familyFolder = @"X:\Revit\Revit Unifi Library\z-Autodesk OOTB Revit 2015 Family Library\Libraries\US Imperial\Structural Columns\Steel";
       // const string _familyFolder = Path.GetDirectoryName(typeof(FamilySymbol).Assembly.Location);
        static string _family_path = null;
        static string _familyfile_name;

        public void loadFamily(Document doc, EtabObject etabObject)
        {
            //using regular expression to search for the abbriviation of shape
            var typeNameRegex_Expression = @"^[A-Z]*\D";
            var typeNameRegex = Regex.Match(etabObject._SectionName, typeNameRegex_Expression);
            string returnValue;
            ColumnDictionary().TryGetValue(typeNameRegex.ToString(), out returnValue);
            // Retrive file name from dictionary using the appriviation
            _familyfile_name = returnValue;

            FamilySymbol familySymbol = null;
            string familypath = FilePath; // method file path

            using (Transaction tx = new Transaction(doc))
            {
                tx.Start("Load Family");
                Family family = null;
                string symbName = null;
                int counter = 0;
                if (doc.LoadFamily(familypath, new FamilyLoadingOverwriteOption(), out family))
                {
                    foreach (ElementId fsids in family.GetFamilySymbolIds())
                    {
                        ElementType elemtype = doc.GetElement(fsids) as ElementType;
                        FamilySymbol symb = elemtype as FamilySymbol;
                        //TaskDialog.Show("Symbol names", symb.Name);
                        if (counter == 0)
                        {
                            symbName = symb.Name;
                        }
                        counter++;
                    }
                }
                tx.RollBack();

                Transaction transNew = new Transaction(doc, "RealLoading");

                transNew.Start();

                if (doc.LoadFamilySymbol(familypath, symbName, new FamilyLoadingOverwriteOption(), out familySymbol))
                {
                    TaskDialog.Show("Status",
                      "We managed to load only one desired symbol!");
                }
                transNew.Commit();
            } 
        }

        private static Dictionary <string, string> ColumnDictionary()
        {
            Dictionary<String, String> ColumnName = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            ColumnName.Add("W", "W-Wide Flange-Column.rfa");
            ColumnName.Add("HSS", "HSS-Hollow Structural Section-Column.rfa");
            return ColumnName;
        }

        static string FilePath
        {
            get
            {
                if (null == _family_path)
                {
                    _family_path = Path.Combine(_familyFolder, _familyfile_name);
                    //_family_path = Path.Combine(_family_path);
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
