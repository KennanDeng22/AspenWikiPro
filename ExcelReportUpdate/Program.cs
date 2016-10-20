using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelReportUpdate
{
    class Program
    {
        static void Main(string[] args)
        {
            string sourceFilePath = @"D:\TeamDocuments\FMB_Documents\QueryResult.xls";
            string filePath = @"D:\TeamDocuments\FMB_Documents\Defect.xlsx";
            DefectImporter importer = new DefectImporter();
            //importer.UpdateFailTCDefects(filePath);
            importer.ImportDefect(sourceFilePath, filePath);
        }
    }
}
