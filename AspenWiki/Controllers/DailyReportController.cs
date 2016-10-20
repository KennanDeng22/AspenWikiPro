using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AspenWiki.DAL;
using AspenWiki.Models;
using AspenWiki.DB;

namespace AspenWiki.Controllers
{
    public class DailyReportController : BaseController
    {
        DailyReportDAO dao = new DailyReportDAO();
        private GenericDAO<User> userDAO = new GenericDAO<User>();

        // GET: 
        public ActionResult Index(string date)
        {
            DateTime theDate;
            if(!DateTime.TryParse(date, out theDate))
            {
                theDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            }

            ViewBag.UserId = GetUserId();
            ViewBag.DateDisplay = theDate.ToLongDateString();
            ViewBag.Date = theDate.ToShortDateString();
            ViewBag.NextDate = theDate.AddDays(1).ToShortDateString();
            ViewBag.PreviousDate = theDate.AddDays(-1).ToShortDateString();

            var reports = dao.FindReports(null, theDate);//TODO
            ViewBag.ReportCount = reports.Count;
            //var reports = db.DailyReports.Where(t => t.ReportDate.Equals(theDate));

            //if (User.Identity.IsAuthenticated)
            //{
            //    string userId = GetUserId();
            //    string today = DateTime.Now.ToShortDateString();
            //    return RedirectToAction("Details", new { userId = userId, date = today });
            //}
            return View(reports.OrderBy(x=>x.User.FullName).ToList());
        }

        // GET: DailyReport/Details?userId=&date
        public ActionResult Details(string userId, string date)
        {
            if (userId == null || date==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DateTime theDate = DateTime.Parse(date);
            var reports = dao.FindReports(userId, theDate);
            DailyReport dailyReport = reports.FirstOrDefault();
            //DailyReport dailyReport = db.DailyReports.SingleOrDefault(t => t.UserId == userId && t.ReportDate.Equals(theDate));
            if (dailyReport == null)
            {
                dailyReport = new DailyReport { UserId = userId, ReportDate = theDate, ReportDetails = new List<ReportDetail>() };
                //db.DailyReports.Add(dailyReport);
            }
            return View(dailyReport);
        }

        // GET: DailyReport/Create/?userId=&date
        public ActionResult Create(string userId, string date)
        {
            DailyReport dailyReport = null;    
            
            if (userId == null || date == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DateTime theDate = DateTime.Parse(date);
            //dailyReport = db.DailyReports.SingleOrDefault(t => t.UserId == userId && t.ReportDate.Equals(theDate));
            var reports = dao.FindReports(userId, theDate);
            dailyReport = reports.FirstOrDefault();

            if (dailyReport == null)
            {
                dailyReport = new DailyReport { UserId = userId, ReportDate = theDate, ReportDetails = new List<ReportDetail>() };                
                //db.Entry(dailyReport).State = EntityState.Added;
                //db.DailyReports.Add(dailyReport);
                //db.SaveChanges();
                dao.Create(dailyReport);
            }

            return RedirectToAction("Edit", new { id = dailyReport.Id });
        }

        // GET: DailyReport/CreateAll/?userId=&date
        public ActionResult CreateAll(string userId, string date)
        {
            DateTime theDate = DateTime.Parse(date);
            var users = userDAO.ListAll();
            var reports = dao.FindReports(null, theDate);
            foreach (var user in users)
            {
                if (user.Location == "SH")
                {
                    if (!reports.Any(x => x.UserId == user.Id))
                    {
                        var dailyReport = new DailyReport { UserId = user.Id, ReportDate = theDate, ReportDetails = new List<ReportDetail>() };
                        dao.Save(dailyReport);
                    }
                }
            }

            return RedirectToAction("Index", new { date = date });
        }

        //Edit simple report without detail items
        // GET: DailyReport/Edit/?userId=&date
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyReport dailyReport = null;//dao.Read(id);            
            var workItem = dao.ReadWorkItem("None"); //db.WorkItems.Find("None");
            dailyReport = dao.Read(id);//db.DailyReports.Find(id);
            return View(dailyReport);
        }

        // POST: DailyReport/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DailyReport dailyReport)
        {
            //DailyReport dailyReport = db.DailyReports.Include(i => i.ReportDetails).Where(i => i.Id == id).Single();
            
            if (ModelState.IsValid)
            {                
                dao.Save(dailyReport);
                return RedirectToAction("Index", new { date = dailyReport.ReportDate.ToShortDateString() });
            }
            return View(dailyReport);
        }


        // GET: DailyReport/EditDetail/?userId=&date
        public ActionResult EditDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyReport dailyReport = null;//dao.Read(id);            
            var workItem = dao.ReadWorkItem("None"); //db.WorkItems.Find("None");
            if (id != null)
            {
                dailyReport = dao.Read(id);//db.DailyReports.Find(id);
            }

            int count = dailyReport.ReportDetails.Count();
            for (int i = count; i < 10; i++)
                dailyReport.ReportDetails.Add(new ReportDetail { WorkItemId = workItem.Id, WorkItem = workItem });

            ViewBag.WorkItems = dao.WorkItems.OrderBy(i=>i.OrderIdx).ToList();
            return View(dailyReport);
        }

        // POST: DailyReport/EditDetail/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDetail(int id, FormCollection formCollection, ReportDetail[] ReportDetails)
        {
            //DailyReport dailyReport = db.DailyReports.Include(i => i.ReportDetails).Where(i => i.Id == id).Single();
            DailyReport dailyReport = dao.Read(id);
            
            if (ModelState.IsValid)
            {                
                dailyReport.ReportDetails.Clear();
                for (var i = 0; i < ReportDetails.Length; i++)
                {
                    if (!ReportDetails[i].WorkItemId.Equals("None"))
                        dailyReport.ReportDetails.Add(ReportDetails[i]);
                }
                //db.Entry(dailyReport).State = EntityState.Modified;
                //db.SaveChanges();
                dao.Update(dailyReport);
                return RedirectToAction("Index", new { date = dailyReport.ReportDate.ToShortDateString() });
            }
            return View(dailyReport);
        }

        // GET: DailyReport/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyReport dailyReport = dao.Read(id);
            //if (dailyReport == null)
            //{
            //    return HttpNotFound();
            //}
            //db.DailyReports.Remove(dailyReport);
            //db.SaveChanges();
            dao.Delete(id);
            return RedirectToAction("Index", new { date = dailyReport.ReportDate.ToShortDateString() });
        }
        
    }
}
