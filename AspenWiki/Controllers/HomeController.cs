using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AspenWiki.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            DateTime theDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            ViewBag.UserId = GetUserId();
            ViewBag.Date = theDate.ToShortDateString();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}