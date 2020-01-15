using BuildingRestFulAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingRestFulAPI.Dtos
{
    public class CreateAccountDTO
    {
        public string AccountCategoryName { get; set; }

        public string BankName { get; set; }

    }
}
