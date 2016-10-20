using FlexCel.XlsAdapter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ExcelReportUpdate
{
    public class DefectImporter
    {
        private int GetLastRow(XlsFile xls)
        {
            int row = 0;
            while (true)
            {
                string val = Convert.ToString(xls.GetCellValue(++row, 1));
                if (string.IsNullOrEmpty(val))
                    return row;
            }
        }

        private int FindRow(XlsFile xls, string cq)
        {
            int row = 0;
            while (true)
            {
                string val = Convert.ToString(xls.GetCellValue(++row, 1));
                if (string.IsNullOrEmpty(val))
                    break;
                if (cq.Equals(val))
                {
                    return row;
                }
            }
            return -1;
        }

        Dictionary<string, string> GetFailTCDefects(XlsFile xls)
        {
            Dictionary<string, string> defects = new Dictionary<string, string>();
            xls.ActiveSheetByName = "FailTC";
            int lastRow = xls.GetRowCount(xls.ActiveSheet);
            for (var i = 1; i <= lastRow; i++)
            {
                string val = Convert.ToString(xls.GetCellValue(i, 1));
                if (!string.IsNullOrEmpty(val) && !defects.ContainsKey(val))
                {
                    defects.Add(val, "Yes");
                }
            }
            return defects;
        }

        public void UpdateFailTCDefects(XlsFile xls)
        {
            //XlsFile xls = new XlsFile(fileName);
            Dictionary<string, string> defects = GetFailTCDefects(xls);
            xls.ActiveSheetByName = "Defects";

            int row = 1;//skip header row
            while (true)
            {
                string val = Convert.ToString(xls.GetCellValue(++row, 1));
                if (string.IsNullOrEmpty(val))
                    break;
                if (defects.ContainsKey(val))
                {
                    //column R=18(from 1)
                    xls.SetCellValue(row, 18, "Yes");
                }
            }
            //SaveToFile(xls, fileName);
        }


        public void ImportDefect(string fileName, string targetFileName)
        {
            SheetData sheetData = ExcelUtil.ImportFromExcel(fileName);

            XlsFile xls = new XlsFile(targetFileName);
            xls.ActiveSheetByName = "Defects";

            int LAST_ROW = GetLastRow(xls);

            for (int i = 0; i < sheetData.RecordCount; i++)
            {
                var record = sheetData.Records[i];//sheetData.GetRecord(i);
                //id	State	Severity	Priority	Owner	Headline	Submit_Date	Last_Modified	Version_Number	Target_Rel	Area
                object id, State, Severity, Priority, Owner, Headline, Submit_Date, Last_Modified, Version_Number, Target_Rel, Area;
             
                id = record[0];
                if (id==null ||id.ToString()=="")
                    continue;

                int row = FindRow(xls, id.ToString());
                if (row == -1)
                {
                    row = LAST_ROW++;
                }

                for (int j = 0; j < record.Length; j++)
                {
                    xls.SetCellValue(row, j+1, record[j]);
                }
            }

            UpdateFailTCDefects(xls);

            SaveToFile(xls, targetFileName);
        }

        private void SaveToFile(XlsFile xls, string fileName)
        {
            string outputFile = fileName.Replace(".", "_new.");
            xls.Save(outputFile);
            File.Copy(outputFile, fileName, true);
            File.Delete(outputFile);
        }
    }
}