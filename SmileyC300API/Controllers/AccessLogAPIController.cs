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
    [Route("api/AccessLog")]
    public class AccessLogAPIController : Controller
    {
        private AppDbContext _dbContext;

        public AccessLogAPIController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAllAccessLogs()
        {
            List<AccessLog> logs = _dbContext.AccessLog.ToList();

            return Ok(logs);
        }

        [HttpGet("{num}")]
        public IActionResult GetAccessLog(int num)
        {
            AccessLog log = _dbContext.AccessLog.Where(w => w.Sno == num).FirstOrDefault();

            return Ok(log);
        }

    }
}
