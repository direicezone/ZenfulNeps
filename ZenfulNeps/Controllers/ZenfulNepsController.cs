using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using System.Web.SessionState;
using System.Xml;
using System.Xml.Linq;
using ZenfulNeps.Models;

namespace ZenfulNeps.Controllers
{
    public class ZenfulNepsController : Controller
    {

        public ActionResult Index()
        {
			var feeds = GetFeeds();
	        var zenfulModel = new Models.ZenfulNeps();
	        zenfulModel.RssFeeds = feeds;
			return View("ZenfulNeps", zenfulModel);
        }

		public ActionResult Archive()
		{
			return View("Archive");
		}

		public ActionResult BodyFatCalculator()
		{
			return View("BodyFatCalc");
		}

		public ActionResult DeadComedians()
		{
			return View("DeadComedians");
		}

		public ActionResult MashSpargeWater()
		{
			return View("MashSpargeWater");
		}

		public ActionResult MaltExtractConversion()
		{
			return View("MaltExtractConversion");
		}

		public ActionResult FantasyDrafter()
		{
			Session.Timeout = 240;
			return View("FantasyDrafter");
		}

		[WebMethod(EnableSession = true), AcceptVerbs("POST")]
		public void SaveKeepers(string[] Keepers)
		{
			System.Web.HttpContext.Current.Session["FFKeepers"] = Keepers;
		}

		[WebMethod(EnableSession = true), AcceptVerbs("POST")]
		public void SaveMyPlayers(string[] MyPlayers)
		{
			System.Web.HttpContext.Current.Session["MyPlayers"] = MyPlayers;
		}

		[WebMethod(EnableSession = true), AcceptVerbs("GET")]
		public JsonResult GetKeepers()
		{
			if (System.Web.HttpContext.Current.Session["FFKeepers"] == null)
				return Json(new
				{
					Keepers = string.Empty
				}, JsonRequestBehavior.AllowGet);
			var keepers = (string[]) System.Web.HttpContext.Current.Session["FFKeepers"];
			return Json(new
			{
				Keepers = keepers
			}, JsonRequestBehavior.AllowGet);
		}

		[WebMethod(EnableSession = true), AcceptVerbs("GET")]
		public JsonResult GetMyPlayers()
		{
			if (System.Web.HttpContext.Current.Session["MyPlayers"] == null)
				return Json(new
				{
					MyPlayers = string.Empty
				}, JsonRequestBehavior.AllowGet);
			var myplayers = (string[])System.Web.HttpContext.Current.Session["MyPlayers"];
			return Json(new
			{
				MyPlayers = myplayers
			}, JsonRequestBehavior.AllowGet);
		}

	    public List<Rss> GetFeeds()
		{
			var rssFeed = new List<Rss>();
		    var myTimer = new Stopwatch();
			rssFeed.Add(GetFeed("http://www.homebrewing.com/rssfull.php", "From Homebrewing"));
			rssFeed.Add(GetFeed("http://feeds2.feedburner.com/zenhabits?format=xml", "From Zen Habits"));
			rssFeed.Add(GetFeed("http://feeds.tasteofhome.com/taste-of-home/lower-fat-recipes/", "Taste of Home - Lower Fat Recipe"));
			rssFeed.Add(GetFeed("http://www.history.com/this-day-in-history/rss", "Today in History"));
			//rssFeed.Add(GetFeed("http://www.merriam-webster.com/word/index.xml", "Word of the Day"));
			return rssFeed;
		}

		private Rss GetFeed(string blogURL, string heading)
		{
			try
			{
				XDocument feedXml = XDocument.Load(blogURL);
				var feeds = from feed in feedXml.Descendants("item")
							select new Rss
							{
								Heading = heading,
								Title = feed.Element("title").Value,
								Link = feed.Element("link").Value,
								Description = feed.Element("description").Value
							};
				return feeds.FirstOrDefault();
			}
			catch (Exception)
			{
				return null;
				//return new Rss
				//{
				//	Heading = "Error retrieving feed: (" + blogURL + ")",
				//	Title = "Error retrieving feed",
				//	Link = "#",
				//	Description = "Error retrieving feed"
				//};
			}
		}

