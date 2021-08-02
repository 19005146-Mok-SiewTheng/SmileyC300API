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
    [Route("api/SensorData")]
    public class SensorDataAPIController : Controller
    {
        private AppDbContext _dbContext;

        public SensorDataAPIController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAllSensorData()
        {
            List<SensorData> model = _dbContext.SensorData.ToList();

            return Ok(model);
        }

        [HttpGet("{num}")]
        public IActionResult GetSensorData(int num)
        {
            SensorData model = _dbContext.SensorData.Where(w => w.Sno == num).FirstOrDefault();

            return Ok(model);
        }

    }
}
