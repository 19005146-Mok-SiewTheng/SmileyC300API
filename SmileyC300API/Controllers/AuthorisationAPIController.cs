using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmileyC300API.Models;
using System.Dynamic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace SmileyC300API.Controllers
{
    [Route("api/Authorisation")]
    public class AuthorisationAPIController : Controller
    {
        private AppDbContext _dbContext;

        public AuthorisationAPIController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAllAuthorisations()
        {
            List<Authorisation> model = _dbContext.Authorisation.ToList();

            return Ok(model);
        }

        [HttpGet("{accessId}/{userId}")]
        public IActionResult GetAuthorisation(int accessId, string userId)
        {
            Authorisation model = _dbContext.Authorisation
                .Where(w => w.AccessPointId == accessId)
                .Where(w => w.UserId.Equals(userId))
                .FirstOrDefault();

            return Ok(model);
        }

        [HttpPost("Create")]
        public IActionResult Create(Authorisation authorisation)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Authorisation.Add(authorisation);

                if (_dbContext.SaveChanges() == 1)
                    return Ok(1);
                else
                    return Ok(0);
            }
            else
            {
                TempData["Msg"] = "Invalid information entered";
                return Ok(-1);
            }
        }

        [HttpPost("Update")]
        public IActionResult Update(Authorisation authorisation)
        {
            if (ModelState.IsValid)
            {
                Authorisation tOrder = _dbContext.Authorisation.Where(mo => mo.AccessPointId == authorisation.AccessPointId)
                                     .FirstOrDefault();

                if (tOrder != null)
                {
                    tOrder.AccessPointId = authorisation.AccessPointId;
                    tOrder.StartDate = authorisation.StartDate;
                    tOrder.EndDate = authorisation.EndDate;
                    tOrder.UserId = authorisation.UserId;

                    if (_dbContext.SaveChanges() == 1)
                        return Ok(1);
                    else
                        return Ok(0);
                }
                else
                {
                    TempData["Msg"] = "Authorisation not found!";
                    return Ok(-1);
                }
            }

            else
            {
                TempData["Msg"] = "Invalid information entered";
                return Ok(-2);
            }
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(int id, string username)
        {
            DbSet<Authorisation> dbs = _dbContext.Authorisation;

            Authorisation tOrder = dbs.Where(mo => mo.AccessPointId == id && mo.UserId == username).FirstOrDefault();

            if (tOrder != null)
            {
                dbs.Remove(tOrder);

                if (_dbContext.SaveChanges() == 1)
                    return Ok(1);
                else
                    return Ok(0);
            }
            else
            {
                return Ok(-1);
            }
        }

    }
}
