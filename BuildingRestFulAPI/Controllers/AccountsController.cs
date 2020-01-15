using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuildingRestFulAPI.DAL;
using BuildingRestFulAPI.Dtos;
using BuildingRestFulAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BuildingRestFulAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccount _account;
        public AccountsController(IAccount account)
        {
            _account = account;
        }
        [Authorize(AuthenticationSchemes = "Basic")]
        [Route("CreateAccountAsync")]
        [HttpPost]
        public async Task<IActionResult> CreateAccountAsync([FromBody] CreateAccountDTO createAccount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _account.CreateAccount(createAccount); 
            if (result.Response.Equals("Account Created Successfully") && result.Status.Equals(true))
            {
                return Ok(result.Response);
            }
            if (result.Response.Equals($"You already have a {createAccount.AccountCategoryName} with {createAccount.BankName}") && result.Status.Equals(false))
            {
                return Ok(result.Response);
            }
            else
            {
                return BadRequest(result.Response);
            }
        }

        [Authorize(AuthenticationSchemes = "Basic")]
        [Route("GetCustomerAccountAsync")]
        [HttpPost]
        public async Task<IActionResult> GetCustomerAccountAsync([FromBody]GetAccountDTO getAccount)
        {
            var result = await _account.GetAccount(getAccount);
            if (result == null)
            {
                return NotFound($"The customer does not have a {getAccount.AccountTypeName} Account");
            }
            else
            {
                return Ok(result);
            }
        }


        //[Authorize(AuthenticationSchemes = "Basic")]
        [Route("AllMyAccountsAsync")]
        [HttpGet]
        public async Task<IActionResult> AllMyAccounts(Guid? getAccount)
        {
            var result = await _account.AllMyAccounts(getAccount);
            if (result == null)
            {
                return NotFound($"No account was found");
            }
            else
            {
                return Ok(result);
            }
        }

        //[Authorize(AuthenticationSchemes = "Basic")]
        [Route("GetAllBanks")]
        [HttpGet]
        public IActionResult GetAllBanks()
        {
            var result = _account.GetAllBankName();
            if (result == null)
            {
                return Ok(null);
            }
            return Ok(result);
        }

        //GetAllBankNameById

        [Route("GetAllBankNameById")]
        [HttpGet]
        public IActionResult GetAllBankNameById(Guid? customerId)
        {
            var result = _account.GetAllBankNameById(customerId);
            if (result == null)
            {
                return Ok(null);
            }
            return Ok(result);
        }

        [Route("GetAllAccountCategory")]
        [HttpGet]
        public IActionResult GetAllAccountCategory()
        {
            var result = _account.GetAllAccountType();
            if (result == null)
            {
                return Ok(null);
            }
            return Ok(result);
        }
    }
}