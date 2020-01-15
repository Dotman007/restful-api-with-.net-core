using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuildingRestFulAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BuildingRestFulAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountCategorysController : ControllerBase
    {
        private readonly IAccountCategory _category;
        public AccountCategorysController(IAccountCategory category)
        {
            _category = category;
        }

        [Route("CreateAccountCategoryAsync")]
        [HttpPost]
        public async Task<IActionResult> CreateAccountCategoryAsync(CreateAccountCategoryDTO create)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _category.CreateAccountCategory(create);
            if (result.Equals("Account Category Added Successfully"))
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