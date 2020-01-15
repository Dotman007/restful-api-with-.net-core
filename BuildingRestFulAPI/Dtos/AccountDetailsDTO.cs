using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingRestFulAPI.Dtos
{
    public class AccountDetailsDTO
    {

        public string AccountName { get; set; }

        public string AccountCategoryName { get; set; }

        public string AccountBalance { get; set; }


        public string BankName { get; set; }



        public Guid? CustomerId { get; set; }

        public string AccountStatus { get; set; }

        public string DateCreated { get; set; }

        public string AccountNo { get; set; }
    }
}
