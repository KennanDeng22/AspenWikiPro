using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspenWiki.Models
{
    public class Build
    {
        [Key]
        public int Id { get; set; }
        [StringLength(10)]
        public string Version { get; set; }
        [StringLength(30)]
        public string Name { get; set; }
        [StringLength(10)]
        public string PlatinumBuild { get; set; }
        [StringLength(100)]
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime? CreateTime { get; set; }        
    }
}
