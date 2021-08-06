using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Data;
using System.Security.Claims;
using SmileyC300API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;



namespace SmileyC300API.Controllers
{
    [Route("api/User")]
    public class UserAPIController : Controller
    {
        private const string AUTHSCHEME = "UserSecurity";
        private const string LOGIN_SQL =
           @"SELECT * FROM Smiley_User 
            WHERE user_id = '{0}' 
              AND Password = HASHBYTES('SHA1', '{1}')";

        private const string LASTLOGIN_SQL =
           @"UPDATE Smiley_User SET last_login=GETDATE() 
                        WHERE user_id='{0}'";

        private const string ROLE_COL = "role";
        private const string NAME_COL = "full_name";

        private AppDbContext _dbContext;

        public UserAPIController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var model = _dbContext.SmileyUser
                .Select(s => new
                {
                    s.UserId,
                    s.FullName,
                    s.Email,
                    s.PhoneNum,
                    s.Picfile,
                    s.Role
                })
                .ToList();

            return Ok(model);
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(string id)
        {
            var userProfile = _dbContext.SmileyUser
                     .Where(w => w.UserId.Equals(id))
                     .Select(s => new
                     {
                         s.UserId,
                         s.FullName,
                         s.Email,
                         s.PhoneNum,
                         s.Picfile,
                         s.Role
                     })
                 .FirstOrDefault();

            return Ok(userProfile);
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginUser user)
        {
            if (!AuthenticateUser(user.UserId, user.Password, out ClaimsPrincipal principal))
            {
                return Ok(0);
            }
            else
            {
                HttpContext.SignInAsync(
                   AUTHSCHEME,
                   principal,
               new AuthenticationProperties
               {
                   IsPersistent = false
               });

                int num_affected = _dbContext.Database.ExecuteSqlInterpolated($"UPDATE Smiley_User SET last_login=GETDATE() WHERE user_id = {user.UserId}");

                var userProfile = _dbContext.SmileyUser
                    .Where(w => w.UserId.Equals(user.UserId))
                    .Select(s => new
                    {
                        s.UserId,
                        s.FullName,
                        s.Email,
                        s.PhoneNum,
                        s.Picfile,
                        s.Role
                    })
                .FirstOrDefault();

                return Ok(userProfile);
            }
        }

        [HttpPost("Create")]
        public IActionResult Create(SmileyUser user)
        {
            if (UniqueUsername(user.UserId))
            {
                _dbContext.SmileyUser.Add(user);

                if (_dbContext.SaveChanges() == 1)
                    return Ok(1);
                else
                    return Ok(0);
            }
            else {
                return Ok(-1);
            }
            
        }



        private bool AuthenticateUser(string uid, string pw, out ClaimsPrincipal principal)
        {

            DbSet<SmileyUser> dbs = _dbContext.SmileyUser;
            var pw_bytes = System.Text.Encoding.ASCII.GetBytes(pw);

            SmileyUser smileyUser = dbs.FromSqlInterpolated($"SELECT * FROM Smiley_User WHERE user_id = {uid} AND password = HASHBYTES('SHA1', {pw_bytes})").FirstOrDefault();

            principal = null;

            if (smileyUser != null)
            {
                principal =
                   new ClaimsPrincipal(
                      new ClaimsIdentity(
                         new Claim[] {
                        new Claim(ClaimTypes.NameIdentifier, smileyUser.UserId),
                        new Claim(ClaimTypes.Name, smileyUser.FullName),
                        new Claim(ClaimTypes.Role, smileyUser.Role)
                         }, "Basic"
                      )
                   );
                return true;
            }
            return false;
        }

        [HttpPost("ChangePassword")]
        public IActionResult ChangePassword(PasswordUpdate pu)
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var sql = String.Format($"UPDATE Smiley_User SET password = HASHBYTES('SHA1', '{pu.NewPassword}') WHERE user_id='{userid}' AND password = HASHBYTES('SHA1', '{pu.CurrentPassword}')");

            if (_dbContext.Database.ExecuteSqlCommand(sql) == 1)
                return Ok(1);
            else
                return Ok(0);
        }

        [HttpGet("VerifyCurrPassword/{smileyUserId}/{currentPassword}")]
        public JsonResult VerifyCurrentPassword(string smileyUserId, string currentPassword)
        {
            DbSet<SmileyUser> dbs = _dbContext.SmileyUser;
            var userid = smileyUserId;
            var pw_bytes = System.Text.Encoding.ASCII.GetBytes(currentPassword);

            SmileyUser user = dbs.FromSqlInterpolated($"SELECT * FROM Smiley_User WHERE user_id = {userid} AND password = HASHBYTES('SHA1', {pw_bytes})").FirstOrDefault();

            if (user != null)
                return Json(true);
            else
                return Json(false);
        }

        [HttpGet("VerifyNewPassword/{smileyUserId}/{newPassword}")]
        public JsonResult VerifyNewPassword(string smileyUserId, string newPassword)
        {
            DbSet<SmileyUser> dbs = _dbContext.SmileyUser;
            var userid = smileyUserId;
            var pw_bytes = System.Text.Encoding.ASCII.GetBytes(newPassword);

            SmileyUser user = dbs.FromSqlInterpolated($"SELECT * FROM Smiley_User WHERE user_id = {userid} AND password = HASHBYTES('SHA1', {pw_bytes})").FirstOrDefault();
            if (user == null)
                return Json(true);
            else
                return Json(false);
        }

        
        private bool UniqueUsername(string userId)
        {
            var userProfile = _dbContext.SmileyUser.Where(w => w.UserId.Equals(userId)).FirstOrDefault();

            if (userProfile == null)
            {
                return true;
            }
            return false;
        }

    }
}
