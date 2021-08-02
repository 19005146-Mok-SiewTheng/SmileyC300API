using System;
using System.Collections.Generic;

namespace SmileyC300API.Models
{
    public partial class Authorisation
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int AccessPointId { get; set; }
        public string UserId { get; set; }

        public virtual AccessPoint AccessPoint { get; set; }
        public virtual SmileyUser User { get; set; }
    }
}
