using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZenfulNeps.Models
{
	
	public class Recipe
	{
		public string Id { get; set; }
		public string Description { get; set; }
		public string Author { get; set; }
		public List<Ingredient> Ingredients { get; set; }
		public string Instructions { get; set; }
		public string Image { get; set; }
		public string Info { get; set; }
	}

	public class Ingredient
	{
		public string Name { get; set; }
		public string Amount { get; set; }
		public string Unit { get; set; }
		public string Note { get; set; }
        public string AmazonLink { get; set; }
    }
}