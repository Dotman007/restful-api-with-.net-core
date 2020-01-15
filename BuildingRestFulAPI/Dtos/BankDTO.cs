using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingRestFulAPI.Dtos
{
    public class CreateBankDTO
    {

        public string BankName { get; set; }

        public string SortCode { get; set; }

        public string AccountNumberPrefix { get; set; }
    }
}
