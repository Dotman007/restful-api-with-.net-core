using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingRestFulAPI.Models
{
    public class Account
    {
        [Key]
        public Guid AccountId { get; set; }

        //[Required,MaxLength(50)]
        public string AccountName { get; set; }

        public Guid? AccountCategoryId { get; set; }
        public virtual AccountCategory AccountCategory { get; set; }

        [Column(TypeName ="decimal")]
        public decimal AccountBalance { get; set; }


        public Guid? BankId { get; set; }
        public virtual Bank Bank { get; set; }
        public bool IsActive { get; set; }

        public Guid? CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public string AccountStatus { get; set; }

        public DateTime DateCreated { get; set; }

        public string AccountNo { get; set; }


    }
}
