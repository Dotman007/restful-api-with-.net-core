using System;
using System.Collections.Generic;
using System.Linq;
using BuildingRestFulAPI.DAL;
using BuildingRestFulAPI.Dtos;
using BuildingRestFulAPI.Hubs;
using BuildingRestFulAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace BuildingRestFulAPI.Services
{
    public class AccountTransactionService : IAccountTransaction
    {
        private readonly StoreContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IHubContext<TransactionHub> _hubContext;
        public AccountTransactionService(StoreContext context, IHttpContextAccessor httpContextAccessor,IHubContext<TransactionHub> hubContext)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _hubContext = hubContext;
        }

        public int DailyCreditTransaction(Guid customerId)
        {
            var transaction = _context.Transactions.Where(c => c.RecieverCustomerId == customerId && c.TransactionDate.Day == DateTime.Now.Day).Count();
            return transaction;
        }

        public int DailyDebitTransaction(Guid customerId)
        {
            var transaction = _context.Transactions.Where(c => c.SenderCustomerId == customerId && c.TransactionDate.Day == DateTime.Now.Day).Count();
            return transaction;
        }

        public GetAccountDetailsResponse GetAccountDetailsAsync(GetAccountDetailDto transfer, Guid customerId)
        {
            try
            {
                var sourceAccount = _context.Accounts.Where(c => c.AccountNo == transfer.AccountNo).SingleOrDefault();
                if (sourceAccount == null)
                {
                    return new GetAccountDetailsResponse
                    {
                        ResponseMessage = "Invalid Source Account or Source Account not found"
                    };
                }
                var destinationAccount = _context.Accounts.Where(c => c.AccountNo == transfer.DestinationAccount).SingleOrDefault();
                if (destinationAccount == null)
                {
                    return new GetAccountDetailsResponse
                    {
                        ResponseMessage = "Invalid Destination Account or Destination Account not found"
                    };
                }
                var checkIfAccountNumberBelongsToCustomer = _context.Accounts.Where(c => c.CustomerId == customerId).Select(c => c.AccountNo).ToList();
                var verifyContain = checkIfAccountNumberBelongsToCustomer.Contains(transfer.AccountNo);
                if (!verifyContain)
                {
                    return new GetAccountDetailsResponse
                    {
                        ResponseMessage = "The account number you provided is not vaild, or the account number is not your account number"
                    };
                }
                var transactionDetails = _context.Accounts.Where(c => c.AccountNo == transfer.AccountNo).Select(d => new GetAccountDetailsResponse
                {
                    SourceAccount = sourceAccount.AccountNo,
                    SourceAccountName = sourceAccount.AccountName,
                    SourceAccountType = _context.Accounts.Where(c => c.AccountCategoryId == sourceAccount.AccountCategoryId).Select(c => c.AccountCategory.Name).FirstOrDefault(),
                    DestinationAccount = destinationAccount.AccountNo,
                    DestinationAccountName = destinationAccount.AccountName,
                    DestinationAccountType = _context.Accounts.Where(c => c.AccountCategoryId == destinationAccount.AccountCategoryId).Select(c => c.AccountCategory.Name).FirstOrDefault(),
                    Amount = transfer.Amount,
                    ResponseMessage = "Account Retrieved Successfully",
                    DestinationBankName = _context.Accounts.Where(c => c.AccountNo == destinationAccount.AccountNo).Select(c => c.Bank.BankName).FirstOrDefault(),
                    SourceBankName = _context.Accounts.Where(c => c.AccountNo == sourceAccount.AccountNo).Select(c => c.Bank.BankName).FirstOrDefault(),
                }).SingleOrDefault();
                return transactionDetails;
            }
            catch (Exception ex)
            {
                return new GetAccountDetailsResponse
                {
                    ResponseMessage = ex.Message
                };
                throw ex;
            }
            
        }

        public List<TransactionDetails> GetAllCreditTransactionsAsync(Guid customerId)
        {
            var getSourceAccount = new List<TransactionDetails>();
            var getTransactionDetails = new List<TransactionDetails>();
            var getAccountNumber = _context.Accounts.Where(c => c.CustomerId == customerId).Select(c => c.AccountNo).ToList();
            foreach (var account in getAccountNumber)
            {
                getSourceAccount = _context.Transactions.Where(c => c.DestinationAccountNo == account).Select(c => new TransactionDetails
                {
                    SourceAccount = c.SourceAccountNo,
                    SourceAccountName = c.SourceAccountName,
                    DestinationAccount = c.DestinationAccountNo,
                    DestinationAccountName = c.DestinationAccountName,
                    Amount = c.Amount,
                    DestinationAccountType = c.DestinationAccountType,
                    SourceAccountType = c.SourceAccountType,
                    TransactionDate = c.TransactionDate,
                    Charge = c.Charge,
                    TotalAmount = c.TotalAmount,
                    TransactionRef = c.TransactionReference,
                    TransactionStatus = c.TransactionStatus,
                    SourceBankName = c.SourceBankName,
                    DestinationBankName = c.DestinationBankName
                }).ToList();
                getTransactionDetails.AddRange(getSourceAccount);
            }
            return getTransactionDetails;
        }

        public List<TransactionDetails> GetAllDebitTransactionsAsync(Guid customerId)
        {
            var getSourceAccount = new List<TransactionDetails>();
            var getTransactionDetails = new List<TransactionDetails>();
            var getAccountNumber = _context.Accounts.Where(c => c.CustomerId == customerId).Select(c => c.AccountNo).ToList();
            foreach (var account in getAccountNumber)
            {
                getSourceAccount = _context.Transactions.Where(c => c.SourceAccountNo == account).Select(c => new TransactionDetails
                {
                    SourceAccount = c.SourceAccountNo,
                    SourceAccountName = c.SourceAccountName,
                    DestinationAccount = c.DestinationAccountNo,
                    DestinationAccountName = c.DestinationAccountName,
                    Amount = c.Amount,
                    DestinationAccountType = c.DestinationAccountType,
                    SourceAccountType = c.SourceAccountType,
                    TransactionDate = c.TransactionDate,
                    Charge = c.Charge,
                    TotalAmount = c.TotalAmount,
                    TransactionRef = c.TransactionReference,
                    TransactionStatus = c.TransactionStatus,
                    SourceBankName = c.SourceBankName,
                    DestinationBankName = c.DestinationBankName
                }).ToList();
                getTransactionDetails.AddRange(getSourceAccount);
            }
            return getTransactionDetails;
        }

        public List<TransactionDetails> GetAllTransactionsAsync(Guid customerId)
        {
            
            var getTransactions = _context.Transactions.Where(c => c.SenderCustomerId == customerId).Select(c=> new TransactionDetails
            {
                SourceAccount = c.SourceAccountNo,
                SourceAccountName = c.SourceAccountName,
                DestinationAccount = c.DestinationAccountNo,
                DestinationAccountName = c.DestinationAccountName,
                Amount = c.Amount,
                DestinationAccountType = c.DestinationAccountType,
                SourceAccountType = c.SourceAccountType,
                TransactionDate = c.TransactionDate,
                Charge = c.Charge,
                TotalAmount = c.TotalAmount,
                TransactionRef = c.TransactionReference,
                TransactionStatus = c.TransactionStatus,
                SourceBankName  = c.SourceBankName,
                DestinationBankName = c.DestinationBankName
            }).OrderByDescending(c=>c.TransactionDate).ToList();
            if (getTransactions == null)
            {
                return null;
            }
            return getTransactions;
        }

        public string GetCustomerPin(string accountNo, string custpin)
        {
            var accountPin = _context.AccountTransactions.Where(c => c.AccountNo == accountNo).Select(c => c.Pin).ToList();
            var customerPin = "";
            foreach (var pin in accountPin)
            {
                if (pin  == custpin)
                {
                    customerPin = pin;
                }
                else
                {
                    return "Invalid Pin Supplied";
                }
            }
            return customerPin;
        }

        public string GetDailyCredit(Guid customerId)
        {
            //_transaction.FetchTransaction(customerId);
            var transaction = _context.Transactions.Where(c => c.RecieverCustomerId == customerId && c.TransactionDate.Day == DateTime.Now.Day).Select(c => c.TotalAmount).ToList().Sum();
            return transaction.ToString("c");
        }

        public string GetDailyDebit(Guid customerId)
        {
            var transaction = _context.Transactions.Where(c => c.SenderCustomerId == customerId && c.TransactionDate.Day == DateTime.Now.Day).Select(c => c.TotalAmount).ToList().Sum();
            return transaction.ToString("c");
        }

        public string GetMonthlyCredit(Guid customerId)
        {
            var transaction = _context.Transactions.Where(c => c.RecieverCustomerId == customerId && c.TransactionDate.Month == DateTime.Now.Month).Select(c => c.TotalAmount).ToList().Sum();
            return transaction.ToString("c");
        }

        public string GetMonthlyDebit(Guid customerId)
        {
            var transaction = _context.Transactions.Where(c => c.SenderCustomerId == customerId && c.TransactionDate.Month == DateTime.Now.Month).Select(c => c.TotalAmount).ToList().Sum();
            return transaction.ToString("c");
        }

        public string GetYearlyCredit(Guid customerId)
        {
            var transaction = _context.Transactions.Where(c => c.RecieverCustomerId == customerId && c.TransactionDate.Year == DateTime.Now.Year).Select(c => c.TotalAmount).ToList().Sum();
            return transaction.ToString("c");
        }

        public string GetYearlyDebit(Guid customerId)
        {
            var transaction = _context.Transactions.Where(c => c.SenderCustomerId == customerId && c.TransactionDate.Year == DateTime.Now.Year).Select(c => c.TotalAmount).ToList().Sum();
            return transaction.ToString("c");
        }

        public RestrictionResponse RestrictAccess(Guid customerId, string accountNo)
        {
            var getAccountNo = _context.Accounts.Where(c => c.CustomerId == customerId).SingleOrDefault();
            if (getAccountNo.AccountNo == accountNo)
            {
                return new RestrictionResponse
                {
                    Response = "Account Retrieved Successfully"
                };
            }
            else
            {
                return new RestrictionResponse
                {
                    Response = "Account does not exist"
                };
            }
        }

        public string SetUpAccount(AccountTransactionDto account)
        {
            try
            {
                var getAccount = _context.Accounts.Where(c => c.AccountNo == account.AccountNo).SingleOrDefault();
                var accounts = new AccountTransaction
                {
                    AccountNo = getAccount.AccountNo,
                    AccountCategoryId = getAccount.AccountCategoryId,
                    AccountName = getAccount.AccountName,
                    Balance = getAccount.AccountBalance,
                    Pin = account.Pin
                };
                _context.AccountTransactions.Add(accounts);
                _context.SaveChangesAsync();
                return "Account Setup was successful";
            }
            catch (Exception ex)
            {
                return ex.Message;
                throw ex;
            }
            
        }

        public string TransferMoney(AccountTransferDto transfer)
        {
            try
            {
                var account = _context.AccountTransactions.Where(c => c.AccountNo == transfer.AccountNo).SingleOrDefault();
                var accountss = _context.Accounts.Where(c => c.AccountNo == transfer.AccountNo).SingleOrDefault();
                var destAccount = _context.Accounts.Where(c => c.AccountNo == transfer.DestinationAccount).SingleOrDefault();
                var sourceAccountName = _context.Accounts.Where(c => c.AccountNo == transfer.AccountNo).Select(c => c.Bank.BankName).FirstOrDefault();
                var destinationAccountName = _context.Accounts.Where(c => c.AccountNo == transfer.DestinationAccount).Select(c => c.Bank.BankName).FirstOrDefault();
                
                if (transfer.Amount > account.Balance)
                {
                    return "Insufficient Funds";
                }
                if (transfer.Pin != account.Pin)
                {
                    return "Invalid Pin code";
                }
                if (sourceAccountName != destinationAccountName)
                {
                    if (account.AccountNo == transfer.AccountNo && destAccount.AccountNo == transfer.DestinationAccount && transfer.Pin == account.Pin)
                    {
                        account.Balance -= transfer.Amount;
                        accountss.AccountBalance -= transfer.Amount;
                        destAccount.AccountBalance += transfer.Amount;
                        decimal transferCharge = 65.0M;
                        var intertransaction = new Transaction
                        {
                            TransactionDate = DateTime.Now,
                            Amount = transfer.Amount,
                            DestinationAccountName = destAccount.AccountName,
                            SourceAccountNo = transfer.AccountNo,
                            SourceAccountName = account.AccountName,
                            DestinationAccountNo = destAccount.AccountNo,
                            DestinationAccountType = _context.AccountCategories.Where(c => c.AccountCategoryId == destAccount.AccountCategoryId).Select(c => c.Name).SingleOrDefault(),
                            SourceAccountType = _context.AccountCategories.Where(c => c.AccountCategoryId == account.AccountCategoryId).Select(c => c.Name).SingleOrDefault(),
                            TotalAmount = transfer.Amount + transferCharge,
                            Charge = transferCharge,
                            TransactionReference = Guid.NewGuid().ToString(),
                            TransactionStatus = "Successful",
                            IsSuccessful = true,
                            IsFalied = false,
                            SenderCustomerId = _context.Accounts.Where(c=>c.AccountNo == transfer.AccountNo).Select(c=>c.CustomerId).SingleOrDefault(),
                            RecieverCustomerId = _context.Accounts.Where(c=>c.AccountNo == transfer.DestinationAccount).Select(c=>c.CustomerId).SingleOrDefault(),
                            SourceBankName = _context.Accounts.Where(c => c.AccountNo == account.AccountNo).Select(c => c.Bank.BankName).FirstOrDefault(),
                            DestinationBankName = _context.Accounts.Where(c => c.AccountNo == destAccount.AccountNo).Select(c => c.Bank.BankName).FirstOrDefault()
                        };
                        _context.Transactions.Add(intertransaction);
                    }
                }
                if (sourceAccountName == destinationAccountName)
                {
                    if (account.AccountNo == transfer.AccountNo && destAccount.AccountNo == transfer.DestinationAccount && transfer.Pin == account.Pin)
                    {
                        account.Balance -= transfer.Amount;
                        accountss.AccountBalance -= transfer.Amount;
                        destAccount.AccountBalance += transfer.Amount;
                        var intratransactions = new Transaction
                        {
                            TransactionDate = DateTime.Now,
                            Amount = transfer.Amount,
                            DestinationAccountName = destAccount.AccountName,
                            SourceAccountNo = transfer.AccountNo,
                            SourceAccountName = account.AccountName,
                            DestinationAccountNo = destAccount.AccountNo,
                            DestinationAccountType = _context.AccountCategories.Where(c => c.AccountCategoryId == destAccount.AccountCategoryId).Select(c => c.Name).SingleOrDefault(),
                            SourceAccountType = _context.AccountCategories.Where(c => c.AccountCategoryId == account.AccountCategoryId).Select(c => c.Name).SingleOrDefault(),
                            TotalAmount = transfer.Amount,
                            TransactionReference = Guid.NewGuid().ToString(),
                            TransactionStatus = "Successful",
                            IsSuccessful = true,
                            IsFalied = false,
                            SenderCustomerId = _context.Accounts.Where(c => c.AccountNo == transfer.AccountNo).Select(c => c.CustomerId).SingleOrDefault(),
                            RecieverCustomerId = _context.Accounts.Where(c => c.AccountNo == transfer.DestinationAccount).Select(c => c.CustomerId).SingleOrDefault(),
                            SourceBankName = _context.Accounts.Where(c => c.AccountNo == account.AccountNo).Select(c => c.Bank.BankName).FirstOrDefault(),
                            DestinationBankName = _context.Accounts.Where(c => c.AccountNo == destAccount.AccountNo).Select(c => c.Bank.BankName).FirstOrDefault()
                        };
                        _context.Transactions.Add(intratransactions);
                    }
                }
                _context.SaveChanges();
                return $"{transfer.Amount.ToString("c")} was successfully transferred to {transfer.DestinationAccount}; Thank you for banking with us";
            }
            catch (Exception ex)
            {
                return ex.Message;
                throw ex;
            }
        }
    }
}
