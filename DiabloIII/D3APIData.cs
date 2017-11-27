using System;
using System.Collections.Generic;

namespace DiabloIIIApi
{
	[Serializable]
	public class D3APIData
	{
		public string battleTag { get; set; }
		public int paragonLevel { get; set; }
		public int paragonLevelHardcore { get; set; }
		public int paragonLevelSeason { get; set; }
		public List<Hero> heroes { get; set; }
	}

	[Serializable]
	public class Hero
	{
		public int paragonLevel { get; set; }
		public bool seasonal { get; set; }
		public string name { get; set; }
		public decimal id { get; set; }
		public int level { get; set; }
		public bool hardcore { get; set; }
		public int gender { get; set; }
		public bool dead { get; set; }
		public string className { get; set; }
	}

	[Serializable]
	public class HeroDetails
	{
		public decimal id { get; set; }
		public string name { get; set; }
		public string className { get; set; }
		public int level { get; set; }
		public int paragonLevel { get; set; }
		public bool hardcore { get; set; }
		public bool seasonal { get; set; }
		public int seasonCreated { get; set; }
		public Skills skills { get; set; }
		public HeroItems items { get; set; }
		public HerosFollowers followers { get; set; }
		public Statics stats { get; set; }
		public Kills kills { get; set; }
		public bool dead { get; set; }
		public decimal lastupdated { get; set; }
	}

	[Serializable]
	public class Kills
	{
		public decimal elites { get; set; }
	}

	[Serializable]
	public class Statics
	{
		public decimal life { get; set; }
		public decimal damage { get; set; }
		public decimal toughness { get; set; }
		public decimal healing { get; set; }
		public decimal attackSpeed { get; set; }
		public decimal armor { get; set; }
		public decimal strength { get; set; }
		public decimal dexterity { get; set; }
		public decimal vitality { get; set; }
		public decimal intelligence { get; set; }
		public decimal physicalResist { get; set; }
		public decimal fireResist { get; set; }
		public decimal coldResist { get; set; }
		public decimal lightningResist { get; set; }
		public decimal poisonResist { get; set; }
		public decimal arcaneResist { get; set; }
		public decimal critDamage { get; set; }
		public decimal blockChance { get; set; }
		public decimal blockAmountMin { get; set; }
		public decimal blockAmountMax { get; set; }
		public decimal damageIncrease { get; set; }
		public decimal critChance { get; set; }
		public decimal damageReduction { get; set; }
		public decimal thorns { get; set; }
		public decimal lifeSteal { get; set; }
		public decimal lifePerKill { get; set; }
		public decimal goldFind { get; set; }
		public decimal magicFind { get; set; }
		public decimal lifeOnHit { get; set; }
		public decimal primaryResource { get; set; }
		public decimal secondaryResource { get; set; }
		public decimal cooldown { get; set; }
	}
	
	[Serializable]
	public class HerosFollowers
	{
		public HerosFollower templar { get; set; }
		public HerosFollower scoundrel { get; set; }
		public HerosFollower enchantress { get; set; }
	}

	[Serializable]
	public class HerosFollower
	{
		public string slug { get; set; }
		public int level { get; set; }
		public FollowerApiItems items { get; set; }
		public List<FollowerSkills> skills { get; set; }
	}

	[Serializable]
	public class FollowerApiItems
	{
		public ApiItem special { get; set; }
		public ApiItem mainHand { get; set; }
		public ApiItem offHand { get; set; }
		public ApiItem rightFinger { get; set; }
		public ApiItem leftFinger { get; set; }
		public ApiItem neck { get; set; }
	}

	[Serializable]
	public class FollowerSkills
	{
		public Skill skill { get; set; }
	}

	[Serializable]
	public class HeroItems
	{
		public ApiItem head { get; set; }
		public ApiItem torso { get; set; }
		public ApiItem feet { get; set; }
		public ApiItem hands { get; set; }
		public ApiItem shoulders { get; set; }
		public ApiItem legs { get; set; }
		public ApiItem bracers { get; set; }
		public ApiItem mainHand { get; set; }
		public ApiItem offhand { get; set; }
		public ApiItem waist { get; set; }
		public ApiItem rightFinger { get; set; }
		public ApiItem leftFinger { get; set; }
		public ApiItem neck { get; set; }
	}

	[Serializable]
	public class ApiItem
	{
		public string id { get; set; }
		public string name { get; set; }
		public string icon { get; set; }
		public string tooltipParams { get; set; }
	}

	[Serializable]
	public class Skills
	{
		public List<Active> active { get; set; }
		public List<Passive> passive { get; set; }
	}

	[Serializable]
	public class Active
	{
		public Skill skill { get; set; }
		public Rune rune { get; set; }
	}

	[Serializable]
	public class Passive
	{
		public Skill skill { get; set; }
	}

