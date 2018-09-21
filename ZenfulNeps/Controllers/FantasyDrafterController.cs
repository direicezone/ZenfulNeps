using System;
using System.IO;
using System.Net;
using System.Web.Mvc;
using System.Web.Services;

namespace ZenfulNeps.Controllers
{
	public class FantasyDrafterController : Controller
    {
		private const string PLAYER_BUILT = "BUILT";
		private const string FFTODAY_URL = "http://www.fftoday.com/rankings/playerrank.php?&PosID={0}";
	    private const string FILES_LOCATION = "/Data/{0}.txt";
		private bool RebuildFiles;

		public enum Players
		{
			QuarterBacks = 10,
			RunningBacks = 20,
			WideReceivers = 30,
			TightEnds = 40,
			Kickers = 80,
			Defense = 99,
		}

		public ActionResult Index()
        {
			Session.Timeout = 240;
	        RebuildFiles = Request.QueryString["rebuild"] != null;
			if (System.Web.HttpContext.Current.Session["PlayersBuilt"] == null || RebuildFiles)
			{
				foreach (Players player in Enum.GetValues(typeof(Players)))
				{
					BuildPlayers(player);
				}               
				System.Web.HttpContext.Current.Session["PlayersBuilt"] = PLAYER_BUILT;
			}
			return View("FantasyDrafter");
		}

		private void BuildPlayers(Players players)
		{
			if (!FilesExists(players.ToString()) || RebuildFiles)
			{
				using (WebClient client = new WebClient()) // WebClient class inherits IDisposable
				{
					string htmlCode = client.DownloadString(string.Format(FFTODAY_URL, (int)players));
					var startPos = htmlCode.IndexOf("<TD class=\"bodycontent\" ALIGN=\"center\" BGCOLOR=\"#ffffff\">&nbsp;</TD>");
					if (players == Players.Defense || startPos < 0)
					{
						startPos = htmlCode.IndexOf("<TD class=\"sort1\" ALIGN=\"center\" BGCOLOR=\"#ffffff\">&nbsp;</TD>");
					}
					htmlCode = htmlCode.Substring(startPos - 5);
					var endPos = htmlCode.IndexOf("</table>");
					htmlCode = htmlCode.Substring(0, endPos - 1);
					htmlCode = CleanUpHtml(htmlCode);
					System.Web.HttpContext.Current.Session[players.ToString()] = htmlCode;
					//Build Files
					var mypath = Server.MapPath(string.Format(FILES_LOCATION, players.ToString()));
					System.IO.File.WriteAllText(mypath, htmlCode);
				}
			}
			else
			{
				var mypath = Server.MapPath(string.Format(FILES_LOCATION, players.ToString()));
				var htmlCode = System.IO.File.ReadAllText(mypath);
				System.Web.HttpContext.Current.Session[players.ToString()] = htmlCode;
			}
		}

	    private bool FilesExists(string fileName)
	    {
			var mypath = Server.MapPath(string.Format(FILES_LOCATION, fileName));
			return System.IO.File.Exists(mypath);
	    }

	    private string CleanUpHtml(string rawHtml)
		{
			var cleanedHtml = string.Empty;
			var selectFrom = "&nbsp;";
			var selectTo = "<div class=\"pretty p-default p - thick\"><input type=\"checkbox\" class=\"taken\"/><div class=\"state\"><label></label></div></div>";
			cleanedHtml = rawHtml.Replace(selectFrom, selectTo);
			var upImage = "<img SRC=\"../common/icn_arrow_Up_medium.gif\" width=\"11\" height=\"9\" border=\"0\">";
			cleanedHtml = cleanedHtml.Replace(upImage, selectTo);
			var downImage = "<img SRC=\"../common/icn_arrow_Down_medium.gif\" width=\"11\" height=\"9\" border=\"0\">";
			cleanedHtml = cleanedHtml.Replace(downImage, selectTo);
			cleanedHtml = cleanedHtml.Replace("bodycontent", "sort1");
			cleanedHtml = cleanedHtml.Replace("<TR>", "<TR class=\"note-td\">");
			return cleanedHtml;
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
			var keepers = (string[])System.Web.HttpContext.Current.Session["FFKeepers"];
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

	}
}
