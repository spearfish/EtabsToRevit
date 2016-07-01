using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
/* To work eith EPPlus library */
using OfficeOpenXml;
using System.Diagnostics;

namespace DSB.RevitTools.EtabsToRevit
{
    class EtabsModel
    {
        public void getExcel(string path)
        {          
            List<EtabObject> EtabsPoints = new List<EtabObject>();
            try
            {
                var package = new ExcelPackage(new FileInfo(path));
                ExcelWorksheet workSheet = package.Workbook.Worksheets[1];

                var start = workSheet.Dimension.Start;
                var end = workSheet.Dimension.End;

                for (int row = 2; row <= end.Row; row++)
                {
                    EtabObject Etabs_Object = new EtabObject();
                    for (int col = start.Column; col <= end.Column; col++)
                    {
                        object cellValue = workSheet.Cells[row, col].Text;                        
                    }
                }

                // Close the Excel file. 
                package.Stream.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Cannot open " + path + " for reading. Exception raised - " + ex.Message);
            }
        }

        public void CaseSwitch(int Column, object cellValue, EtabObject Etabs_Object)
        {
            switch(Column)
            {
                case 3:
                    Etabs_Object.Set_Start_X(Convert.ToDouble(cellValue));
                    break;
            }
        }
    }
}
