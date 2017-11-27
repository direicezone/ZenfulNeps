using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Web.Services;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Linq;
namespace DiabloIIIApi
{
	public partial class D3FollowerItems : System.Web.UI.Page
	{
		private string toolTipLink = "http://us.battle.net/d3/en/";
		private int altRowCount;
		private string _heroName;

		protected void Page_Load(object sender, EventArgs e)
		{

			var diabloIIIApi = new DiabloIIIApi();
			//var api_Hero_Details = diabloIIIApi.GetHeroFromAPI("direice#1124", "44792928"); //Velangel
			//var api_Item_Details = diabloIIIApi.GetItemDetailsFromAPI("item/CoEBCKuzwqUHEgcIBBXNcusTHWYjBlAdgYHGxB1nPuzkHZsGAMsdf46HzDCLAji7A0AAUBJYBGC7A2opCgwIABD3pOXwgICAwDwSGQic56vKDBIHCAQVhSeUsTCPAjgAQAGQAQCAAUalAWc-7OStAeYV2w21AX_5Tl24AYSliuEFwAESGJrL274CUApYAg");


			if (string.IsNullOrEmpty(txtBattleTag.Value))
			{
				tableHero.Visible = false;
				return;
			}
			try
			{
				lblError.Visible = false;
				tableHero.Visible = true;
				var api_Career = diabloIIIApi.GetCareerFromAPI(txtBattleTag.Value);
				foreach (var hero in api_Career.heroes.Where(h => h.level == 70).ToList())
				{
					_heroName = hero.name;
					var api_Hero_Details = diabloIIIApi.GetHeroFromAPI(txtBattleTag.Value, hero.id.ToString()); 
					var formattedHeroName = string.Format("{0} {1} ({2}) {3} {4} {5}", hero.name, hero.level, hero.paragonLevel,
														  hero.className, hero.seasonal ? "S" : string.Empty,
														  hero.hardcore ? "H" : string.Empty);
					if (selFollower.Value == "Enchantress")
						AddItemToTable(formattedHeroName, hero.id.ToString(), "Enchantress", diabloIIIApi.GetFollowerItems(api_Hero_Details.followers.enchantress));
					if (selFollower.Value == "Scoundrel")
						AddItemToTable(formattedHeroName, hero.id.ToString(), "Scoundrel", diabloIIIApi.GetFollowerItems(api_Hero_Details.followers.scoundrel));
					if (selFollower.Value == "Templar")
						AddItemToTable(formattedHeroName, hero.id.ToString(), "Templar", diabloIIIApi.GetFollowerItems(api_Hero_Details.followers.templar));
					if (selFollower.Value == "All")
					{
						AddItemToTable(formattedHeroName, hero.id.ToString(), "Enchantress", diabloIIIApi.GetFollowerItems(api_Hero_Details.followers.enchantress));
						AddItemToTable(string.Empty, hero.id.ToString(), "Scoundrel", diabloIIIApi.GetFollowerItems(api_Hero_Details.followers.scoundrel));
						AddItemToTable(string.Empty, hero.id.ToString(), "Templar", diabloIIIApi.GetFollowerItems(api_Hero_Details.followers.templar));
					}
					++altRowCount;
				}
			}
			catch (Exception exception)
			{
				tableHero.Visible = false;
				lblError.InnerText =
					string.Format("BattleTag not found or is incorrectly formatted (tagname#1234) or the service is down. {0}",
					              exception.Message);
				lblError.Visible = true;
			}
			
		}

		private void AddItemToTable(string heroName, string heroId, string follower, List<ApiItem> followerItems)
		{
			var className = altRowCount % 2 == 0 ? string.Empty : "alt";
			var row = new HtmlTableRow();
			var cell1 = new HtmlTableCell();
			if (!string.IsNullOrEmpty(heroName))
			{
				CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
				TextInfo textInfo = cultureInfo.TextInfo;
				heroName = textInfo.ToTitleCase(heroName);
				var heroHref = new HyperLink();
				heroHref.Text = heroName;
				heroHref.NavigateUrl = "#";
				heroHref.Attributes.Add("onclick", "return false;");
				heroHref.Attributes.Add("class", "opener");
				heroHref.Attributes.Add("data-heroname", heroName);
				heroHref.Attributes.Add("data-heroid", heroId);
				cell1.Controls.Add(heroHref);
			}
			cell1.Attributes.Add("class", className);
			row.Cells.Add(cell1);
			var cell2 = new HtmlTableCell { InnerText = follower };
			cell2.Attributes.Add("class", className);
			row.Cells.Add(cell2);

			if (followerItems.Count == 0)
			{
				for (int i = 0; i < 6; i++)
				{
					var cellfill = new HtmlTableCell { InnerText = string.Empty };
					cellfill.Attributes.Add("class", className);
					row.Cells.Add(cellfill);
				}
			}
			foreach (var followerItem in followerItems)
			{
				if (followerItem != null)
				{
					var href = new HyperLink();
					href.Text = followerItem.name;
					href.NavigateUrl = toolTipLink + followerItem.tooltipParams;
					href.Attributes.Add("onclick", "return false;");
					var cell = new HtmlTableCell();
					cell.Attributes.Add("class", className);
					cell.Controls.Add(href);
					row.Cells.Add(cell);
				}
				else
				{
					var cellfill2 = new HtmlTableCell { InnerText = string.Empty };
					cellfill2.Attributes.Add("class", className);
					row.Cells.Add(cellfill2);
				}
			}
			tableHero.Rows.Add(row);
		}

		[WebMethod(true)]
		public static HeroDetails GetHeroInfo(string battleTag, string heroId)
		{
			var diabloIIIApi = new DiabloIIIApi();
			var api_Hero_Details = diabloIIIApi.GetHeroFromAPI(battleTag, heroId);
			var heroItems = new List<ItemDetails>();
			var itemList = from prop in api_Hero_Details.items.GetType().GetProperties() where prop != null select prop;
			foreach (var property in itemList)
			{
				var toolTip = ((ApiItem)property.GetValue(api_Hero_Details.items)).tooltipParams;
				heroItems.Add(diabloIIIApi.GetItemDetailsFromAPI(toolTip));
			}
			api_Hero_Details.stats.cooldown = diabloIIIApi.GetCoolDown(heroItems);
			
			//////TEST BED TEST BEDDDDDD
			var heavenly = diabloIIIApi.IsPassiveActive(api_Hero_Details, "Heavenly Strength");

			//////END TEST BED TEST BEDDDDDD
			
			return api_Hero_Details;

		}
	}
}