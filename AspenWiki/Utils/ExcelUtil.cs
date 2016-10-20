using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlexCel.Core;
using FlexCel.XlsAdapter;
using System.Data.OleDb;
using System.Data;
using System.Collections;

namespace AspenWiki.Utils
{
    class ExcelUtil
    {
        public static SheetData ImportFromExcel(string fileName)
        {
            //string strCon = "Provider = Microsoft.Jet.OLEDB.4.0;  Data Source = " + fileName + ";Extended Properties=Excel 8.0";
            //OleDbConnection strConn = new OleDbConnection(strCon); 
            //string strSql = "select * from [sheet1$]"; strConn.Open();
            //OleDbDataAdapter strCom = new OleDbDataAdapter(strSql, strConn);
            //var ds = new DataSet();
            //strCom.Fill(ds, "[sheet1$]"); strConn.Close();
            //dataGridView1.DataMember = "[sheet1$]"; dataGridView1.DataSource = ds;
            //return ds;

            XlsFile xls = new XlsFile(fileName);
            return GetSheetData(xls, 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xls"></param>
        /// <param name="sheet">Start from 1</param>
        /// <returns></returns>
        public static SheetData GetSheetData(XlsFile xls, int sheet)
        {
            xls.ActiveSheet = sheet;
            SheetData sheetData = new SheetData();
            int colCount = xls.GetColCount(sheet);
            int rowCount = xls.GetRowCount(sheet);
            int row = 1;
            for (int col = 1; col <= colCount; col++)
            {
                string val = Convert.ToString(xls.GetCellValue(row, col));
                if (string.IsNullOrEmpty(val))
                    break;
                sheetData.Columns.Add(val.Trim());                
            }
            colCount = sheetData.Columns.Count();

            row++;
            while (row<=rowCount)
            {                
                object[] recordRow = new object[colCount];
                for (int col = 1; col <= colCount; col++)
                {
                    recordRow[col - 1] = xls.GetCellValue(row, col);
                }
                sheetData.Records.Add(recordRow);
                row++;
            }

            return sheetData;
        }


        private void setExcelColumnValues(FlexCel.XlsAdapter.XlsFile xls, int rowStart, int column, String[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
                xls.SetCellValue(rowStart + i, column, arr[i]);
        }

        //private static bool IsColumnExportable(DataGridViewColumn col)
        //{
        //    return col.Visible;//&& (col is DataGridViewTextBoxColumn)
        //}

        public static string ValueToString(object val)
        {
            if (val is Boolean)
            {
                var newVal = Convert.ToBoolean(val);
                return newVal == true ? "是" : "否";
            }else if(val is DateTime)
            {
                var newVal = Convert.ToDateTime(val);
                return newVal.ToString("yyyy-MM-dd");
            }
            return val.ToString();
        }

        private static DateTime DEFAULT_DATE = new DateTime(1, 1, 1);
        public static DateTime ParseDate(object val)
        {
            string strVal = Convert.ToString(val);
            if (string.IsNullOrEmpty(strVal))
                return DEFAULT_DATE;
            string[] parts = null;
            if (strVal.IndexOf('.') > 0)
            {
                parts = strVal.Split(new char[]{'.'});
            }
            else if (strVal.IndexOf('-') > 0)
            {
                parts = strVal.Split(new char[] { '-' });
            }

            int day = 1;
            if (parts != null && parts.Length >= 2)
            {
                int year = Convert.ToInt32(parts[0]);
                int month = Convert.ToInt32(parts[1]);
                if(parts.Length>=3)
                    day = Convert.ToInt32(parts[2]);
                if (year >= 1900 && month >= 1 && month <= 12 && day >= 1 && day <= 31)
                    return new DateTime(year, month, day);
                else
                    throw new Exception(val + " is invalid.");
            }
            return DEFAULT_DATE;
        }

        public static bool ParseBoolean(object val)
        {
            string strVal = Convert.ToString(val);
            if (string.IsNullOrEmpty(strVal))
                return false;
            return (strVal.Equals("1") || strVal.Equals("是") || strVal.Equals("Yes",StringComparison.InvariantCultureIgnoreCase));
        }

    }


    class SheetData
    {
        public IList<string> Columns { get { return _columns; } }
        public IList<object[]> Records { get { return _records; } }
        private IList<string> _columns = new List<string>();
        private IList<object[]> _records = new List<object[]>();

        public int ColumnCount { get { return _columns.Count(); } }
        public int RecordCount { get { return _records.Count(); } }

        /// <summary>
        /// Get a record as dictionary
        /// </summary>
        /// <param name="row">Index of row. 0 based</param>
        /// <returns></returns>
        public IDictionary<string, object> GetRecord(int row)
        {
            IDictionary<string, object> rowDictionary = new Dictionary<string, object>();
            var recordRow = _records[row];
            int columnCount = ColumnCount;
            for (int col = 0; col < columnCount; col++)
            {
                rowDictionary.Add(_columns[col], recordRow[col]);
            }
            return rowDictionary;
        }
    }
}
