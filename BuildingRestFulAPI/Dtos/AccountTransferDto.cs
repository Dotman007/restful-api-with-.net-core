using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingRestFulAPI.Dtos
{
    public class AccountTransferDto
    {
        public string AccountNo { get; set; }

        public decimal Amount { get; set; }

        public decimal Charge { get; set; }

        public string DestinationAccount { get; set; }

        public string Pin { get; set; }
    }
}