		public JsonResult GetMeasurements(string chest, string abdomen, string thigh, string tricep, 
			string subscapular, string suprailiac, string midaxilla, string bicep, string calf, 
			string lowerback, string sex, string age, string weight)
		{
			if (isnumeric(chest, NumberStyles.Integer) && isnumeric(abdomen, NumberStyles.Integer) && isnumeric(thigh, NumberStyles.Integer) && 
				isnumeric(subscapular, NumberStyles.Integer) && isnumeric(suprailiac, NumberStyles.Integer) && isnumeric(midaxilla, NumberStyles.Integer) &&
				isnumeric(bicep, NumberStyles.Integer) && isnumeric(calf, NumberStyles.Integer) && isnumeric(lowerback, NumberStyles.Integer) && 
				isnumeric(age, NumberStyles.Integer) && isnumeric(weight, NumberStyles.Integer))
			{
				var bodyFat = new BodyFat();
				bodyFat.JP7Percent = string.Format("{0:#.##}", JP7(chest, abdomen, thigh, tricep, subscapular, suprailiac, midaxilla, sex, age));
				bodyFat.JP3Percent = string.Format("{0:#.##}", JP3(chest, abdomen, thigh, tricep, suprailiac, sex, age));
				bodyFat.PA3Percent = string.Format("{0:#.##}", PA3(chest, tricep, subscapular, abdomen, suprailiac, sex, age));
				bodyFat.P9Percent = string.Format("{0:#.##}", P9(chest, abdomen, thigh, tricep, subscapular, suprailiac, bicep, calf, lowerback, sex, weight));
				bodyFat.AverageBF = string.Empty;
				bodyFat.AverageBF = sex == "M"
					                    ? string.Format("{0:#.##}",
					                                    (Convert.ToDouble(bodyFat.JP7Percent) + Convert.ToDouble(bodyFat.JP3Percent) +
					                                     Convert.ToDouble(bodyFat.PA3Percent) + Convert.ToDouble(bodyFat.P9Percent))/4)
					                    : string.Format("{0:#.##}",
					                                    (Convert.ToDouble(bodyFat.JP7Percent) + Convert.ToDouble(bodyFat.JP3Percent) +
					                                     Convert.ToDouble(bodyFat.PA3Percent))/3);
				bodyFat.FatWeight = string.Format("{0:#.##}", (Convert.ToDouble(weight) * Convert.ToDouble(bodyFat.AverageBF) / 100));
				bodyFat.LeanWeight = string.Format("{0:#.##}", Convert.ToDouble(weight) - Convert.ToDouble(bodyFat.FatWeight));
				return Json(bodyFat, JsonRequestBehavior.AllowGet);
			}
			else
			{
				return Json("ERROR", JsonRequestBehavior.AllowGet);

			}
		}

		public bool isnumeric(string val, System.Globalization.NumberStyles NumberStyle)
		{
			Double result;
			return Double.TryParse(val, NumberStyle, System.Globalization.CultureInfo.CurrentCulture, out result);
		}

		static double JP7(string oChest, string oAbdomen, string oThigh, string oTricep, string oSubscapular,
			string oSuprailiac, string oMidaxilla, string oSex, string oAge)
		{
			double BD;
			double Skinfold_Sum = Convert.ToDouble(oChest) + Convert.ToDouble(oAbdomen) + Convert.ToDouble(oThigh) + Convert.ToDouble(oTricep) +
				Convert.ToDouble(oSubscapular) + Convert.ToDouble(oSuprailiac) + Convert.ToDouble(oMidaxilla);
			if (oSex.Trim() == "M")
			{
				BD = 1.112 - (0.00043499 * Skinfold_Sum) + (0.00000055 * Skinfold_Sum * Skinfold_Sum) - (0.00028826 * Convert.ToInt16(oAge));
			}
			else
			{
				BD = 1.097 - (0.00046971 * Skinfold_Sum) + (0.00000056 * Skinfold_Sum * Skinfold_Sum) - (0.00012828 * Convert.ToInt16(oAge));
			}
			return 495 / BD - 450;
		}

		static double JP3(string oChest, string oAbdomen, string oThigh, string oTricep, string oSuprailiac, string oSex, string oAge)
		{
			double BD;
			double Skinfold_Sum;
			if (oSex.Trim() == "M")
			{
				Skinfold_Sum = Convert.ToDouble(oChest) + Convert.ToDouble(oAbdomen) + Convert.ToDouble(oThigh);
				BD = 1.10938 - (0.0008267 * Skinfold_Sum) + (0.0000016 * Skinfold_Sum * Skinfold_Sum) - (0.0002574 * Convert.ToInt16(oAge));
			}
			else
			{
				Skinfold_Sum = Convert.ToDouble(oAbdomen) + Convert.ToDouble(oTricep) + Convert.ToDouble(oSuprailiac);
				BD = 1.089733 - (0.0009245 * Skinfold_Sum) + (0.0000025 * Skinfold_Sum * Skinfold_Sum) - (0.0000979 * Convert.ToInt16(oAge));
			}
			return 495 / BD - 450;
		}

		static double PA3(string oChest, string oTricep, string oSubscapular, string oAbdomen, string oSuprailiac, string oSex, string oAge)
		{
			double BD;
			double Skinfold_Sum;
			if (oSex.Trim() == "M")
			{
				Skinfold_Sum = Convert.ToDouble(oChest) + Convert.ToDouble(oTricep) + Convert.ToDouble(oSubscapular);
				BD = 1.1125025 - (0.0013125 * Skinfold_Sum) + (0.0000055 * Skinfold_Sum * Skinfold_Sum) - (0.000244 * Convert.ToInt16(oAge));
			}
			else
			{
				Skinfold_Sum = Convert.ToDouble(oAbdomen) + Convert.ToDouble(oTricep) + Convert.ToDouble(oSuprailiac);
				BD = 1.0902369 - (0.0009379 * Skinfold_Sum) + (0.0000026 * Skinfold_Sum * Skinfold_Sum) - (0.00001087 * Convert.ToInt16(oAge));
			}
			return 495 / BD - 450;
		}

		static double P9(string oChest, string oAbdomen, string oThigh, string oTricep, string oSubscapular, string oSuprailiac, string oBicep,
			string oCalf, string oLowerback, string oSex, string oWeight)
		{
			double Skinfold_Sum = Convert.ToDouble(oChest) + Convert.ToDouble(oAbdomen) + Convert.ToDouble(oThigh) +
				Convert.ToDouble(oTricep) + Convert.ToDouble(oSubscapular) + Convert.ToDouble(oSuprailiac) +
				Convert.ToDouble(oBicep) + Convert.ToDouble(oCalf) + Convert.ToDouble(oLowerback);
			if (oSex.Trim() == "M")
			{
				return 27 * Skinfold_Sum / Convert.ToInt16(oWeight);
			}
			return 0;
		}

    }
}
