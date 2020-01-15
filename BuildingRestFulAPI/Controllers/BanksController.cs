using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuildingRestFulAPI.Dtos;
using BuildingRestFulAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BuildingRestFulAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BanksController : ControllerBase
    {
        private readonly IBank _bank;
        public BanksController(IBank bank)
        {
            _bank = bank;
        }
        [Authorize(AuthenticationSchemes = "Basic")]
        [Route("CreateBankAsync")]
        [HttpPost]
        public async Task<IActionResult> CreateBankAsync([FromBody]CreateBankDTO create)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _bank.CreateBank(create);
            if (result.Equals("Bank Created Successfully"))
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}