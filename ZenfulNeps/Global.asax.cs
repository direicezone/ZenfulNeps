using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ZenfulNeps.App_Start;

namespace ZenfulNeps
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("DefaultOld", "default.aspx", new { controller = "ZenfulNeps", action = "Index" });
            routes.MapRoute("BodyFatCalc", "bodyfatcalc.aspx", new { controller = "ZenfulNeps", action = "BodyFatCalculator" });
            routes.MapRoute("CompanionPlants", "companionplants/default.aspx", new { controller = "CompanionPlants", action = "Index" });
            //routes.MapRoute("CompanionPlants2", "companionplants/", new { controller = "CompanionPlants", action = "Index" });
            routes.MapRoute(
				"Default", // Route name
				"{controller}/{action}/{id}", // URL with parameters
				new { controller = "ZenfulNeps", action = "Index", id = UrlParameter.Optional } // Parameter defaults
			);
        }

        protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			RegisterGlobalFilters(GlobalFilters.Filters);
			RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
		}
	}
}