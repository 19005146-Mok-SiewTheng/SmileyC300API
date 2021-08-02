using System;
using System.Collections.Generic;

namespace SmileyC300API.Models
{
    public partial class AccessPoint
    {
        public AccessPoint()
        {
            Authorisation = new HashSet<Authorisation>();
            Sensor = new HashSet<Sensor>();
        }

        public int AccessPointId { get; set; }
        public int FacilityId { get; set; }
        public string Description { get; set; }

        public virtual Facility Facility { get; set; }
        public virtual ICollection<Authorisation> Authorisation { get; set; }
        public virtual ICollection<Sensor> Sensor { get; set; }
    }
}
