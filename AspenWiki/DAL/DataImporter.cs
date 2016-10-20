using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AspenWiki.Utils;
using AspenWiki.Models;
using AspenWiki.DB;

namespace AspenWiki.DAL
{
    public class DataImporter
    {
        
        public void ImportDefect(string fileName)
        {
            SysSettingDAO sysSettingDAO = new SysSettingDAO();
            DefectDAO defectDAO = new DefectDAO();

            DateTime? lastModifiedTime = null;
            SheetData sheetData = ExcelUtil.ImportFromExcel(fileName);
            for (int i = 0; i < sheetData.RecordCount; i++)
            {
                var record = sheetData.GetRecord(i);
                //id State Impact Owner Headline Submit_Date Last_Modified Version_Number Target_Rel	Area
                object id, State, Impact, Owner, Headline, Submit_Date, Last_Modified, Version_Number, Target_Rel, Area;
                if (!record.TryGetValue("id", out id))
                    continue;
                Defect defect = defectDAO.Read(id);
                bool isNew = (defect == null);
                if (defect == null)
                {
                    defect = new Defect() { Id = id.ToString() };
                    //db.Defects.Add(defect);
                }
                if (record.TryGetValue("State", out State))
                    defect.State = State.ToString();
                if (record.TryGetValue("Impact", out Impact))
                    defect.Impact = Impact.ToString();
                if (record.TryGetValue("Owner", out Owner))
                    defect.Owner = Owner.ToString();
                if (record.TryGetValue("Headline", out Headline))
                    defect.Headline = Headline.ToString();
                if (record.TryGetValue("Submit_Date", out Submit_Date))
                    defect.SubmitDate = DateTime.Parse(Submit_Date.ToString());
                if (record.TryGetValue("Last_Modified", out Last_Modified))
                    defect.LastModified = DateTime.Parse(Last_Modified.ToString());
                if (record.TryGetValue("Version_Number", out Version_Number))
                    defect.VersionNumber = Version_Number.ToString();
                if (record.TryGetValue("Target_Rel", out Target_Rel))
                    defect.TargetRel = Target_Rel.ToString();
                if (record.TryGetValue("Area", out Area))
                    defect.Area = Area.ToString();

                var lastModified = DateTime.Parse(Last_Modified.ToString());
                if (lastModifiedTime == null || (lastModifiedTime.Value.CompareTo(lastModified) < 0))
                    lastModifiedTime = lastModified;
                //defect.WorkingHours = 0;

                if (isNew)
                    defectDAO.Create(defect);
                else
                    defectDAO.Update(defect);
            }

            if (lastModifiedTime != null)
            {
                var setting = sysSettingDAO.Read(SysSettingConstants.DEFECT_LAST_IMPORT_TIME);
                if (setting == null)
                {
                    setting = new SysSetting();
                    setting.SettingName = SysSettingConstants.DEFECT_LAST_IMPORT_TIME;
                    setting.SettingValue = lastModifiedTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    sysSettingDAO.Create(setting);
                }
                else {
                    setting.SettingValue = lastModifiedTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    sysSettingDAO.Update(setting);
                }
                
            }
        }
    }
}