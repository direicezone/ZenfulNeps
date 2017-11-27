using System.Web.Optimization;

namespace ZenfulNeps.App_Start
{
	public static class BundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/Mobilejs").Include(
				"~/Scripts/jquery.mobile-1.*",
				"~/Scripts/jquery-1.*"));

			bundles.Add(new StyleBundle("~/Content/Mobilecss").Include(
				"~/Content/jquery.mobile.structure-1.4.5.min.css",
				"~/Content/jquery.mobile-1.4.5.css"));
		}
	}
}