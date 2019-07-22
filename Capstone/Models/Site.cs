using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Site
    {
        public int SiteID { get; set; }
        public int CampgroundID { get; set; }
        public int SiteNumber { get; set; }
        public int Occupancy { get; set; }
        public bool WCAccessible { get; set; }
        public int MaxRvLength { get; set; }
        public bool Utilities { get; set; }
    }
}
