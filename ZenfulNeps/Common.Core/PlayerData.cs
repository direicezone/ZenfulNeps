using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZenfulNeps.Common.Core
{
    public class PlayerData
    {
        public int Rank { get; set; }
        public string Player { get; set; }
        public string Type { get; set; }
        public string Team { get; set; }
        public int ByeWeek { get; set; }
        public double ADP { get; set; }
        public bool Taken { get; set; }
    }
}