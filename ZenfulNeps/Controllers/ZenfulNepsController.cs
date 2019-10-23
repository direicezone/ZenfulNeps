using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Services;
using System.Xml.Linq;
using ZenfulNeps.Models;
using HtmlAgilityPack;

namespace ZenfulNeps.Controllers
{

	public class ZenfulNepsController : Controller
	{
        public List<RawRssInfo> randomRssInfoList;
		public List<RawRssInfo> rawRssInfoList = new List<RawRssInfo>
		{
			new RawRssInfo
			{
				RssLink = "https://www.gardeningknowhow.com/feed",
				RssHeading = "Gardening Know How's Blog",
				RssHost = "www.gardeningknowhow.com"
			},
			new RawRssInfo
			{
				RssLink = "https://www.skinnytaste.com/feed/",
				RssHeading = "skinnytaste",
				RssHost = "www.skinnytaste.com"
            },
			new RawRssInfo
			{
				RssLink = "https://www.chef-in-training.com/feed/",
				RssHeading = "Chef in Training",
				RssHost = "www.chef-in-training.com"
            },
			new RawRssInfo
			{
				RssLink = "https://therecipecritic.com/feed/",
				RssHeading = "The Recipe Critic",
				RssHost = "therecipecritic.com"
            },
			new RawRssInfo
			{
				RssLink = "http://brulosophy.com/feed/",
				RssHeading = "From Brülosophy",
				RssHost = "brulosophy.com"
			},
			new RawRssInfo
			{
				RssLink = "http://www.homebrewfinds.com/feed",
				RssHeading = "Homebrew Finds",
				RssHost = "www.homebrewfinds.com"
			},
			new RawRssInfo {RssLink = "http://feeds2.feedburner.com/zenhabits?format=xml", RssHeading = "From Zen Habits"},
			new RawRssInfo
			{
				RssLink = "http://feeds.tasteofhome.com/taste-of-home/lower-fat-recipes/",
				RssHeading = "Taste of Home - Lower Fat Recipe"
			},
			new RawRssInfo {RssLink = "http://www.history.com/this-day-in-history/rss", RssHeading = "Today in History"},
			new RawRssInfo
			{
				RssLink = "http://beersmith.com/blog/feed/",
				RssHeading = "BeerSmith™ Home Brewing Blog",
				RssHost = "beersmith.com"
			},
            new RawRssInfo
            {
                RssLink = "https://amindfullmom.com/feed/",
                RssHeading = "A Mind 'Full' Mom",
                RssHost = "amindfullmom.com"
            },
            new RawRssInfo
            {
                RssLink = "https://theviewfromgreatisland.com/feed/",
                RssHeading = "The View from Great Island",
                RssHost = "theviewfromgreatisland.com"
            },
            new RawRssInfo
            {
                RssLink = "https://www.dinneratthezoo.com/feed/",
                RssHeading = "Dinner at the Zoo",
                RssHost = "www.dinneratthezoo.com"
            }
		};

		public ActionResult Index()
		{
			//var feeds = GetFeeds();

			var domain = Request.Url.Host.ToLower();
			if (domain.Contains("greentrussville"))
			{
				if (IsMobile())
				{
					return Redirect("~/Green/default_mobile.html");
				}
				return Redirect("~/Green/default.html");
			}
			var zenfulModel = new Models.ZenfulNeps();
			zenfulModel.RssCount = rawRssInfoList.Count();
            var rnd = new Random();
            randomRssInfoList = rawRssInfoList.OrderBy(x => rnd.Next()).ToList();
            Session["randomList"] = randomRssInfoList;
            return View("ZenfulNeps", zenfulModel);
		}

