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
    public class DefectController : BaseController
    {
        private DefectDAO defectDAO = new DefectDAO();        

        // GET: Defect
        public ActionResult Index()
        {
            ViewBag.Title = "Defects";
            //return View(db.Defects.ToList());
            return View(defectDAO.ListAll());
        }

        // GET: Defect
        public ActionResult Mine()
        {
            ViewBag.Title = "My Defects";
            //var user = db.Users.Find(GetUserId());
            var user = GetUser();
            var loginName = user.CQLoginName;
            //var defects = db.Defects.Where(t => t.Owner.Equals(loginName));
            var defects = defectDAO.FindBy("Owner", loginName);
            return View("Index", defects.ToList());
        }

        // GET: Defect/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Defect defect = defectDAO.Read(id);//db.Defects.Find(id);
            if (defect == null)
            {
                return HttpNotFound();
            }
            return View(defect);
        }


        // GET: Defect/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Defect defect = db.Defects.Find(id);
            Defect defect = defectDAO.Read(id);
            if (defect == null)
            {
                return HttpNotFound();
            }
            return View(defect);
        }

        // POST: Defect/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,State,WorkingHours,DevNote")] Defect defect)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(defect).State = EntityState.Modified;
                //db.SaveChanges();
                defectDAO.Update(defect, new string[] { "DevNote", "WorkingHours" });
                return RedirectToAction("Index");
            }
            return View(defect);
        }

        // GET: Defect/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Defect defect = db.Defects.Find(id);
            //if (defect == null)
            //{
            //    return HttpNotFound();
            //}
            //db.Defects.Remove(defect);
            //db.SaveChanges();
            defectDAO.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
