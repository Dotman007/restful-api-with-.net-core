using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingRestFulAPI.Models
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }

        public DateTime TransactionDate { get; set; }

        public string SourceAccountNo { get; set; }

        public string SourceAccountName { get; set; }

        public string DestinationAccountNo { get; set; }

        public string DestinationAccountName { get; set; }

        [Column(TypeName ="decimal")]
        public decimal Amount { get; set; }
        [Column(TypeName = "decimal")]
        public decimal Charge { get; set; }
        [Column(TypeName = "decimal")]
        public decimal TotalAmount { get; set; }

        public string TransactionReference { get; set; }

        public string TransactionStatus { get; set; }

        public bool IsSuccessful { get; set; }

        public bool IsFalied { get; set; }

        public string SourceAccountType { get; set; }
        public string DestinationAccountType { get; set; }

        public string SourceBankName { get; set; }
        public string DestinationBankName{ get; set; }

        public Guid? SenderCustomerId { get; set; }
        public Guid? RecieverCustomerId { get; set; }
    }
}
