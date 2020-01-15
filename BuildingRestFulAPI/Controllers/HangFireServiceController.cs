using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuildingRestFulAPI.Services;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BuildingRestFulAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangFireServiceController : ControllerBase
    {
        private IHangFireService _hangfire;
        public HangFireServiceController(IHangFireService hangfire)
        {
            _hangfire = hangfire;
        }
        [Route("GetServices")]
        [HttpGet]
        public IActionResult GetServices()
        {
           var result  = _hangfire.GetService();
           RecurringJob.AddOrUpdate(()=>_hangfire.GetService(),Cron.Minutely);
           return Ok(result);
        }

       
    }
}