		public bool IsMobile()
		{
			string userAgent = Request.ServerVariables["HTTP_USER_AGENT"];
			Regex OS = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
			Regex device = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
			string device_info = string.Empty;
			if (OS.IsMatch(userAgent))
			{
				device_info = OS.Match(userAgent).Groups[0].Value;
			}
			if (device.IsMatch(userAgent.Substring(0, 4)))
			{
				device_info += device.Match(userAgent).Groups[0].Value;
			}
			return !string.IsNullOrEmpty(device_info);
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

		public List<Rss> GetFeeds()
		{
			var cacheKey = DateTime.Now.ToShortDateString();
			if (HttpContext.Cache[cacheKey] != null)
			{
				return (List<Rss>)HttpContext.Cache[cacheKey];
			}
			var rssFeed = new List<Rss>();
			var myTimer = new Stopwatch();
			rssFeed.Add(GetFeed("http://brulosophy.com/feed/", "From Brülosophy", "brulosophy.com"));
			rssFeed.Add(GetFeed("http://www.homebrewfinds.com/feed", "Homebrew Finds", "www.homebrewfinds.com"));
			rssFeed.Add(GetFeed("http://feeds2.feedburner.com/zenhabits?format=xml", "From Zen Habits"));
			rssFeed.Add(GetFeed("https://blog.gardeningknowhow.com/feed/", "Gardening Know How's Blog", "www.gardeningknowhow.com"));
			rssFeed.Add(GetFeed("http://feeds.tasteofhome.com/taste-of-home/lower-fat-recipes/", "Taste of Home - Lower Fat Recipe"));
			rssFeed.Add(GetFeed("http://www.history.com/this-day-in-history/rss", "Today in History"));
			rssFeed.Add(GetFeed("http://beersmith.com/blog/feed/", "BeerSmith™ Home Brewing Blog", "beersmith.com"));
			HttpContext.Cache[cacheKey] = rssFeed;
			return rssFeed;
		}

		private int _maxImageWidth = 485;

		private Rss GetFeed(string blogURL, string heading, string host = null)
		{
			try
			{
				if (host != null)
				{
					ServicePointManager.Expect100Continue = true;
					ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3 | (SecurityProtocolType)3072;
					var request = (HttpWebRequest)WebRequest.Create(blogURL);
					request.UserAgent = "(Request for Feed)";
					CookieContainer cookieJar = new CookieContainer();
					request.CookieContainer = cookieJar;
					request.Accept = @"text/html, application/xhtml+xml, */*";
					request.Referer = @"http://zenfulneps.com/";
					request.Headers.Add("Accept-Language", "en-US");
					request.UserAgent = @"Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";
					request.ContentType = "application/x-www-form-urlencoded";
					request.Host = host;
					using (var response = request.GetResponse())
					{
						using (var stream = response.GetResponseStream())
						{
							var feedXml2 = XDocument.Load(stream);
							var feeds2 = from feed in feedXml2.Descendants("item")
										 select new Rss
										 {
											 Heading = heading,
											 Title = feed.Element("title").Value,
											 Link = feed.Element("link").Value,
											 Description = feed.Element("description").Value
										 };

							List<Uri> links = new List<Uri>();
							string regexImgSrc = @"<img[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>";
							MatchCollection matchesImgSrc = Regex.Matches(feeds2.FirstOrDefault().Description, regexImgSrc, RegexOptions.IgnoreCase | RegexOptions.Singleline);
							var newDesc = string.Empty;
							var returnFeed = feeds2.FirstOrDefault();
							foreach (Match m in matchesImgSrc)
							{
								var imgTag = m.Value;
								XElement div = XElement.Parse(imgTag);
								int width = (int)div.Attribute("width");
								int height = (int)div.Attribute("height");
								var subBy = 0;
								if (width > _maxImageWidth)
								{
									subBy = width - _maxImageWidth;
								}
								if (subBy <= 0) break;
								var oldWidthString = string.Format("width=\"{0}\"", width);
								var oldHeightString = string.Format("height=\"{0}\"", height);
								var newWidthString = string.Format("width=\"{0}\"", width - subBy);
								var newHeightString = string.Format("height=\"{0}\"", height - subBy);
								returnFeed.Description = returnFeed.Description.Replace(oldWidthString, newWidthString);
								returnFeed.Description = returnFeed.Description.Replace(oldHeightString, newHeightString);
							}
							return returnFeed;
						}
					}
				}
				else
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
			}
			catch (Exception ex)
			{
				var feed = new Rss
				{
					Heading = heading,
					Title = "Broken Feed",
					Link = "Broken Link",
					Description = ex.Message
				};
				//return feed;
				return null;
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
														 Convert.ToDouble(bodyFat.PA3Percent) + Convert.ToDouble(bodyFat.P9Percent)) / 4)
										: string.Format("{0:#.##}",
														(Convert.ToDouble(bodyFat.JP7Percent) + Convert.ToDouble(bodyFat.JP3Percent) +
														 Convert.ToDouble(bodyFat.PA3Percent)) / 3);
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

		private static double JP7(string oChest, string oAbdomen, string oThigh, string oTricep, string oSubscapular,
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

		private static double JP3(string oChest, string oAbdomen, string oThigh, string oTricep, string oSuprailiac, string oSex, string oAge)
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

		private static double PA3(string oChest, string oTricep, string oSubscapular, string oAbdomen, string oSuprailiac, string oSex, string oAge)
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

		private static double P9(string oChest, string oAbdomen, string oThigh, string oTricep, string oSubscapular, string oSuprailiac, string oBicep,
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

		[WebMethod(EnableSession = true), AcceptVerbs("GET")]
		public JsonResult GetRssFeed(string rssIteration)
		{
            var cacheKey = DateTime.Now.ToShortDateString() + "-" + rssIteration;
            if (HttpContext.Cache[cacheKey] != null)
            {
                var cfeed = (Rss)HttpContext.Cache[cacheKey];
                return Json(new
                {
                    Link = cfeed.Link,
                    Heading = cfeed.Heading,
                    Title = cfeed.Title,
                    Description = cfeed.Description
                }, JsonRequestBehavior.AllowGet);
            }

           // var feedToGet = rawRssInfoList[Convert.ToInt16(rssIteration)];
            var feedToGet = ((List<RawRssInfo>)Session["randomList"])[Convert.ToInt16(rssIteration)];
            
			var feed = GetFeed(feedToGet.RssLink, feedToGet.RssHeading, feedToGet.RssHost);
		    feed.Description = CleanImagesFromHtml(feed.Description);
			HttpContext.Cache[cacheKey] = feed;
			return Json(new
			{
				Link = feed.Link,
				Heading = feed.Heading,
				Title = feed.Title,
				Description = feed.Description
			}, JsonRequestBehavior.AllowGet);
		}

	    private string CleanImagesFromHtml(string cfeedDescription)
	    {
	        HtmlDocument document = new HtmlDocument();
	        document.LoadHtml(cfeedDescription);

	        var images = document.DocumentNode.SelectNodes("//img");
	        if (images != null)
	        {
	            foreach (HtmlNode image in images)
	            {
	                image.Attributes["width"]?.Remove();
	                image.Attributes["height"]?.Remove();
	                image.SetAttributeValue("style", "max-width: 100%; height: auto; display: block; margin-left: auto; margin-right: auto;");
	                image.AppendChild(HtmlNode.CreateNode("<br/>"));
	            }
	        }
            return document.DocumentNode.InnerHtml;
        }
	}
}