using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingRestFulAPI.Dtos
{
    public class GetAccountDTO
    {
        public Guid CustomerId { get; set; }

        public string AccountTypeName { get; set; }

        public string BankName { get; set; }

    }
}
