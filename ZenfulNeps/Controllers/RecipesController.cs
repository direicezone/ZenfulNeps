using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using ZenfulNeps.Models;

namespace ZenfulNeps.Controllers
{
	public class RecipesController : Controller
	{

		public ActionResult Index()
		{
			var recipes = GetRecipes(true);
			return View("Recipes", recipes);
		}

		public ActionResult Details(string id)
		{
			var recipe = GetRecipes(false).Where(w => w.Id == id).FirstOrDefault();
			return View("RecipeDetails", recipe);
		}

		private List<Recipe> GetRecipes(bool clearCache)
		{
			var items = HttpRuntime.Cache["recipes"] as List<Recipe>;
			if (items == null || clearCache)
			{
				items = new List<Recipe>();
				var doc = new XmlDocument();
				var mypath = Server.MapPath("~/Data/Recipes.xml");
				doc.Load(mypath);
				var root = doc.SelectSingleNode("Recipes");
				var nodeList = root.SelectNodes("Recipe");
				foreach (XmlNode element in nodeList)
				{
					var item = new Recipe();
					item.Id = element.SelectSingleNode("Id").InnerText;
					item.Description = element.SelectSingleNode("Description").InnerText;
					item.Author = element.SelectSingleNode("Author").InnerText;
					item.Ingredients = new List<Ingredient>();
					var ingredients = element.SelectNodes("Ingredient");
					foreach (XmlNode gred in ingredients)
					{
						var ingred = new Ingredient();
						ingred.Name = gred.Attributes["Name"].Value;
						ingred.Amount = gred.Attributes["Amount"].Value;
						ingred.Unit = gred.Attributes["Unit"].Value;
						ingred.Note = gred.Attributes["Note"].Value;
						item.Ingredients.Add(ingred);
					}
					item.Instructions = FormatInstructions(element.SelectSingleNode("Instructions").InnerText);
					item.Info = element.SelectSingleNode("Info").InnerText;
					item.Image = element.SelectSingleNode("Image").InnerText;
					items.Add(item);
				}
				HttpRuntime.Cache["recipes"] = items;
			}
			return items;
		}

		private string FormatInstructions(string innerText)
		{
			return innerText.Replace("[BR]", "<BR>");
		}
	}
}
