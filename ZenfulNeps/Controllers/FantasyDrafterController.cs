using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web.Mvc;
using System.Web.Services;
using HtmlAgilityPack;

namespace ZenfulNeps.Controllers
{
	public class FantasyDrafterController : Controller
    {
		private const string PLAYER_BUILT = "BUILT";
		private const string FFKEEPERS = "FFKeepers";
		private const string MYPLAYERS = "MyPlayers";
		private const string PLAYERSDATA = "PLAYERSDATA";
        private const string FFTODAY_URL = "https://www.fftoday.com/rankings/playerrank.php?o=1&PosID={0}";
		private const string HOST_NAME = "www.fftoday.com";
	    private const string FILES_LOCATION = "/Data/{0}.txt";
		private bool RebuildFiles;
        private List<Common.Core.PlayerData> PlayersData;

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
            RebuildFiles = System.Web.HttpContext.Current.Session["rebuild"] != null;
            if (System.Web.HttpContext.Current.Session["PlayersBuilt"] == null || RebuildFiles)
			{
				foreach (Players player in Enum.GetValues(typeof(Players)))
				{
					BuildPlayers(player);
				}               
				System.Web.HttpContext.Current.Session["PlayersBuilt"] = PLAYER_BUILT;
                var sortPlayers = PlayersData.Where(w => w.Player != null).Select(s => s).OrderBy(o => o.ADP).ToList();
                HttpContext.Cache[PLAYERSDATA] = sortPlayers;
            }
            System.Web.HttpContext.Current.Session["rebuild"] = null;
            return View("FantasyDrafter");
		}

        public ActionResult Rebuild()
        {
            System.Web.HttpContext.Current.Session["rebuild"] = "YES";
            if (FilesExists(FFKEEPERS))
                System.IO.File.Delete(Server.MapPath(string.Format(FILES_LOCATION, FFKEEPERS)));
            if (FilesExists(MYPLAYERS))
                System.IO.File.Delete(Server.MapPath(string.Format(FILES_LOCATION, MYPLAYERS)));
            return RedirectToAction("Index");
        }

        private void BuildPlayers(Players players)
		{
            if (!FilesExists(players.ToString()) || RebuildFiles)
			{
                string htmlCode;
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3 | (SecurityProtocolType)3072;
                using (WebClient client = new WebClient()) // WebClient class inherits IDisposable
                {
                    htmlCode = client.DownloadString(string.Format(FFTODAY_URL, (int)players));
                }
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
                if (PlayersData == null)
                    PlayersData = new List<Common.Core.PlayerData>();
                PlayersData.AddRange(BuildData(htmlCode, players));
            }
            else
			{
				var mypath = Server.MapPath(string.Format(FILES_LOCATION, players.ToString()));
				var htmlCode = System.IO.File.ReadAllText(mypath);
				System.Web.HttpContext.Current.Session[players.ToString()] = htmlCode;
                if (PlayersData == null)
                    PlayersData = new List<Common.Core.PlayerData>();
                PlayersData.AddRange(BuildData(htmlCode, players));
            }
        }

        private List<Common.Core.PlayerData> BuildData(string htmlCode, Players players)
        {
            var playersData = new List<Common.Core.PlayerData>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlCode);
            foreach (HtmlNode row in doc.DocumentNode.SelectNodes("tr"))
            {
                ///This is the row.
                var player = new Common.Core.PlayerData();
                player.Type = players.ToString();
                player.Taken = false;
                var ppos = 0;
                foreach (HtmlNode cell in row.SelectNodes("td"))
                {
                    var data1 = cell.InnerText;
                    switch (ppos)
                    {
                        case 1:
                            player.Rank = Convert.ToInt16(data1);
                            break;
                        case 2:
                            player.Player = data1.Trim();
                            break;
                        case 3:
                            player.Team = data1.Trim();
                            break;
                        case 4:
                            player.ByeWeek = Convert.ToInt16(data1);
                            break;
                        case 5:
                            player.ADP = Convert.ToDouble(data1.Replace("-", "100"));
                            break;
                        default:
                            break;
                    }

                    ++ppos;
                }
                playersData.Add(player);
            }
            return playersData;
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
            var playersData = (List<Common.Core.PlayerData>)HttpContext.Cache[PLAYERSDATA];
            playersData.ForEach(fe => fe.Taken = false);
            if (Keepers == null)
            {
                HttpContext.Cache.Remove(FFKEEPERS);
                return;
            }
            HttpContext.Cache[FFKEEPERS] = Keepers;
            foreach (var keeper in Keepers)
            {
                playersData.First(w => w.Player == keeper.Trim()).Taken = true;
            }
            HttpContext.Cache[PLAYERSDATA] = playersData;
        }

        [WebMethod(EnableSession = true), AcceptVerbs("POST")]
		public void SaveMyPlayers(string[] MyPlayers)
		{
            if (MyPlayers == null)
            {
                HttpContext.Cache.Remove(MYPLAYERS);
                return;
            }
            HttpContext.Cache[MYPLAYERS] = MyPlayers;
        }

        [WebMethod(EnableSession = true), AcceptVerbs("GET")]
		public JsonResult GetKeepers()
		{
            if (HttpContext.Cache[FFKEEPERS] == null)
                return Json(new
                {
                    Keepers = string.Empty
                }, JsonRequestBehavior.AllowGet);

            var keepers = (string[])HttpContext.Cache[FFKEEPERS];
            return Json(new
            {
                Keepers = keepers
            }, JsonRequestBehavior.AllowGet);
        }

        [WebMethod(EnableSession = true), AcceptVerbs("GET")]
		public JsonResult GetMyPlayers()
		{
			if (HttpContext.Cache[MYPLAYERS] == null)
				return Json(new
				{
					MyPlayers = string.Empty
				}, JsonRequestBehavior.AllowGet);

            var myplayers = (string[])HttpContext.Cache[MYPLAYERS];
            return Json(new
			{
				MyPlayers = myplayers
			}, JsonRequestBehavior.AllowGet);
		}

        [WebMethod(EnableSession = true), AcceptVerbs("POST")]
        public JsonResult GetADP(string position)
        {
            if (HttpContext.Cache[PLAYERSDATA] == null)
                return Json(new
                {
                    ADP = string.Empty
                }, JsonRequestBehavior.AllowGet);

            var players = (List<Common.Core.PlayerData>)HttpContext.Cache[PLAYERSDATA];
            var adp = players.Where(w => w.Taken == false).ToList();
            return Json(new
            {
                ADP = adp
            }, JsonRequestBehavior.AllowGet);
        }


    }
}
