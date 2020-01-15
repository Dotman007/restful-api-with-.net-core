using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuildingRestFulAPI.Dtos;
using BuildingRestFulAPI.Models;
using BuildingRestFulAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BuildingRestFulAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagementController : ControllerBase
    {
        private readonly IManagement _management;
        public ManagementController(IManagement management)
        {
            _management = management;
        }
        // GET: api/Management
        [Route("Login")]
        [HttpPost]
        public  IActionResult Login([FromBody] LoginInput login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = _management.Login(login);
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes ="Bearer")]
        [Route("Register")]
        [HttpPost]
        public IActionResult CreateUser([FromBody] Management management)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = _management.Register(management);
            return Ok(result);
        }


        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("ManagementDashboardAsync")]
        [HttpPost]
        public async Task<ActionResult<LoginResponseMessage>> Dashboard()
        {
            var result = await _management.Dashboard();
            if (result == null)
            {
                return BadRequest("Invalid username or password");
            }
            return Ok(result);
        }

        //[Authorize(AuthenticationSchemes = "Bearer")]
        //[Route("ManagementRoleNameAsync")]
        //[HttpPost]
        //public async Task<ActionResult<LoginSuccessDto>> ManagementRoleNameAsync(int roleId)
        //{
        //    var result = await _management.ManagementRoleNameAsync(roleId);
        //    if (result == null)
        //    {
        //        return BadRequest("Invalid username or password");
        //    }
        //    return Ok(result);
        //}




    }
}
