using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AspenWiki.DAL;

namespace AspenWiki
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            //DefectDbContext db = new DefectDbContext();
            //DataImporter importer = new DataImporter();
            //importer.ImportDefect(db, @"D:\projects\AspenWiki\AspenWiki\App_Data\QueryResult.xls");
        }
    }
}
