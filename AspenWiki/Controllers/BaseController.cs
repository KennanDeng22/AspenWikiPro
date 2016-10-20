using AspenWiki.DAL;
using AspenWiki.DB;
using AspenWiki.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AspenWiki.Controllers
{
    public class BaseController : Controller
    {
        //protected AspenWikiDbContext db = new AspenWikiDbContext();
        //protected AspenWiki.DB.DbContext dbContext = new AspenWiki.DB.DbContext();

        protected GenericDAO<User> userDAO = new GenericDAO<User>();

        public string GetUserId()
        {
            string username = User.Identity.Name;
            if (username.IndexOf('\\') > 0)
            {
                username = username.Substring(username.IndexOf('\\') + 1);
            }
            return username;
        }

        public User GetUser()
        {
            return userDAO.Read(GetUserId());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}