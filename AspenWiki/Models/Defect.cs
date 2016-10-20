using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspenWiki.Models
{
    public class Defect
    {
        //id	State	Impact	login_name	Headline	Last_Modified	Target_Rel	Area
        [Key]
        public string Id { get; set; }
        public string State { get; set; }//Assigned Approved Implemented 

        public string Impact { get; set; }//1-Critical 2-Urgent 3-Important 4-Minor
        public string Priority { get; set; }
        public string Owner { get; set; }//Owner.login_name
        public string Headline { get; set; }
        
        //DateTime has the range: January 1, 1753, through December 31, 9999
        //DateTime2 has the range: 0001-01-01 through 9999-12-31
        //Entity Framework will add default date for the field of value {01/01/0001 00:00:00} and this is outside the range of SQL Date Generation. 
        //So to make it work, we need to ask EF not to generate a default date, we can make it nullable by doing the following in the Model of that class.
        public DateTime? SubmitDate { get; set; }
        public DateTime? LastModified { get; set; }
        public string VersionNumber { get; set; }
        public string TargetRel { get; set; }
        public string Area { get; set; }        

        //
        public int WorkingHours { get; set; }
        public string DevNote { get; set; }
    }
}