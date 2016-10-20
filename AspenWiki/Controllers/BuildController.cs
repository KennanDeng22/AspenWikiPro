using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AspenWiki.Models;
using AspenWiki.DB;

namespace AspenWiki.Controllers
{
    public class BuildController : BaseController
    {
        private GenericDAO<Build> buildDAO = new GenericDAO<Build>();

        // GET: Build
        public ActionResult Index()
        {
            return View(buildDAO.ListAll().OrderByDescending(item=>item.CreateTime).ToList());
        }

        // GET: Build/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Build build = buildDAO.Read(id);
            if (build == null)
            {
                return HttpNotFound();
            }
            return View(build);
        }

        // GET: Build/Create
        public ActionResult Create()
        {
            var build = new Build{ Version = "V8.8", Name = "Bxx", PlatinumBuild = "9.0.0.xx", CreateTime = DateTime.Now };
            return View(build);
        }

        // POST: Category/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Version,Name,PlatinumBuild,Description,CreateTime")] Build build)
        {
            if (ModelState.IsValid)
            {
                buildDAO.Create(build);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(build);
        }

        // GET: Build/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Build build = buildDAO.Read(id);
            if (build == null)
            {
                return HttpNotFound();
            }
            return View(build);
        }

        // POST: Build/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Version,Name,PlatinumBuild,Description,CreateTime")] Build build)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(build).State = EntityState.Modified;
                //db.SaveChanges();
                buildDAO.Update(build);
                return RedirectToAction("Index");
            }
            return View(build);
        }

        // GET: Build/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Build build = buildDAO.Read(id);
            if (build == null)
            {
                return HttpNotFound();
            }
            return View(build);
        }

        // POST: Build/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Build build = db.Builds.Find(id);
            //db.Builds.Remove(build);
            //db.SaveChanges();
            buildDAO.Delete(id);
            return RedirectToAction("Index");
        }

    }
}