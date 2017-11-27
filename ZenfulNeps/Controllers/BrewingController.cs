using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using ZenfulNeps.Models;

namespace ZenfulNeps.Controllers
{
    public class BrewingController : Controller
    {
        //
        // GET: /Brewing/

        public ActionResult Index()
        {
			var brewingModel = new Models.Brewing();
			brewingModel.Grains = GetGrains(true);
	        brewingModel.Colors = GetColors(true);
			return View("Brewing", brewingModel);
		}

		private List<Grain> GetGrains(bool clearCache)
		{
			var items = HttpRuntime.Cache["grains"] as List<Grain>;
			if (items == null)
			{
				items = new List<Grain>();
				var doc = new XmlDocument();
				var mypath = Server.MapPath("../Data/BrewingIngredients.xml");
				doc.Load(mypath);
				var root = doc.SelectSingleNode("Ingredients");
				var nodeList = root.SelectNodes("Ingredient");
				foreach (XmlNode element in nodeList)
				{
					var item = new Grain();
					item.Id = Convert.ToInt16(element.SelectSingleNode("ID").InnerText);
					item.Name = element.SelectSingleNode("Name").InnerText;
					item.Lovibond = Convert.ToDecimal(element.SelectSingleNode("Lovibond").InnerText);
					item.Ppg = Convert.ToDecimal(element.SelectSingleNode("PPG").InnerText);
					item.Mashable = element.SelectSingleNode("Mashable").InnerText == "1";
					item.Category = element.SelectSingleNode("Category").InnerText;
					items.Add(item);
				}
				HttpRuntime.Cache["grains"] = items;
			}
			return items.Where(w => w.Category == "Grain" || w.Category == "Adjunct").OrderBy(o => o.Name).ToList();
		}

		private List<Color> GetColors(bool clearCache)
		{
			var colors = HttpRuntime.Cache["colors"] as List<Color>;
			if (colors == null)
			{
				colors = new List<Color>();
				var doc = new XmlDocument();
				var mypath = Server.MapPath("../Data/Colors.xml");
				doc.Load(mypath);
				var root = doc.SelectSingleNode("COLORS");
				var nodeList = root.SelectNodes("COLOR");
				foreach (XmlNode element in nodeList)
				{
					var color = new Color();
					color.SRM = Convert.ToDecimal(element.SelectSingleNode("SRM").InnerText);
					color.RGB = element.SelectSingleNode("RGB").InnerText;
					colors.Add(color);
				}
				HttpRuntime.Cache["colors"] = colors;
			}
			return colors;
		}

		public JsonResult GetPPG(string id)
		{
			var x = 0;
			var grains = GetGrains(false);
			var ppg = grains.Where(s => s.Id == Convert.ToInt16(id)).Select(ss => ss.Ppg).FirstOrDefault();
			return Json(new { PPG = ppg }, JsonRequestBehavior.AllowGet);

		}
		
		public JsonResult GetLovi(string id)
		{
			var x = 0;
			var grains = GetGrains(false);
			var lovi = grains.Where(s => s.Id == Convert.ToInt16(id)).Select(ss => ss.Lovibond).FirstOrDefault();
			return Json(new { Lovi = lovi }, JsonRequestBehavior.AllowGet);

		}

		public JsonResult GetColor(string srm)
		{
			var x = 0;
			var colors = GetColors(false);
			var rgb = colors.Where(s => s.SRM == Convert.ToDecimal(srm)).Select(ss => ss.RGB).FirstOrDefault();
			if (rgb == null)
			{
				rgb = "255,255,255";
			}
			if (Convert.ToDecimal(srm) > 40)
			{
				rgb = "3,4,3";
			}
			return Json(new { RGB = rgb }, JsonRequestBehavior.AllowGet);

		}
		
		[HttpGet, OutputCache(Duration = 0)]
		public JsonResult GetGrainList()
		{
			var grains = GetGrains(false);
			return Json(new
			{
				GrainList = grains.Select(s => s.Name).ToArray()
			}, JsonRequestBehavior.AllowGet);
		}

    }
}
