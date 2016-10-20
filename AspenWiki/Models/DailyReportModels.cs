using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/*
 * Daily report models
 */
namespace AspenWiki.Models
{
    public class DailyReport
    {
        [Key]// UserId+reportDate is unique
        public int Id { get; set; }
        [StringLength(20)]
        public string UserId { get; set; }
        public DateTime ReportDate { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }
        [StringLength(1000)]
        public string Comment { get; set; }

        public virtual ICollection<ReportDetail> ReportDetails { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }

    public class ReportDetail
    {
        public int Id { get; set; }
        public string WorkItemId { get; set; }//WorkItem
        [StringLength(30)]
        public string DefectId { get; set; }//Defect
        [StringLength(1000)]
        public string Description { get; set; }        
        public int WorkingHours { get; set; }
        [StringLength(30)]
        public string State { get; set; }
        [StringLength(1000)]
        public string Comment { get; set; }

        [ForeignKey("WorkItemId")]
        public virtual WorkItem WorkItem { get; set; }
    }

    public class WorkItem
    {
        [Key]
        [StringLength(20)]
        public string Id { get; set; }
        public int OrderIdx { get; set; }
        [StringLength(20)]
        public string WorkType { get; set; }//Task, Defect(DefectFix, DefectReview), Leave(Company Vacation, Sick Leave), Meeting, Other
        [StringLength(100)]
        public string Name { get; set; }
        public bool Closed { get; set; }
        //Below properties are for task 
        [StringLength(20)]
        public string Owner { get; set; }
        [StringLength(20)]
        public string Version { get; set; }
        [StringLength(20)]
        public string Area { get; set; }
        [StringLength(20)]
        public string SubArea { get; set; }
        [StringLength(200)]
        public string Requirement { get; set; }
        public float Progress { get; set; }//0~100
        [StringLength(20)]
        public string State { get; set; }//Create, Assign, Start, Pause, Implement, Test, Complete
        public int EstimatedEffort { get; set; }//Estimated effort in hours
        public int ActualEffort { get; set; }//Actual effort in hours

        public DateTime? PlanStartDate { get; set; }
        public DateTime? PlanEndDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }

    }
}