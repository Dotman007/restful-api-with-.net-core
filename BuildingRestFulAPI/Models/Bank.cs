using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingRestFulAPI.Models
{
    public class Bank
    {
        [Key]
        public Guid BankId { get; set; }

        [Required]
        [MaxLength(15)]
        public string BankName { get; set; }

        [Required, MaxLength(5)]
        public string SortCode { get; set; }

        [Required,MaxLength(3)]
        public string AccountNumberPrefix { get; set; }

        public string AgentName { get; set; }


        public Guid? CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
