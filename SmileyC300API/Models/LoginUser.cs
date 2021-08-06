using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SmileyC300API.Models
{
    public class LoginUser
    {
        public string UserId { get; set; }

        public string Password { get; set; }

    }
}
