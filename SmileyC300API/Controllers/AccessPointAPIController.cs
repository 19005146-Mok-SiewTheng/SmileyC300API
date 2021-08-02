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
    [Route("api/AccessPoint")]
    public class AccessPointAPIController : Controller
    {
        private AppDbContext _dbContext;

        public AccessPointAPIController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAllAccessPoints()
        {
            List<AccessPoint> model = _dbContext.AccessPoint.ToList();
       
            return Ok(model);
        }

        [HttpGet("{num}")]
        public IActionResult GetAccessPoint(int num)
        {
            AccessPoint model = _dbContext.AccessPoint.Where(w => w.AccessPointId == num).FirstOrDefault();

            return Ok(model);
        }

        [HttpPost("Create")]
        public IActionResult Create(AccessPoint accessPoint)
        {
            if (ModelState.IsValid)
            {
                _dbContext.AccessPoint.Add(accessPoint);

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

        [HttpPost("Update")]
        public IActionResult Update(AccessPoint accessPoint)
        {
            if (ModelState.IsValid)
            {
                AccessPoint tOrder = _dbContext.AccessPoint.Where(mo => mo.AccessPointId == accessPoint.AccessPointId).FirstOrDefault();

                if (tOrder != null)
                {
                    tOrder.AccessPointId = accessPoint.AccessPointId;
                    tOrder.FacilityId = accessPoint.FacilityId;
                    tOrder.Description = accessPoint.Description;

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

            else
            {
                return Ok(-2);
            }
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {
            DbSet<AccessPoint> dbs = _dbContext.AccessPoint;

            AccessPoint tOrder = dbs.Where(mo => mo.AccessPointId == id).FirstOrDefault();

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
                TempData["Msg"] = "Access Point not found!";
                return Ok(-1);
            }
        }

    }
}
