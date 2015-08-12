using LoanManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace LoanManagementSystem
{
    public class MvcApplication : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            using (var db = new LoanDBContext())
            {
                Database.SetInitializer(new Business.Base.LoanDBInitializer());
                db.Database.Initialize(true);
            }

            GlobalFilters.Filters.Add(new LoanManagementSystem.App_Start.MenuLoaderActionFilter(), 0);
        }
    }
}