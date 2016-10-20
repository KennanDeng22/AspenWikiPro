using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspenWiki.Models
{
    public class User
    {
        [Key]
        [StringLength(20)]
        public string Id { get; set; }//corp name
        [StringLength(20)]
        public string Location { get; set; }// SH or NALA
        [StringLength(30)]
        public string CQLoginName { get; set; }
        [StringLength(30)]
        public string FullName { get; set; }
        [StringLength(50)]
        public string Email { get; set; }
        [StringLength(20)]
        public string SupervisorId { get; set; }
    }
}