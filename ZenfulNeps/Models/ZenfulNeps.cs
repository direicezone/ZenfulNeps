using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZenfulNeps.Models
{
	public class ZenfulNeps
	{
		public List<Rss> RssFeeds { get; set; }
        public int RssCount { get; set; }
	}

	public class Rss
	{
		public string Heading { get; set; }
		public string Link { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
	}

	public class BodyFat
	{
		public string JP7Percent { get; set; }
		public string JP3Percent { get; set; }
		public string PA3Percent { get; set; }
		public string P9Percent { get; set; }
		public string AverageBF { get; set; }
		public string FatWeight { get; set; }
		public string LeanWeight { get; set; }
	}

    public class RawRssInfo
    {
        public string RssLink { get; set; }
        public string RssHeading { get; set; }
        public string RssHost { get; set; }
    }
}