	[Serializable]
	public class Skill
	{
		public string name { get; set; }
		public string icon { get; set; }
		public int level { get; set; }
		public string tooltipUrl { get; set; }
		public string description { get; set; }
		public string simpleDescription { get; set; }
		public string skillCalcId { get; set; }
	}

	[Serializable]
	public class Rune
	{
		public string type { get; set; }
		public string name { get; set; }
		public int level { get; set; }
		public string tooltipParams { get; set; }
		public string description { get; set; }
		public string simpleDescription { get; set; }
		public string skillCalcId { get; set; }
		public int order { get; set; }
	}

	//Item Details
	[Serializable]
	public class ItemDetails
	{
		public string id { get; set; }
		public string name { get; set; }
		public string icon { get; set; }
		public string displayColor { get; set; }
		public string tooltipParams { get; set; }
		public int requiredLevel { get; set; }
		public int itemLevel { get; set; }
		public int bonusAffixes { get; set; }
		public int bonusAffixesMax { get; set; }
		public bool accountBound { get; set; }
		public string flavorText { get; set; }
		public string typeName { get; set; }
		public MinMaxRange dps { get; set; }
		public MinMaxRange attacksPerSecond { get; set; }
		public MinMaxRange minDamage { get; set; }
		public MinMaxRange maxDamage { get; set; }
		public ItemDetailsType type { get; set; }
		public MinMaxRange armor { get; set; }
		public ItemDetailsAttributes attributes { get; set; }
		public ItemDetailsAttributesRaw attributesRaw { get; set; }
		public List<ItemDetailsGems> gems { get; set; }
		public ItemDetailsSet set { get; set; }
		public decimal seasonRequiredToDrop { get; set; }
		public bool isSeasonRequiredToDrop { get; set; }
		public MinMaxRange blockChance { get; set; }
	}

	[Serializable]
	public class ItemDetailsType
	{
		public bool twoHanded { get; set; }
		public string id { get; set; }
	}

	[Serializable]
	public class MinMaxRange
	{
		public decimal min { get; set; }
		public decimal max { get; set; }
	}

	[Serializable]
	public class ItemDetailsAttributes
	{
		public List<ItemDetailsAttributesDetails> primary { get; set; }
		public List<ItemDetailsAttributesDetails> secondary { get; set; }
		public List<ItemDetailsAttributesDetails> passive { get; set; }
	}

	[Serializable]
	public class ItemDetailsAttributesDetails
	{
		public string text { get; set; }
		public string color { get; set; }
		public string affixType { get; set; }
	}

