using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AspenWiki.Models;
using AspenWiki.DB;
using Microsoft.AspNet.Identity;

namespace AspenWiki.Controllers
{
    public class UserController : BaseController
    {
        private GenericDAO<User> dao = new GenericDAO<User>(null, "Id", false);

        // GET: User
        public ActionResult Index()
        {
            return View(dao.ListAll().OrderBy(x=>x.FullName).ToList());
        }

        // GET: User/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = dao.Read(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            var user = new User();
            return View(user);
        }

        // POST: Category/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                dao.Create(user);
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: User/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = dao.Read(id);
            if (user == null)
            {
                user = new User();
                user.Id = id;
                dao.Create(user);
                //return HttpNotFound();
            }
            return View(user);
        }

        // POST: Build/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Location,CQLoginName,FullName,Email,SupervisorId")] User user)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(build).State = EntityState.Modified;
                //db.SaveChanges();
                dao.Update(user);
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: User/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = dao.Read(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Build/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            //Build build = db.Builds.Find(id);
            //db.Builds.Remove(build);
            //db.SaveChanges();
            dao.Delete(id);
            return RedirectToAction("Index");
        }

    }
}