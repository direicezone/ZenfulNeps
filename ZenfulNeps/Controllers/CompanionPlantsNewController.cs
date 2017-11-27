using System.Linq;
using System.Web.Mvc;

namespace ZenfulNeps.Controllers
{
	public class CompanionPlantsNewController : Controller
	{
		public ActionResult Index()
		{
			return View("CompanionPlantsNew", Common.Core.Common.GetData(false));
		}

		public ActionResult Search(string searchValue, string pestValue, string button)
		{
			if (searchValue == null && button == null && Session["SearchValue"] == null)
			{
				return View("CompanionPlantsNew", Common.Core.Common.GetData(false));
			}
			var companionPlants = Common.Core.Common.GetData(false);

			if ((Session["SearchValue"] != null && button == null) || (!string.IsNullOrEmpty(searchValue) && button == "Search"))
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
				companionPlants = Common.Core.Common.GetData(true);
			}

			return View("CompanionPlantsNew", companionPlants);
		}
	}
}