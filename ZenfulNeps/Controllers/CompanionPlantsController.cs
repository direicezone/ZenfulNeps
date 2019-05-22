using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using ZenfulNeps.Models;

namespace ZenfulNeps.Controllers
{
    public class CompanionPlantsController : Controller
    {

        public ActionResult Index()
        {
            ViewBag.LastUpdate = "Last Update 2/9/2019: Added Swiss Chard, Chicory, and Kale";
            return View("CompanionPlants", GetData(false));
        }

		public ActionResult Details(string id)
		{
			var details = new ZenfulNeps.Models.PlantDetails();
			details.Plants = GetData(false);
			details.Plant = details.Plants.FirstOrDefault(i => i.PlantId == id);
			var plantIndex = details.Plants.FindIndex(f => f.PlantId == id);
			var prevIndex = plantIndex - 1;
			var nextIndex = plantIndex + 1;
			if (prevIndex >= 0)
			{
				details.Plant.PrevPlant = details.Plants[prevIndex].Plant;
				details.Plant.PrevPlantId = details.Plants[prevIndex].PlantId;
			}
			if (nextIndex < details.Plants.Count)
			{
				details.Plant.NextPlant = details.Plants[nextIndex].Plant;
				details.Plant.NextPlantId = details.Plants[nextIndex].PlantId;
			}

			return View("CompanionPlantDetail", details);

		}

		public ActionResult Search(string searchValue, string pestValue, string button)
		{
			//Test Comment
			if (searchValue == null && button == null && Session["SearchValue"] == null)
			{
				return View("CompanionPlants", GetData(false));
			}
			var companionPlants = GetData(false);

			if ((Session["SearchValue"] != null && button==null) || (!string.IsNullOrEmpty(searchValue)&& button == "Search"))
			{
				if (button == null && Session["SearchValue"] != null)
				{
					searchValue = Session["SearchValue"].ToString();
				}
				ViewData["searchValue"] = searchValue;
				companionPlants =
					companionPlants
						.Where(
							i =>
								i.Plant.ToLower().Contains(searchValue.ToLower().Trim()) ||
								i.Companions.Any(c => c.ToLower().Contains(searchValue.ToLower().Trim())))
						.ToList();
				Session["SearchValue"] = searchValue;
			}

			if ((Session["pestValue"] != null && button == null) || (!string.IsNullOrEmpty(pestValue) && button == "Search"))
			{
				ViewData["pestValue"] = pestValue;
				companionPlants = companionPlants.Where(i => i.Benefits.ToLower().Contains(pestValue.ToLower().Trim())).ToList();
				Session["pestValue"] = pestValue;
			}

			if (button == "Clear")
			{
				ViewData["searchValue"] = string.Empty;
				Session["SearchValue"] = null;
				ViewData["pestValue"] = string.Empty;
				Session["pestValue"] = null;
				companionPlants = GetData(true);
			}

			return View("CompanionPlants", companionPlants);

		}



	    protected List<CompanionPlant> GetData(bool clearCache)
	    {
		    var items = HttpRuntime.Cache["companion_plant"] as List<CompanionPlant>;
			if (items == null || clearCache)
			{
				items = new List<CompanionPlant>();
				var doc = new XmlDocument();
				var mypath = Server.MapPath("../Data/CompanionPlants.xml");
				doc.Load(mypath);
                // string json = JsonConvert.SerializeXmlNode(doc);
                // var myList = JsonConvert.DeserializeObject<CompanionPlant>(json);
                var root = doc.SelectSingleNode("CompanionPlants");
				var nodeList = root.SelectNodes("Plants");
				foreach (XmlNode element in nodeList)
				{
					var item = new CompanionPlant();
					item.PlantId = element.SelectSingleNode("PlantID").InnerText;
					item.Plant = element.SelectSingleNode("Plant").InnerText;
					item.Companions = new List<string>();
					item.Companions = element.SelectSingleNode("Companions").InnerText.Split(',').ToList();
					item.Incompatibles = new List<string>();
					item.Incompatibles = element.SelectSingleNode("Incompatible").InnerText.Split(',').ToList();
					item.Benefits = string.Empty;
					if (element.SelectSingleNode("Benefits") != null)
					{
						item.Benefits = element.SelectSingleNode("Benefits").InnerText;
					}
					item.Type = element.SelectSingleNode("Type").InnerText;
					item.PlantPicture = element.SelectSingleNode("PlantPic").InnerText;
					if (element.SelectSingleNode("Stars") != null)
					{
						item.Rating = element.SelectSingleNode("Stars").InnerText;
					}
					item.ScientificName = element.SelectSingleNode("ScientificName").InnerText;
					items.Add(item);
				}
				HttpRuntime.Cache["companion_plant"] = items;
			}
            Session["PlantCount"] = items.Count;

            return items.OrderBy(o => o.Plant).ToList();
	    }

		[HttpGet, OutputCache(Duration = 0)]
		public JsonResult GetPlantList()
		{
			var plants = (List<CompanionPlant>)HttpRuntime.Cache["companion_plant"];
			var plantList = plants.Select(s => s.Plant.Trim()).ToList();
			var companionList = plants.Select(s => s.Companions.ToArray());
			foreach (var result in companionList)
			{
				plantList.AddRange(result);
			}
			var incompatibleList = plants.Select(s => s.Incompatibles.ToArray());
			foreach (var result in incompatibleList)
			{
				plantList.AddRange(result);
			}
			var cleanList = plantList.Where(w => w != string.Empty).Select(s => s.Trim()).Distinct().ToList();
			return Json(new
			{
				PlantList = cleanList.ToArray()
			}, JsonRequestBehavior.AllowGet);
		}

    }
}
