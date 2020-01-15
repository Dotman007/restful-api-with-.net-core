using BuildingRestFulAPI.Dtos;
using BuildingRestFulAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingRestFulAPI.Services
{
    public interface IAccountTransaction
    {
        string SetUpAccount(AccountTransactionDto account);
        string TransferMoney(AccountTransferDto transfer);
        GetAccountDetailsResponse GetAccountDetailsAsync(GetAccountDetailDto transfer, Guid customerId);
        List<TransactionDetails> GetAllTransactionsAsync(Guid customerId);
        List<TransactionDetails> GetAllDebitTransactionsAsync(Guid customerId);
        List<TransactionDetails> GetAllCreditTransactionsAsync(Guid customerId);
        string GetMonthlyCredit(Guid customerId);
        string GetMonthlyDebit(Guid customerId);
        string GetDailyCredit(Guid customerId);
        string GetDailyDebit(Guid customerId);
        string GetYearlyDebit(Guid customerId);
        string GetYearlyCredit(Guid customerId);
        int DailyCreditTransaction(Guid customerId);
        int DailyDebitTransaction(Guid customerId);
        RestrictionResponse RestrictAccess(Guid customerId, string accountNo);
        string GetCustomerPin(string accountNo, string custpin);
    }
}
