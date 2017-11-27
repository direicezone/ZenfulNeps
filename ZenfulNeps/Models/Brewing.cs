using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZenfulNeps.Models
{
	public class Brewing
	{
		public List<Grain> Grains { get; set; }
		public List<Color> Colors { get; set; }
	}

	public class Grain
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public decimal Lovibond { get; set; }
		public decimal Ppg { get; set; }
		public bool Mashable { get; set; }
		public string Category { get; set; }
	}

	public class Color
	{
		public decimal SRM { get; set; }
		public string RGB { get; set; }
	}
}