	[Serializable]
	public class ItemDetailsAttributesRaw
	{
		public MinMaxRange Hitpoints_Regen_Per_Second { get; set; }
		public MinMaxRange Hitpoints_On_Kill { get; set; }
		public MinMaxRange Attacks_Per_Second_Percent { get; set; }
		public MinMaxRange Vitality { get; set; }
		public MinMaxRange Intelligence { get; set; }
		public MinMaxRange Strength { get; set; }
		public MinMaxRange Dexterity { get; set; }
		public MinMaxRange Vitality_Item { get; set; }
		public MinMaxRange Intelligence_Item { get; set; }
		public MinMaxRange Strength_Item { get; set; }
		public MinMaxRange Dexterity_Item { get; set; }
		public MinMaxRange Resistance_Lightning { get; set; }
		public MinMaxRange Resistance_Physical { get; set; }
		public MinMaxRange Resistance_Poison { get; set; }
		public MinMaxRange Resistance_Arcane { get; set; }
		public MinMaxRange Resistance_Cold { get; set; }
		public MinMaxRange Resistance_Fire { get; set; }
		public MinMaxRange Season { get; set; }
		public MinMaxRange Crit_Percent_Bonus_Capped { get; set; }
		public MinMaxRange Crit_Damage_Percent { get; set; }
		public MinMaxRange Durability_Max_Before_Reforge { get; set; }
		public MinMaxRange Item_LegendaryItem_Level_Override { get; set; }
		public MinMaxRange Durability_Max { get; set; }
		public MinMaxRange Item_Legendary_Item_Base_Item { get; set; }
		public MinMaxRange Armor_Item { get; set; }
		public MinMaxRange Armor_Bonus_Item { get; set; }
		public MinMaxRange Gem_Attributes_Multiplier { get; set; }
		public MinMaxRange Sockets { get; set; }
		public MinMaxRange Item_Binding_Level_Override { get; set; }
		public MinMaxRange Loot_2_0_Drop { get; set; }
		public MinMaxRange Durability_Cur { get; set; }
		public MinMaxRange Health_Globe_Bonus_Health { get; set; }
		public MinMaxRange ConsumableAddSockets { get; set; }
		public MinMaxRange Damage_Weapon_Delta_Physical { get; set; }
		public MinMaxRange Damage_Weapon_Delta_Lightning { get; set; }
		public MinMaxRange Damage_Weapon_Delta_Poison { get; set; }
		public MinMaxRange Damage_Weapon_Delta_Cold { get; set; }
		public MinMaxRange Damage_Weapon_Delta_Fire { get; set; }
		public MinMaxRange Damage_Weapon_Delta_Arcane { get; set; }
		public MinMaxRange Damage_Weapon_Delta_Holy { get; set; }
		public MinMaxRange Attacks_Per_Second_Item { get; set; }
		public MinMaxRange Damage_Weapon_Min_Lightning { get; set; }
		public MinMaxRange Damage_Weapon_Min_Physical { get; set; }
		public MinMaxRange Resource_Max_Bonus_Faith { get; set; }
		public MinMaxRange Block_Amount_Item_Delta { get; set; }
		public MinMaxRange Block_Chance_Item { get; set; }
		public MinMaxRange Block_Amount_Item_Min { get; set; }
		public MinMaxRange Power_Cooldown_Reduction_Percent_All { get; set; }
		public MinMaxRange Experience_Bonus { get; set; }
		public MinMaxRange Hitpoints_Max_Percent_Bonus_Item { get; set; }
		public MinMaxRange CrowdControl_Reduction { get; set; }
		public MinMaxRange Attribute_Set_Item_Discount { get; set; }
		public MinMaxRange Damage_Dealt_Percent_Bonus_Fire { get; set; }
		public MinMaxRange Damage_Dealt_Percent_Bonus_Cold { get; set; }
		public MinMaxRange Damage_Dealt_Percent_Bonus_Poison { get; set; }
		public MinMaxRange Damage_Dealt_Percent_Bonus_Holy { get; set; }
		public MinMaxRange Damage_Dealt_Percent_Bonus_Physical { get; set; }
		public MinMaxRange Damage_Dealt_Percent_Bonus_Lightning { get; set; }
		public MinMaxRange Damage_Dealt_Percent_Bonus_Arcane { get; set; }
		public MinMaxRange Damage_Weapon_Percent_All { get; set; }
		public MinMaxRange Damage_Percent_Bonus_Vs_Elites { get; set; }
		public MinMaxRange Damage_Percent_Reduction_From_Elites { get; set; }
		public MinMaxRange Gold_Find { get; set; }
	}

	[Serializable]
	public class ItemDetailsGems
	{
		public GemItem item { get; set; }
		public bool isGem { get; set; }
		public bool isJewel { get; set; }
		public int jewelRank { get; set; }
		public int jewelSecondaryEffectUnlockRank { get; set; }
		public string jewelSecondaryEffect { get; set; }
		public GemAttributes attributes { get; set; }
		public GemAttributesRaw attributesRaw { get; set; }
	}

	[Serializable]
	public class GemItem
	{
		public string id { get; set; }
		public string name { get; set; }
		public string icon { get; set; }
		public string displayColor { get; set; }
		public string tooltipParams { get; set; }
	}

	[Serializable]
	public class GemAttributes
	{
		public List<GemAttribute> primary { get; set; }
		public List<GemAttribute> secondary { get; set; }
		public List<GemAttribute> passive { get; set; }
	}

	[Serializable]
	public class GemAttributesRaw
	{
		public MinMaxRange Power_Cooldown_Reduction_Percent_All { get; set; }
		public MinMaxRange Vitality_Item { get; set; }
		public MinMaxRange Intelligence_Item { get; set; }
		public MinMaxRange Strength_Item { get; set; }
		public MinMaxRange Dexterity_Item { get; set; }
		public MinMaxRange Resistance_All { get; set; }
		public MinMaxRange Crit_Damage_Percent { get; set; }
		public MinMaxRange Hitpoints_Max_Percent_Bonus_Item { get; set; }
		public MinMaxRange Damage_Percent_Bonus_Vs_Elites { get; set; }
		public MinMaxRange Damage_Percent_Reduction_From_Elites { get; set; }
		public MinMaxRange Gold_Find { get; set; }
	}

	[Serializable]
	public class GemAttribute
	{
		public string text { get; set; }
		public string color { get; set; }
		public string affixType { get; set; }
	}

	[Serializable]
	public class ItemDetailsSet
	{
		public string name { get; set; }
		public List<ItemDetailsSetItem> items { get; set; }
		public string slug { get; set; }
		public List<Ranks> ranks { get; set; }
	}

	[Serializable]
	public class ItemDetailsSetItem
	{
		public string id { get; set; }
		public string name { get; set; }
		public string icon { get; set; }
		public string displayColor { get; set; }
		public string tooltipParams { get; set; }
	}

	[Serializable]
	public class Ranks
	{
		public int required { get; set; }
		public ItemDetailsAttributes attributes { get; set; }
	}

}
