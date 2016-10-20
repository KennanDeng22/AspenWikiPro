using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspenWiki.Models
{
    public class SysSetting
    {
        [Key]
        [StringLength(100)]
        public string SettingName { get; set; }
        [StringLength(512)]
        public string SettingValue { get; set; }
    }

    public class SysSettingConstants
    {
        public static string DEFECT_LAST_IMPORT_TIME = "Defect.LastImportTime";
    }
}