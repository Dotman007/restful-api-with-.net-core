using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingRestFulAPI.Models
{
    public class AccountCategory
    {
        [Key]
        public Guid AccountCategoryId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
