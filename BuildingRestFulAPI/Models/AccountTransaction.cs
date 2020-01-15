using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingRestFulAPI.Models
{
    public class AccountTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountTransactionId { get; set; }

        public string AccountNo { get; set; }

        public string AccountName { get; set; }

        [Column(TypeName = "decimal")]
        public decimal Balance { get; set; }

        public string Pin { get; set; }

        public Guid? AccountCategoryId { get; set; }

        public virtual AccountCategory AccountCategory { get; set; }

    }
}
