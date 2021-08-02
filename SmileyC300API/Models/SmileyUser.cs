using System;
using System.Collections.Generic;

namespace SmileyC300API.Models
{
    public partial class SmileyUser
    {
        public SmileyUser()
        {
            AccessLog = new HashSet<AccessLog>();
            Authorisation = new HashSet<Authorisation>();
            Facility = new HashSet<Facility>();
        }

        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public byte[] Password { get; set; }
        public string Voicefile { get; set; }
        public string Picfile { get; set; }
        public string Role { get; set; }
        public string PhoneNum { get; set; }
        public DateTime? LastLogin { get; set; }

        public virtual ICollection<AccessLog> AccessLog { get; set; }
        public virtual ICollection<Authorisation> Authorisation { get; set; }
        public virtual ICollection<Facility> Facility { get; set; }
    }
}
