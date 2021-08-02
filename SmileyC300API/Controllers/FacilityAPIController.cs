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
    [Route("api/Facility")]
    public class FacilityAPIController : Controller
    {
        private AppDbContext _dbContext;

        public FacilityAPIController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAllFacilities()
        {
            List<Facility> model = _dbContext.Facility.ToList();

            return Ok(model);
        }

        [HttpGet("{id}")]
        public IActionResult GetFacility(int id)
        {
            Facility model = _dbContext.Facility.Where(w => w.FacilityId == id).FirstOrDefault();

            return Ok(model);
        }

        [HttpPost("Create")]
        public IActionResult Create(Facility facility)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Facility.Add(facility);

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
        public IActionResult Update(Facility facility)
        {
            if (ModelState.IsValid)
            {
                Facility tOrder = _dbContext.Facility.Where(mo => mo.FacilityId == facility.FacilityId)
                                     .FirstOrDefault();

                if (tOrder != null)
                {
                    tOrder.FacilityId = facility.FacilityId;
                    tOrder.AdminId = facility.AdminId;
                    tOrder.FacilityName = facility.FacilityName;
                    tOrder.PostalCode = facility.PostalCode;
                    tOrder.BlockNumber = facility.BlockNumber;
                    tOrder.StreetName = facility.StreetName;
                    tOrder.BannerPic = facility.BannerPic;

                    if (_dbContext.SaveChanges() == 1)
                        return Ok(1);
                    else
                        return Ok(0);
                }
                else
                {
                    TempData["Msg"] = "Facility not found!";
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
        public IActionResult Delete(int id)
        {
            DbSet<Facility> dbs = _dbContext.Facility;

            Facility tOrder = dbs.Where(mo => mo.FacilityId == id).FirstOrDefault();

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
