using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BuildingRestFulAPI.DAL;
using BuildingRestFulAPI.Models;
using BuildingRestFulAPI.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BuildingRestFulAPI.Dtos;
using Microsoft.AspNetCore.SignalR;
using BuildingRestFulAPI.Hubs;

namespace BuildingRestFulAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private IHubContext<TransactionHub> _hubContext;
        private readonly ICustomer _customer;
        private readonly IAgent _agent;
        public CustomersController(IHubContext<TransactionHub> hubContext,ICustomer customer,IAgent agent)
        {
            _hubContext = hubContext;
            _customer = customer;
            _agent = agent;
        }
        
        // GET: api/Customers
        //[Authorize]
        [Route("GetCustomersAsync")]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<ActionResult<List<Customer>>> GetCustomers()
        {
            var result = await _customer.GetCustomerAsync();
            if (result == null)
            {
                return NoContent();
            }
            return Ok(result);
        }
        [Authorize(AuthenticationSchemes = "Basic")]
        [Route("GetCustomerSensitiveInfo")]
        [HttpGet]
        public async Task<ActionResult> GetCustomerSensitiveInfo()
       {
            var result = await _customer.GetCustomerSensitiveInfo();
            if (result == null)
            {
                return NoContent();
            }
            return Ok(result);
        }
        //[Route("GetNumberOfCustomersAsync")]
        //[HttpGet]
        //public async Task<ActionResult> GetNumberOfCustomers(Guid agentId)
        //{
        //    var result = await _agent.GetNumberOfCustomers(agentId);
        //    return Ok(result);
        //}
        //[Authorize(AuthenticationSchemes = "Basic")]
        [Route("TotalNumberOfSavingsAccountAsync")]
        [HttpGet]
        public async Task<ActionResult> TotalNumberOfSavingsAccountAsync(Guid agentId)
        {
            var result = await _agent.TotalNumberOfSavingsAccount(agentId);
            return Ok(result);
        }


        //[Authorize(AuthenticationSchemes = "Basic")]
        [Route("TotalNumberOfCurrentAccountAsync")]
        [HttpGet]
        public async Task<ActionResult> TotalNumberOfCurrentAccountAsync(Guid agentId)
        {
            var result = await _agent.TotalNumberOfCurrentAccount(agentId);
            return Ok(result);
        }

        [Route("GetCustomersWithSavingsAccountAsync")]
        [HttpGet]
        public async Task<ActionResult> GetCustomersWithSavingsAccountAsync(Guid agentId)
        {
            var result = await _agent.GetCustomersWithSavingsAccount(agentId);
            if (result == null)
            {
                return NoContent();
            }
            return Ok(result);
        }
        [Route("GetCustomersWithCurrentAccountAsync")]
        [HttpGet]
        public async Task<ActionResult> GetCustomersWithCurrentAccountAsync(Guid agentId)
        {
            var result = await _agent.GetCustomersWithCurrentAccount(agentId);
            if (result == null)
            {
                return NoContent();
            }
            return Ok(result);
        }


        [Route("GetWeekTransaction")]
        [HttpGet]
        public async Task<ActionResult> GetWeekTransaction(Guid agentId)
        {
            var result = await _agent.GetWeekTransaction(agentId);
            if (result == null)
            {
                return NoContent();
            }
            return Ok(result);
        }
        [Route("GetCustomersWithFixedDepositAccountAsync")]
        [HttpGet]
        public async Task<ActionResult> GetCustomersWithFixedDepositAccountAsync(Guid agentId)
        {
            var result = await _agent.GetCustomersWithFixedDepositAccount(agentId);
            if (result == null)
            {
                return NoContent();
            }
            return Ok(result);
        }

        [Route("GetCustomersOtherAccountAsync")]
        [HttpGet]
        public async Task<ActionResult> GetCustomersOtherAccountAsync(Guid agentId)
        {
            var result = await _agent.GetCustomersWithOtherAccount(agentId);
            if (result == null)
            {
                return NoContent();
            }
            return Ok(result);
        }
        //[Authorize(AuthenticationSchemes = "Basic")]
        [Route("TotalNumberOfFixedDepositAccountAsync")]
        [HttpGet]
        public async Task<ActionResult> TotalNumberOfFixedDepositAccountAsync(Guid agentId)
        {
            var result = await _agent.TotalNumberOfFixedDepositAccount(agentId);
            return Ok(result);
        }

        [Route("GetCustomersWithOtherAccountAsync")]
        [HttpGet]
        public async Task<ActionResult> GetCustomersWithOtherAccountAsync(Guid agentId)
        {
            var result = await _agent.GetCustomersWithOtherAccount(agentId);
            if (result  == null)
            {
                return NoContent();
            }
            return Ok(result);
        }



        //[Authorize(AuthenticationSchemes = "Basic")]
        [Route("TotalNumberOfOtherAccountAsync")]
        [HttpGet]
        public async Task<ActionResult> TotalNumberOfOtherAccount(Guid agentId)
        {
            var result = await _agent.TotalNumberOfOtherAccount(agentId);
            return Ok(result);
        }

        //[Authorize(AuthenticationSchemes = "Basic")]
        [Route("TotalNumberOfBlockedAccountAsync")]
        [HttpGet]
        public async Task<ActionResult> TotalNumberOfBlockedAccount(Guid agentId)
        {
            var result = await _agent.TotalNumberOfBlockedAccount(agentId);
            return Ok(result);
        }


        //[Authorize(AuthenticationSchemes = "Basic")]
        [Route("TotalNumberOfActivatedAccountAsync")]
        [HttpGet]
        public async Task<ActionResult> TotalNumberOfActivatedAccount(Guid agentId)
        {
            var result = await _agent.TotalNumberOfActivatedAccount(agentId);
            return Ok(result);
        }


        [Route("LoginAsync")]
        [HttpPost]
        public async Task<ActionResult<LoginSuccessDto>> Login([FromBody]LoginDto login)
        {
            var result = await _customer.LoginAsync(login);
            if (result.Message == "Invalid username or password")
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }




        [Route("AgentLoginAsync")]
        [HttpPost]
        public async Task<ActionResult<LoginSuccessDto>> AgentLoginAsync([FromBody]LoginDto login)
        {
            var result = await _agent.AgentLoginAsync(login);
            if (result.Message != "Login Successful")
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }


        [Authorize(AuthenticationSchemes = "Basic")]
        [Route("DashboardAsync")]
        [HttpPost]
        public async Task<ActionResult<LoginSuccessDto>> Dashboard()
        {
            var result = await _customer.Dashboard();
            if (result == null)
            {
                return BadRequest("Invalid username or password");
            }
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = "Basic")]
        [Route("AgentDashboardAsync")]
        [HttpPost]
        public async Task<ActionResult<LoginSuccessDto>> AgentDashboardAsync()
        {
            var result = await _agent.AgentDashboard();
            if (result.Message == "Invalid username or password")
            {
                return BadRequest(result);
            }
            return Ok(result);
        }


        //[Authorize(AuthenticationSchemes = "Basic")]
        [Route("GetNumberOfCustomersAsync")]
        [HttpGet]
        public IActionResult GetNumberOfCustomersAsync(Guid agentId)
        {
            var result = _agent.GetNumberOfCustomers(agentId);
            return Ok(result.Result);
        }


        // GET: api/Customers/5
        [Route("GetCustomerById")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Basic")]
        public async Task<ActionResult<Customer>> GetCustomerById(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var ident = User.Identity as ClaimsIdentity;
            var userId = ident.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId != id.ToString())
            {
                return BadRequest("You are not Authorized");
            }
            var customer = await _customer.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }



        [Route("GetCustomerBeforeAsync")]
        [HttpGet]
        public async Task<ActionResult<Customer>> GetCustomerBeforeAsync(Guid id)
        {
            var result = await _customer.GetCustomerBeforeAsync(id);
            if (result == null)
            {
                return NoContent();
            }
            return Ok(result);
        }

        // PUT: api/Customers/5
        [Route("UpdateCustomerAsync")]
        [HttpPut]
        public async Task<IActionResult> PutCustomer(Guid id, [FromBody]Customer customer)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return BadRequest("Id is required");
            }
            var result = await _customer.UpdateCustomerAsync(id, customer);
            if (result.Response.Equals("Customer Record Updated Successfully") && result.StatusCode.Equals("00"))
            {
                return Ok(result);
            }
            if (result.Response.Equals("Customer Details was not updated") && result.StatusCode.Equals("30"))
            {
                return BadRequest(result);
            }
            if (result.Response.Equals("The customer id is invalid") && result.StatusCode.Equals("33"))
            {
                return BadRequest(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        //POST: api/Customers
        [Route("RegisterCustomer")]
        [HttpPost]
        public async Task<ActionResult<bool>> PostCustomer([FromBody]CustomerDto customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("All required fields are compulsory");
            }
            var result = await _customer.RegisterCustomerAsync(customer);
            if (result.Response.Equals("Customer Details successfully added"))
            {
                return Ok(result);
            }
            if (result.Response.Equals("Customer email already exist"))
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }


        [Route("RegisterAgentAsync")]
        [HttpPost]
        public async Task<ActionResult<bool>> RegisterAgentAsync([FromBody]AgentDto customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _agent.RegisterAgentAsync(customer);
            if (result.Response.Equals("Agent Details successfully added"))
            {
                return Ok("Agent Details successfully added");
            }
            if (result.Response.Equals("The Credentials supplied already exist"))
            {
                return Ok("The Credentials supplied already exist");
            }
            else
            {
                return BadRequest(result);
            }
        }

        //// DELETE: api/Customers/5
        [Route("DeleteCustomer")]
        [HttpDelete]
        public async Task<ActionResult<Customer>> DeleteCustomer(Guid id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return Ok("Id is required");
            }
            var result = await _customer.DeleteCustomerAsync(id);
            if (result.Response.Equals("Customer Details deleted successfully") && result.StatusCode.Equals("00"))
            {
                return Ok($"Customer with Id: {id} was Deleted Successfully.");
            }
            else
            {
                return BadRequest($"Customer with Id {id} was not found.");
            }
        }

        //private bool CustomerExists(Guid id)
        //{
        //    return _context.Customers.Any(e => e.Id == id);
        //}
    }
}
