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
        public List<EtabObject> Get_ExcelFileToList(string path)
        {          
            List<EtabObject> EtabsList = new List<EtabObject>();
            try
            {
                // Gets Excel file using a file path 
                var package = new ExcelPackage(new FileInfo(path));
                ExcelWorksheet workSheet = package.Workbook.Worksheets[1];

                var start = workSheet.Dimension.Start;
                var end = workSheet.Dimension.End;

                /* Gets cell values from a row moving left to each column 
                 * then moving to the next row. */
                for (int row = 2; row <= end.Row; row++)
                {
                    EtabObject Etabs_Object = new EtabObject();
                    for (int col = start.Column; col <= end.Column; col++)
                    {
                        object cellValue = workSheet.Cells[row, col].Text;
                        CaseSwitch(col, cellValue, Etabs_Object);
                    }
                    EtabsList.Add(Etabs_Object);
                }

                // Close the Excel file. 
                package.Stream.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Cannot open " + path + " for reading. Exception raised - " + ex.Message);
            }

            return EtabsList;
        }

        /* adds the Excel column to match the Etabs Object.  
         * Sets the feilds into a object that holds all the information from the excel row. */
        public void CaseSwitch(int Column, object cellValue, EtabObject Etabs_Object)
        {
            try
            {
                switch (Column)
                {
                    case 1:
                        Etabs_Object._SectionName = Convert.ToString(cellValue);
                        break;
                    case 2:
                        Etabs_Object._LabelNumber = Convert.ToString(cellValue);
                        break;
                    case 3:
                        Etabs_Object.Set_Start_X(Convert.ToDouble(cellValue));
                        break;
                    case 4:
                        Etabs_Object.Set_Start_Y(Convert.ToDouble(cellValue));
                        break;
                    case 5:
                        Etabs_Object.Set_Start_Z( Convert.ToDouble(cellValue));
                        break;
                    case 6:
                        Etabs_Object._End_X = Convert.ToDouble(cellValue);
                        break;
                    case 7:
                        Etabs_Object._End_Y = Convert.ToDouble(cellValue);
                        break;
                    case 8:
                        Etabs_Object._End_Z = Convert.ToDouble(cellValue);
                        break;
                    case 9:
                        Etabs_Object._UniqueID = Convert.ToInt32(cellValue);
                        break;
                }
            }

            catch { }
        }
            
        // Check if Etabs Label Number contains a "C" to add to a column list.
        public List<EtabObject> Get_EtabsColumnList(List<EtabObject> EtabsList)
        {
            List<EtabObject> EtabsColumnList = new List<EtabObject>();
            foreach (EtabObject EtabsObj in EtabsList)
            {
                if (EtabsObj._LabelNumber.Contains("C"))
                {
                    EtabsColumnList.Add(EtabsObj);
                }
            }
            return EtabsColumnList;
        }
        // Check if Etabs Label Number contains a "B" to add to beam list. 
        public List<EtabObject> Get_EtabsBeamList(List<EtabObject> EtabsList)
        {
            List<EtabObject> EtabsBeamList = new List<EtabObject>();
            foreach (EtabObject EtabsObj in EtabsList)
            {
                if (EtabsObj._LabelNumber.Contains("B"))
                {
                    EtabsBeamList.Add(EtabsObj);
                }
            }
            return EtabsBeamList;
        }
    }
}
