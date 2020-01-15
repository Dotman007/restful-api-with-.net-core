using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingRestFulAPI.Dtos
{
    public class CustomerIdDto
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
        public string FirstName { get; set; }
    }


    public class AgentIdDto
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
        public string FirstName { get; set; }
    }
}
