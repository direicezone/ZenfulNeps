using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web.Script.Serialization;
using System.Web.UI;

namespace DiabloIIIApi
{
	public class DiabloIIIApi : Page
	{
		//New Items using Diablo 3 Web API
		public D3APIData GetCareerFromAPI(string battleTag)
		{
			battleTag = Server.UrlEncode(battleTag);
			var locale = ConfigurationManager.AppSettings["BattleNetLocal"];
			var battleNetUrlForProfile = ConfigurationManager.AppSettings["BattleNetUrlForProfile"];
			var apiKey = ConfigurationManager.AppSettings["BattleNetAPIKey"];
			var callUri = String.Format("{0}{1}/?locale={2}&apikey={3}", battleNetUrlForProfile, battleTag, locale, apiKey);
			var webClient = new WebClient();
			var returnVal = webClient.DownloadString(callUri);
			return new JavaScriptSerializer().Deserialize<D3APIData>(returnVal.Replace("\"class\"", "className"));
		}

		public HeroDetails GetHeroFromAPI(string battleTag, string id)
		{
			battleTag = Server.UrlEncode(battleTag);
			var locale = ConfigurationManager.AppSettings["BattleNetLocal"];
			var battleNetUrlForProfile = ConfigurationManager.AppSettings["BattleNetUrlForProfile"];
			var apiKey = ConfigurationManager.AppSettings["BattleNetAPIKey"];
			var callUri = String.Format("{0}{1}/hero/{2}?locale={3}&apikey={4}", battleNetUrlForProfile, battleTag, id, locale, apiKey);
			var webClient = new WebClient();
			var returnVal = webClient.DownloadString(callUri);
			returnVal = returnVal.Replace("\"class\"", "className");
			returnVal = returnVal.Replace("last-updated", "lastupdated");
			var heroDetails = new JavaScriptSerializer().Deserialize<HeroDetails>(returnVal);
			return heroDetails;

			//var itemList = from prop in heroDetails.items.GetType().GetProperties() where prop != null select prop;
			//foreach (var property in itemList)
			//{
			//	if (property.Name == "bracers") ((ApiItem)property.GetValue(heroDetails.items)).type = ItemType.Bracers;
			//}

			//var getBracer = from prop in heroDetails.items.GetType().GetProperties()
			//				where ((ApiItem) prop.GetValue(heroDetails.items)).type == ItemType.Bracers
			//				select (ApiItem) prop.GetValue(heroDetails.items);


			//var tester =
			//	heroDetails.items.GetType()
			//			   .GetProperties()
			//			   .Where(w => w.Name == "bracers")
			//			   .Select(s => (ApiItem) s.GetValue(heroDetails.items));

		}

		public ItemDetails GetItemDetailsFromAPI(string id)
		{
			var battleNetUrlForItems = ConfigurationManager.AppSettings["BattleNetUrlForItems"];
			var apiKey = ConfigurationManager.AppSettings["BattleNetAPIKey"];
			var locale = ConfigurationManager.AppSettings["BattleNetLocal"];
			var callUri = String.Format("{0}{1}?locale={2}&apikey={3}", battleNetUrlForItems, id, locale, apiKey);
			var webClient = new WebClient();
			var returnVal = webClient.DownloadString(callUri);
			returnVal = returnVal.Replace("last-updated", "lastupdated");
			returnVal = returnVal.Replace("Resistance#Lightning", "Resistance_Lightning");
			returnVal = returnVal.Replace("Resistance#Physical", "Resistance_Physical");
			returnVal = returnVal.Replace("Resistance#Cold", "Resistance_Cold");
			returnVal = returnVal.Replace("Resistance#Fire", "Resistance_Fire");
			returnVal = returnVal.Replace("Resistance#Poison", "Resistance_Poison");
			returnVal = returnVal.Replace("Resistance#Arcane", "Resistance_Arcane");
			returnVal = returnVal.Replace("Damage_Weapon_Delta#Lightning", "Damage_Weapon_Delta_Lightning");
			returnVal = returnVal.Replace("Damage_Weapon_Delta#Poison", "Damage_Weapon_Delta_Poison");
			returnVal = returnVal.Replace("Damage_Weapon_Delta#Cold", "Damage_Weapon_Delta_Cold");
			returnVal = returnVal.Replace("Damage_Weapon_Delta#Fire", "Damage_Weapon_Delta_Fire");
			returnVal = returnVal.Replace("Damage_Weapon_Delta#Arcane", "Damage_Weapon_Delta_Arcane");
			returnVal = returnVal.Replace("Damage_Weapon_Delta#Holy", "Damage_Weapon_Delta_Holy");
			returnVal = returnVal.Replace("Damage_Weapon_Min#Lightning", "Damage_Weapon_Min_Lightning");
			returnVal = returnVal.Replace("Damage_Weapon_Min#Physical", "Damage_Weapon_Min_Physical");
			returnVal = returnVal.Replace("Resource_Max_Bonus#Faith", "Resource_Max_Bonus_Faith");
			returnVal = returnVal.Replace("Damage_Weapon_Delta#Physical", "Damage_Weapon_Delta_Physical");
			returnVal = returnVal.Replace("Damage_Dealt_Percent_Bonus#Fire", "Damage_Dealt_Percent_Bonus_Fire");
			returnVal = returnVal.Replace("Damage_Dealt_Percent_Bonus#Cold", "Damage_Dealt_Percent_Bonus_Cold");
			returnVal = returnVal.Replace("Damage_Dealt_Percent_Bonus#Poison", "Damage_Dealt_Percent_Bonus_Poison");
			returnVal = returnVal.Replace("Damage_Dealt_Percent_Bonus#Holy", "Damage_Dealt_Percent_Bonus_Holy");
			returnVal = returnVal.Replace("Damage_Dealt_Percent_Bonus#Physical", "Damage_Dealt_Percent_Bonus_Physical");
			returnVal = returnVal.Replace("Damage_Dealt_Percent_Bonus#Lightning", "Damage_Dealt_Percent_Bonus_Lightning");
			returnVal = returnVal.Replace("Damage_Dealt_Percent_Bonus#Arcane", "Damage_Dealt_Percent_Bonus_Arcane");
			var itemDetails = new JavaScriptSerializer().Deserialize<ItemDetails>(returnVal);
			return itemDetails;
		}

