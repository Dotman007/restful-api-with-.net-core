using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingRestFulAPI.Dtos
{
    public class AccountDto
    {
        public Guid AccountId { get; set; }

        public string AccountName { get; set; }

        public string AccountCategoryName { get; set; }

        public string AccountBalance { get; set; }

        public string BankName { get; set; }

        public Guid? CustomerId { get; set; }

        public string AccountStatus { get; set; }

        public DateTime DateCreated { get; set; }

        public string AccountNo { get; set; }

    }
}
