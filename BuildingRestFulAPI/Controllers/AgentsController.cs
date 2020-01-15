using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuildingRestFulAPI.DAL;
using BuildingRestFulAPI.Dtos;
using BuildingRestFulAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BuildingRestFulAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        private IAgent _agent;
        public AgentsController(IAgent agent)
        {
            _agent = agent;
        }
        [Route("RegisterAgentAsync")]
        [HttpPost]
        //public IActionResult RegisterAgent(AgentDto agent)
        //{
        //    var result = _agent.RegisterAgent(agent);
        //    if (result != "Registration successful")
        //    {
        //        return BadRequest(result);
        //    }
        //    return Ok(result);
        //}
        // GET: api/Agents
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Agents/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Agents
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Agents/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