		public List<ApiItem> GetFollowerItems(HerosFollower follower)
		{
			var followerItems = new List<ApiItem>();
			if (follower == null) return followerItems;
			followerItems.Add(follower.items.special);
			followerItems.Add(follower.items.mainHand);
			followerItems.Add(follower.items.offHand);
			followerItems.Add(follower.items.leftFinger);
			followerItems.Add(follower.items.rightFinger);
			followerItems.Add(follower.items.neck);
			return followerItems;
		}

		public decimal GetCoolDown(List<ItemDetails> heroItems)
		{
			var itemCooldown =
				heroItems.Where(w => w.attributesRaw.Power_Cooldown_Reduction_Percent_All != null)
				         .Select(s => s.attributesRaw.Power_Cooldown_Reduction_Percent_All.max)
				         .ToList();

			if (heroItems[0] != null)//Helm is first item in the list
			{
				var attrMultiplier = heroItems[0].attributesRaw.Gem_Attributes_Multiplier != null
					                     ? heroItems[0].attributesRaw.Gem_Attributes_Multiplier.max
					                     : 0;
				decimal gemCooldown = 0;
				if (heroItems[0].gems.Count > 0)
				{
					var geminfo =
						heroItems[0].gems.Where(w => w.attributesRaw.Power_Cooldown_Reduction_Percent_All != null && w.isGem)
						            .Select(s => s)
						            .FirstOrDefault();
					if (geminfo != null && geminfo.attributesRaw.Power_Cooldown_Reduction_Percent_All != null)
					{
						gemCooldown = geminfo.attributesRaw.Power_Cooldown_Reduction_Percent_All.max;
					}
				}
				if (gemCooldown != 0)
				{
					if (attrMultiplier != 0)
					{
						var cooldown = gemCooldown * (1 + attrMultiplier);
						itemCooldown.Add(cooldown);
					}
					else
					{
						itemCooldown.Add(gemCooldown);
					}
				}
			}
			decimal initialCooldown = 1;
			foreach (var cooldown in itemCooldown)
			{
				initialCooldown = initialCooldown - (initialCooldown * cooldown);
			}
			return (1 - initialCooldown);
		}

		public decimal GetSocketedGemCount(List<ItemDetails> heroItems)
		{
			var gemCount = 0;
			var gems = heroItems.Where(w => w.gems.Count > 0).Select(s => s.gems).ToList();
			if (gems.Count > 0)
			{
				foreach (var gem in gems)
				{
					foreach (var itemDetailsGem in gem)
					{
						gemCount += 1;
					}
				}
			}
			return gemCount;
		}

		public bool IsPassiveActive(HeroDetails heroDetails, string skillName)
		{
			return heroDetails.skills.passive.Any(w => w.skill.name.Contains(skillName));
		}

	}
}