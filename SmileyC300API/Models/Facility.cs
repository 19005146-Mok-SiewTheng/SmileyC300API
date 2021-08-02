using System;
using System.Collections.Generic;

namespace SmileyC300API.Models
{
    public partial class Facility
    {
        public Facility()
        {
            AccessPoint = new HashSet<AccessPoint>();
        }

        public int FacilityId { get; set; }
        public string AdminId { get; set; }
        public string FacilityName { get; set; }
        public int PostalCode { get; set; }
        public string BlockNumber { get; set; }
        public string StreetName { get; set; }
        public string BannerPic { get; set; }

        public virtual SmileyUser Admin { get; set; }
        public virtual ICollection<AccessPoint> AccessPoint { get; set; }
    }
}
