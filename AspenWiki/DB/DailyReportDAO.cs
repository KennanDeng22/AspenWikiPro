using AspenWiki.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AspenWiki.DB
{
    public class DailyReportDAO:GenericDAO<DailyReport>
    {
        private GenericDAO<User> userDAO = new GenericDAO<User>();
        private ReportDetailDAO detailDAO = new ReportDetailDAO();
        private GenericDAO<WorkItem> workItemDAO = new GenericDAO<WorkItem>();

        public DailyReport Read(object id)
        {
            DailyReport report = base.Read(id);
            if (report != null)
            {
                //TODO load cascade objects
                this.LoadDetails(report);
            }
            return report;
        }

        public bool Delete(object id)
        {
            string sql = null;
            sql = string.Format("delete from ReportDetails where DailyReport_Id={0}", id);
            db.ExecuteNonQuery(sql, CommandType.Text);
            sql = string.Format("delete from DailyReports where id={0}", id);
            return db.ExecuteNonQuery(sql, CommandType.Text) > 0;
        }

        public bool Create(DailyReport model)
        {
            return Save(model);
        }

        public bool Update(DailyReport model)
        {
            return Save(model);
        }

        public bool Save(DailyReport model)
        {
            bool ret = false;

            var reports = FindReports(model.UserId, model.ReportDate);
            var dailyReport = reports.FirstOrDefault();
            if (dailyReport == null)
            {
                dailyReport = model;
            }
            else
            {
                dailyReport.Description = model.Description;
                dailyReport.Comment = model.Comment;
                dailyReport.ReportDetails = model.ReportDetails;
            }

            if (dailyReport.Id == 0)
                ret = base.Create(dailyReport);
            else
                ret = base.Update(dailyReport);
            if (dailyReport.ReportDetails != null && dailyReport.ReportDetails.Count > 0)
            {
                dailyReport.ReportDetails.ToList().ForEach(detail =>
                {
                    if (detail.Id == 0)
                        detailDAO.Create(detail, dailyReport.Id);
                    else
                        detailDAO.Update(detail);
                });
            }
            return ret;
        }

        public IList<DailyReport> FindReports(string userId, DateTime? reportDate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = "";
            if (userId != null)
            {
                sql += "UserId=@UserId";
                parameters.Add(new SqlParameter("@UserId", userId));
            }
            if (reportDate != null)
            {
                if (sql.Length > 0)
                    sql += " and ";
                sql += "ReportDate=@ReportDate";
                parameters.Add(new SqlParameter("@ReportDate", reportDate.Value));
            }
            //sql = "select * from DailyReports where " + sql;
            IList<DailyReport> reports = base.Find(sql, null, parameters.ToArray());
            reports.ToList().ForEach(item => { this.LoadDetails(item); });
            return reports;
        }

        private void LoadDetails(DailyReport report)
        {
            report.User = userDAO.Read(report.UserId);
            report.ReportDetails = detailDAO.FindBy("DailyReport_Id", report.Id);
            report.ReportDetails.ToList().ForEach(detail => {
                detail.WorkItem = workItemDAO.Read(detail.WorkItemId);
            });
        }

        public WorkItem ReadWorkItem(string id)
        {
            return WorkItems.FirstOrDefault((a) => a.Id.Equals(id));
            //return workItemDAO.Read(id);
        }

        private IList<WorkItem> _workItems; 
        public IList<WorkItem> WorkItems { get {
            if (_workItems == null)
                _workItems = workItemDAO.ListAll();
            return _workItems;
        } }
    }

    public class ReportDetailDAO : GenericDAO<ReportDetail>
    {
        public bool Create(ReportDetail model, int reportId)
        {
            string sql = "insert ReportDetails(WorkItemId,DefectId,Description,WorkingHours,State,Comment,DailyReport_Id) values(@WorkItemId,@DefectId,@Description,@WorkingHours,@State,@Comment,@DailyReport_Id)";
            SqlParameter[] paramters = new SqlParameter[]{
                new SqlParameter("@WorkItemId",model.WorkItemId),
                new SqlParameter("@DefectId",model.DefectId==null?DBNull.Value:(object)model.DefectId),
                new SqlParameter("@Description",model.Description==null?DBNull.Value:(object)model.Description),
                new SqlParameter("@WorkingHours",model.WorkingHours),
                new SqlParameter("@State",model.State==null?DBNull.Value:(object)model.State),
                new SqlParameter("@Comment",model.Comment==null?DBNull.Value:(object)model.Comment),
                new SqlParameter("@DailyReport_Id", reportId)
            };

            var ret = db.ExecuteNonQuery(sql, CommandType.Text, paramters) > 0;
            if (ret)
            {
                sql = string.Format("SELECT IDENT_CURRENT('{0}')", TableName);
                int id = int.Parse(db.ExecuteScalar(sql).ToString());
                model.Id = id;
            }
            return ret;
        }

        public bool Update(ReportDetail model)
        {
            string sql = "update ReportDetails set WorkItemId=@WorkItemId,DefectId=@DefectId,Description=@Description,WorkingHours=@WorkingHours,State=@State,Comment=@Comment where Id=@Id";
            SqlParameter[] paramters = new SqlParameter[]{
                new SqlParameter("@WorkItemId",model.WorkItemId),
                new SqlParameter("@DefectId",model.DefectId==null?DBNull.Value:(object)model.DefectId),
                new SqlParameter("@Description",model.Description==null?DBNull.Value:(object)model.Description),
                new SqlParameter("@WorkingHours",model.WorkingHours),
                new SqlParameter("@State",model.State==null?DBNull.Value:(object)model.State),
                new SqlParameter("@Comment",model.Comment==null?DBNull.Value:(object)model.Comment),
                new SqlParameter("@Id",model.Id)
            };
            return db.ExecuteNonQuery(sql, CommandType.Text, paramters) > 0;
        }
    }
}