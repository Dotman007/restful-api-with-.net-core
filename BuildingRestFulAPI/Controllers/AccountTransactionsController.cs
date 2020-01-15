using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BuildingRestFulAPI.Dtos;
using BuildingRestFulAPI.Hubs;
using BuildingRestFulAPI.Services;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BuildingRestFulAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountTransactionsController : ControllerBase
    {
        private readonly IAccountTransaction _account;
        private IHubContext<TransactionHub> _hubContext;
        public AccountTransactionsController(IHubContext<TransactionHub> hubContext,IAccountTransaction account)
        {
            _hubContext = hubContext;
            _account = account;
        }
        // GET: api/AccountTransactions
        [Route("SetUpAccountAsync")]
        [HttpPost]

        public IActionResult SetUpAccount([FromBody] AccountTransactionDto account)
        {
            var result = _account.SetUpAccount(account);
            if (!result.Equals("Account Setup was successful"))
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [Route("TransferAsync")]
        [HttpPost]
        public IActionResult TransferAsync(AccountTransferDto account)
        {
            //BackgroundJob.Enqueue(() => _account.TransferMoney(account));
            //_hubContext.Clients.All.SendAsync("getAllDebitTransaction");
            var result = _account.TransferMoney(account);
            if (result == $"{account.Amount.ToString("c")} was successfully transferred to {account.DestinationAccount}; Thank you for banking with us")
            {
                return Ok(result);
            }
            return BadRequest(result);

        }

        [Route("GetCustomerPin")]
        [HttpPost]
        public IActionResult GetCustomerPin(string accountNo, string custpin)
        {
            //RecurringJob.AddOrUpdate(()=>_account.GetCustomerPin(accountNo, custpin), Cron.Minutely);
            var result = _account.GetCustomerPin(accountNo, custpin);
            if (result == $"Invalid Pin Supplied")
            {
                return BadRequest(result);
            }
            return Ok(result);
        }


        [Route("RestrictAccess")]
        [HttpPost]
        public IActionResult RestrictAccess(Guid customerId, string accountNo)
        {
            var result = _account.RestrictAccess(customerId,accountNo);
            if (result.Response == "Account Retrieved Successfully")
            {
                return Ok(result.Response);
            }
            return BadRequest(result.Response);

        }




        [Route("GetAccountDetailsAsync")]
        [HttpPost]
        public IActionResult GetAccountDetailsAsync([FromBody]GetAccountDetailDto account, Guid customerId)
        {
            var result = _account.GetAccountDetailsAsync(account,customerId);
            if (result.ResponseMessage.Equals("Account Retrieved Successfully"))
            {
                return Ok(result);
            }
            return BadRequest(result.ResponseMessage);
        }


        [Route("GetMonthlyCredit")]
        [HttpGet]
        public IActionResult GetMonthlyCredit(Guid customerId)
        {
            RecurringJob.AddOrUpdate(() => _account.GetMonthlyCredit(customerId), Cron.Minutely);
            var result = _account.GetMonthlyCredit(customerId);
            return Ok(result);
        }


        [Route("GetMonthlyDebit")]
        [HttpGet]
        public IActionResult GetMonthlyDebit(Guid customerId)
        {
            RecurringJob.AddOrUpdate(() => _account.GetMonthlyDebit(customerId), Cron.Minutely);
            var result = _account.GetMonthlyDebit(customerId);
            return Ok(result);
        }

        [Route("GetAllCreditTransactionsAsync")]
        [HttpGet]
        public IActionResult GetAllCreditTransactionsAsync(Guid customerId)
        {
            //RecurringJob.AddOrUpdate(() => _account.GetAllCreditTransactionsAsync(customerId),Cron.Never);
            var result = _account.GetAllCreditTransactionsAsync(customerId);
            if (result == null)
            {
                return NoContent();
            }
            return Ok(result);
        }


        
        //var result = _account.GetAllDebitTransactionsAsync(customerId);
        //    if (result == null)
        //    {
        //        return NoContent();
        //    }
        //    return Ok(result);
        //}


        [Route("GetAllDebitTransactionsAsync")]
        [HttpGet]
        public IActionResult GetAllDebitTransactionsAsync(Guid customerId)
        {
            var result = _account.GetAllDebitTransactionsAsync(customerId);
            if (result == null)
            {
                return NoContent();
            }
            return Ok(result);
        }


        [Route("GetDailyCredit")]
        [HttpGet]
        public IActionResult GetDailyCredit(Guid customerId)
        {
            var result = _account.GetDailyCredit(customerId);
            _hubContext.Clients.All.SendAsync("getAllDebitTransaction",result);
            return Ok(result);
        }
        [Route("GetDailyDebit")]
        [HttpGet]
        public IActionResult GetDailyDebit(Guid customerId)
        {
            var result = _account.GetDailyDebit(customerId);
            return Ok(result);
        }
        [Route("GetYearlyCredit")]
        [HttpGet]
        public IActionResult GetYearlyCredit(Guid customerId)
        {
            var result = _account.GetYearlyCredit(customerId);
            return Ok(result);
        }
        [Route("GetYearlyDebit")]
        [HttpGet]
        public IActionResult GetYearlyDebit(Guid customerId)
        {
            var result = _account.GetYearlyDebit(customerId);
            return Ok(result);
        }
        [Route("DailyCreditTransaction")]
        [HttpGet]
        public IActionResult DailyCreditTransaction(Guid customerId)
        {
            var result = _account.DailyCreditTransaction(customerId);
            return Ok(result);
        }
        [Route("DailyDebitTransaction")]
        [HttpGet]
        public IActionResult DailyDebitTransaction(Guid customerId)
        {
            var result = _account.DailyDebitTransaction(customerId);
            return Ok(result);
        }

        [Route("GetAllTransactionsAsync")]
        [HttpGet]
        public IActionResult GetAllTransactionsAsync(Guid customerId)
        {
            var result = _account.GetAllTransactionsAsync(customerId);
            if (result == null)
            {
                return NoContent();
            }
            return Ok(result);
        }
    }
}
