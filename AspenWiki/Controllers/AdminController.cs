using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AspenWiki.Models;
using AspenWiki.DAL;
using AspenWiki.DB;

namespace AspenWiki.Controllers
{
    public class AdminController : BaseController
    {
        private SysSettingDAO sysSettingDAO = new SysSettingDAO();
        // GET: Admin index page
        public ActionResult Index()
        {
            //var setting = db.SysSettings.Find(SysSettingConstants.DEFECT_LAST_IMPORT_TIME);
            var setting = sysSettingDAO.Read(SysSettingConstants.DEFECT_LAST_IMPORT_TIME);
            ViewBag.DefectLastImportTime = (setting == null) ? "No Import Yet." : setting.SettingValue;
            return View();
        }

        public ActionResult ImportDefects()
        {
            DataImporter importer = new DataImporter();
            importer.ImportDefect(Server.MapPath("~/App_Data/QueryResult.xls"));
            return RedirectToAction("Index");
        }
    }
}