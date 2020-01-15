using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingRestFulAPI.Dtos
{
    public class TransactionDto
    {
        public int TransactionId { get; set; }

        public DateTime TransactionDate { get; set; }

        public string SourceAccountNo { get; set; }

        public string SourceAccountName { get; set; }

        public string DestinationAccountNo { get; set; }

        public string DestinationAccountName { get; set; }

        public string Amount { get; set; }

        public string Charge { get; set; }

        public string TotalAmount { get; set; }

        public string TransactionReference { get; set; }

        public string TransactionStatus { get; set; }

        public DateTime Dates { get; set; }
        public string SourceAccountType { get; set; }
        public string DestinationAccountType { get; set; }

        public string SourceBankName { get; set; }
        public string DestinationBankName { get; set; }

    }
}
