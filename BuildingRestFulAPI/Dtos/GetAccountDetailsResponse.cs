using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingRestFulAPI.Dtos
{
    public class GetAccountDetailsResponse
    {
        public string SourceAccount { get; set; }

        public string SourceAccountName { get; set; }

        public decimal Amount { get; set; }

        public string DestinationAccount { get; set; }

        public string DestinationAccountName { get; set; }

        public string SourceAccountType { get; set; }

        public string DestinationAccountType { get; set; }

        public string ResponseMessage { get; set; }

        public string DestinationBankName { get; set; }

        public string SourceBankName { get; set; }
    }


    public class TransactionDetails
    {
        public string SourceAccount { get; set; }

        public string SourceAccountName { get; set; }

        public decimal Amount { get; set; }

        public string DestinationAccount { get; set; }

        public string DestinationAccountName { get; set; }

        public string SourceAccountType { get; set; }

        public string DestinationAccountType { get; set; }

        public DateTime TransactionDate { get; set; }

        public decimal Charge { get; set; }
        public decimal TotalAmount { get; set; }

        public string TransactionRef { get; set; }
        public string TransactionStatus { get; set; }

        public string SourceBankName { get; set; }
        public string DestinationBankName { get; set; }
    }
}
