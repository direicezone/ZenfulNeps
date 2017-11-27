using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using ZenfulNeps.Models;

namespace ZenfulNeps.Common.Core
{
	public static class Common
	{
		public static List<CompanionPlant> GetData(bool clearCache)
		{
			var items = HttpRuntime.Cache["companion_plant"] as List<CompanionPlant>;
			if (items == null || clearCache)
			{
				items = new List<CompanionPlant>();
				var doc = new XmlDocument();
				var mypath = HttpContext.Current.Server.MapPath("../Data/CompanionPlants.xml");
				doc.Load(mypath);
				var root = doc.SelectSingleNode("CompanionPlants");
				var nodeList = root.SelectNodes("Plants");
				foreach (XmlNode element in nodeList)
				{
					var item = new CompanionPlant();
					item.PlantId = element.SelectSingleNode("PlantID").InnerText;
					item.Plant = element.SelectSingleNode("Plant").InnerText;
					item.Companions = new List<string>();
					item.Companions = element.SelectSingleNode("Companions").InnerText.Split(',').ToList();
					item.CompanionsFlat = element.SelectSingleNode("Companions").InnerText;
					item.Incompatibles = new List<string>();
					item.Incompatibles = element.SelectSingleNode("Incompatible").InnerText.Split(',').ToList();
					item.IncompatiblesFlat = element.SelectSingleNode("Incompatible").InnerText;
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
			return items.OrderBy(o => o.Plant).ToList();
		}
	}
}