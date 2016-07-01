#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
#endregion

namespace DSB.RevitTools.EtabsToRevit
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            EtabsModel EtabModel = new EtabsModel();
            string path = @"F:\00 - ETABS\test\DB-TEST.xlsx";
            List<EtabObject> EtabsList = EtabModel.Get_ExcelFileToList(path);

            // Etab Column List Sorted using the X value. 
            List<EtabObject> EtabsColumnList = EtabModel.Get_EtabsColumnList(EtabsList);
            List<EtabObject> Sorted_EtabsColumnList = EtabsColumnList.OrderBy(o => o.Get_Start_X()).ToList();

            //List<EtabObject> Sorted_EtabsList = EtabsList.OrderBy(o => o.Get_Start_X()).ToList();
            RevitModelElements revitModelElements = new RevitModelElements();
            revitModelElements.GetBeamAndColumnSymbols(doc);
            return Result.Succeeded;
        }
    }
}
