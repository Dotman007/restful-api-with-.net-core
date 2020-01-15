using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingRestFulAPI.Dtos
{
    public class GetAccountDetailDto
    {
        public string AccountNo { get; set; }

        public decimal Amount { get; set; }

        public string DestinationAccount { get; set; }
    }
}
