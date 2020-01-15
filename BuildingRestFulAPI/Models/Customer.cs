using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingRestFulAPI.Models
{
    public class Customer
    {
        public Guid Id { get; set; }

        public string Gender { get; set; }

        public string  Firstname { get; set; }

        public string Lastname { get; set; }

        public string CustomerUserName { get; set; }

        public DateTime Dob { get; set; }

        public string Email { get; set; }

        public string AgentName { get; set; }

        public string AgentBank { get; set; }

        public string AgentUserName { get; set; }

        public Guid MainAddressId { get; set; }


        public string Telephone { get; set; }

        public string Fax { get; set; }

        public bool IsAgent { get; set; }


        public bool IsCustomer { get; set; }

        public string Password { get; set; }
        public bool NewsLetterOpted { get; set; }
    }
}
