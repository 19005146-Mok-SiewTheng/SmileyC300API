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
    [Route("api/Sensor")]
    public class SensorAPIController : Controller
    {
        private AppDbContext _dbContext;

        public SensorAPIController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAllSensors()
        {
            List<Sensor> model = _dbContext.Sensor.ToList();

            return Ok(model);
        }

        [HttpGet("{num}")]
        public IActionResult GetSensor(int id)
        {
            Sensor model = _dbContext.Sensor.Where(w => w.SensorId == id).FirstOrDefault();

            return Ok(model);
        }

        [HttpPost("Create")]
        public IActionResult Create(Sensor sensor)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Sensor.Add(sensor);

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
        public IActionResult Update(Sensor sensor)
        {
            if (ModelState.IsValid)
            {
                Sensor tOrder = _dbContext.Sensor.Where(mo => mo.SensorId == sensor.SensorId)
                                     .FirstOrDefault();

                if (tOrder != null)
                {
                    tOrder.SensorId = sensor.SensorId;
                    tOrder.AccessPointId = sensor.AccessPointId;
                    tOrder.SensorName = sensor.SensorName;

                    if (_dbContext.SaveChanges() == 1)
                        return Ok(1);
                    else
                        return Ok(0);
                }
                else
                {
                    TempData["Msg"] = "Sensor not found!";
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
            DbSet<Sensor> dbs = _dbContext.Sensor;

            Sensor tOrder = dbs.Where(mo => mo.SensorId == id).FirstOrDefault();

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
                TempData["Msg"] = "Sensor not found!";
                return Ok(-1);
            }
        }

    }
}
