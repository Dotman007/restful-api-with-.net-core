using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BuildingRestFulAPI.DAL;
using BuildingRestFulAPI.Dtos;
using BuildingRestFulAPI.Models;
using BuildingRestFulAPI.ResponseMessages;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BuildingRestFulAPI.Services
{
    public class AgentService : IAgent
    {
        private readonly StoreContext _context;
        internal DbSet<Customer> dbSet;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AgentService(StoreContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            this.dbSet = _context.Set<Customer>();
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AgentLoginSuccessDto> AgentDashboard()
        {
            GenericRepository<Customer> generic = new GenericRepository<Customer>(_context, this.dbSet, _httpContextAccessor);
            var user = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            var customer = await generic.AgentDashboard(user);
            if (customer == null)
            {
                var response = new AgentLoginSuccessDto
                {
                    Message = "Invalid username or password"
                };
                return response;
            }
            return customer;
        }

        public Task<CustomerRegistrationResponse> DeleteCustomerAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> GenerateGuid()
        {
            var generatedGuid = Guid.NewGuid();
            return Task.FromResult(generatedGuid);
        }

        public Task<List<AgentDto>> GetCustomerAsync()
        {
            throw new NotImplementedException();
        }

        public Task<AgentDto> GetCustomerBeforeAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<AgentDto> GetCustomerByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<AgentIdDto> GetCustomerSensitiveInfo()
        {
            throw new NotImplementedException();
        }

        public Task<AgentLoginSuccessDto> AgentLoginAsync(LoginDto login)
        {
            try
            {
                if (string.IsNullOrEmpty(login.Username))
                {
                    var error = new AgentLoginSuccessDto
                    {
                        Message = "username is required"
                    };
                    return Task.FromResult<AgentLoginSuccessDto>(error);
                }
                if (string.IsNullOrEmpty(login.Password))
                {
                    var error = new AgentLoginSuccessDto
                    {
                        Message = "password is required"
                    };
                    return Task.FromResult<AgentLoginSuccessDto>(error);
                }
                else if (string.IsNullOrEmpty(login.Username) && string.IsNullOrEmpty(login.Password))
                {
                    var error = new AgentLoginSuccessDto
                    {
                        Message = "username and password is required"
                    };
                    return Task.FromResult<AgentLoginSuccessDto>(error);
                }
                else
                {
                    var agent = _context.Customers.Where(c => c.AgentUserName == login.Username && c.Password == login.Password && c.IsAgent == true).SingleOrDefault();
                    if (agent == null)
                    {
                        var error = new AgentLoginSuccessDto
                        {
                            Message = "Invalid Username or password"
                        };
                        return Task.FromResult<AgentLoginSuccessDto>(error);
                    }
                    var success = new AgentLoginSuccessDto
                    {
                        Firstname = agent.Firstname,
                        Lastname = agent.Lastname,
                        Gender = agent.Gender,
                        Dob = agent.Dob,
                        Email = agent.Email,
                        Fax = agent.Fax,
                        MainAddressId = agent.MainAddressId,
                        NewsLetterOpted = agent.NewsLetterOpted,
                        Telephone = agent.Telephone,
                        Password = agent.Password,
                        CustomerId = agent.Id,
                        AgentBank = agent.AgentBank,
                        AgentName = agent.AgentName,
                        AgentUserName = agent.AgentUserName,
                        Message = "Login Successful"
                    };
                    return Task.FromResult<AgentLoginSuccessDto>(success);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<CustomerRegistrationResponse> RegisterAgentAsync(AgentDto agent)
        {
            try
            {
                var customer = new Customer
                {
                    Id = GenerateGuid().Result,
                    MainAddressId = GenerateGuid().Result,
                    Firstname = agent.Firstname,
                    Lastname = agent.Lastname,
                    AgentBank = agent.AgentBank,
                    AgentName = agent.Firstname + " " + agent.Lastname,
                    AgentUserName = agent.AgentUserName,
                    Dob = agent.Dob,
                    IsAgent = true,
                    Email = agent.Email,
                    Password = agent.Password,
                    Fax = agent.Fax,
                    Gender = agent.Gender,
                    IsCustomer = false,
                    NewsLetterOpted = false,
                    CustomerUserName = null,
                    Telephone = agent.Telephone
                };
                GenericRepository<Customer> generic = new GenericRepository<Customer>(_context, this.dbSet, _httpContextAccessor);
                if (_context.Customers.Any(c => c.Email == customer.Email || c.AgentBank == customer.AgentBank || c.AgentUserName == customer.AgentUserName))
                {
                    var responseEmailExist = new CustomerRegistrationResponse
                    {

                        Response = "The Credentials supplied already exist",

                    };
                    return responseEmailExist;
                }
                var responseSuccess = new CustomerRegistrationResponse
                {
                    Response = "Agent Details successfully added",
                };
                await generic.PostCustomer(customer);
                await _context.SaveChangesAsync();
                return responseSuccess;
            }
            catch (Exception ex)
            {
                var responseException = new CustomerRegistrationResponse
                {
                    Response = ex.Message,
                };
                return responseException;
                throw ex;
            }
        }

        public Task<CustomerRegistrationResponse> UpdateCustomerAsync(Guid id, AgentDto customer)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetNumberOfCustomers(Guid agentId)
        {
            int count = 0;
            var getCustomers = _context.Customers.Where(c => c.IsCustomer == true).ToList();
            foreach (var item in getCustomers)
            {
                var getCustomerBanks = _context.Accounts.Where(c => c.CustomerId == item.Id).Select(c => c.Bank.BankName).ToList();
                foreach (var customers in getCustomerBanks)
                {
                    var getAgentBankName = _context.Customers.Where(c => c.IsAgent && c.Id == agentId).Select(c => c.AgentBank).SingleOrDefault();
                    if (customers == getAgentBankName)
                    {
                        count += 1;
                    }
                }
            }
            return Task.FromResult<int>(count);
        }

        public Task<int> TotalNumberOfSavingsAccount(Guid agentId)
        {
            var count = 0;
            var getCustomers = _context.Customers.Where(c => c.IsCustomer).ToList();
            foreach (var customers in getCustomers)
            {
                var getAgentBankName = _context.Customers.Where(c => c.Id == agentId).Select(c => c.AgentBank).SingleOrDefault();
                var getCustomerBankName = _context.Accounts.Where(c => c.CustomerId == customers.Id).SingleOrDefault();
                var getBankName = _context.Accounts.Where(c => c.CustomerId == customers.Id).Select(c => c.Bank.BankName).SingleOrDefault();
                var getCustomerAccountType = _context.Accounts.Where(c => c.CustomerId == customers.Id).Select(c => c.AccountCategory.Name).SingleOrDefault();
                if (getAgentBankName.Equals(getBankName) && getCustomerAccountType == "Savings Account")
                {
                    count += 1;
                }

            }
            return Task.FromResult<int>(count);
        }

        public Task<int> TotalNumberOfCurrentAccount(Guid agentId)
        {
            var count = 0;
            var getCustomers = _context.Customers.Where(c => c.IsCustomer).ToList();
            foreach (var customers in getCustomers)
            {
                var getAgentBankName = _context.Customers.Where(c => c.Id == agentId).Select(c => c.AgentBank).SingleOrDefault();
                var getCustomerBankName = _context.Accounts.Where(c => c.CustomerId == customers.Id).SingleOrDefault();
                var getBankName = _context.Accounts.Where(c => c.CustomerId == customers.Id).Select(c => c.Bank.BankName).SingleOrDefault();
                var getCustomerAccountType = _context.Accounts.Where(c => c.CustomerId == customers.Id).Select(c => c.AccountCategory.Name).SingleOrDefault();
                if (getAgentBankName.Equals(getBankName) && getCustomerAccountType == "Current Account")
                {
                    count += 1;
                }

            }
            return Task.FromResult<int>(count);
        }

        public Task<int> TotalNumberOfFixedDepositAccount(Guid agentId)
        {
            var count = 0;
            var getCustomers = _context.Customers.Where(c => c.IsCustomer).ToList();
            foreach (var customers in getCustomers)
            {
                var getAgentBankName = _context.Customers.Where(c => c.Id == agentId).Select(c => c.AgentBank).SingleOrDefault();
                var getCustomerBankName = _context.Accounts.Where(c => c.CustomerId == customers.Id).SingleOrDefault();
                var getBankName = _context.Accounts.Where(c => c.CustomerId == customers.Id).Select(c => c.Bank.BankName).SingleOrDefault();
                var getCustomerAccountType = _context.Accounts.Where(c => c.CustomerId == customers.Id).Select(c => c.AccountCategory.Name).SingleOrDefault();
                if (getAgentBankName.Equals(getBankName) && getCustomerAccountType == "Fixed Deposit Account")
                {
                    count += 1;
                }

            }
            return Task.FromResult<int>(count);
        }

        public Task<int> TotalNumberOfOtherAccount(Guid agentId)
        {
            var count = 0;
            var getCustomers = _context.Customers.Where(c => c.IsCustomer).ToList();
            foreach (var customers in getCustomers)
            {
                var getAgentBankName = _context.Customers.Where(c => c.Id == agentId).Select(c => c.AgentBank).SingleOrDefault();
                var getCustomerBankName = _context.Accounts.Where(c => c.CustomerId == customers.Id).SingleOrDefault();
                var getBankName = _context.Accounts.Where(c => c.CustomerId == customers.Id).Select(c => c.Bank.BankName).SingleOrDefault();
                var getCustomerAccountType = _context.Accounts.Where(c => c.CustomerId == customers.Id).Select(c => c.AccountCategory.Name).SingleOrDefault();
                if (getAgentBankName.Equals(getBankName) &&
                    (getCustomerAccountType != "Savings Account"
                    && getCustomerAccountType != "Current Account"
                    && getCustomerAccountType != "Fixed Deposit Account"))
                {
                    count += 1;
                }
            }
            return Task.FromResult<int>(count);
        }

        public Task<int> TotalNumberOfActiveAccount(Guid agentId)
        {
            throw new NotImplementedException();
        }

        public Task<int> TotalNumberOfBlockedAccount(Guid agentId)
        {
            var count = 0;
            var getCustomers = _context.Customers.Where(c => c.IsCustomer).ToList();
            foreach (var customers in getCustomers)
            {
                var getAgentBankName = _context.Customers.Where(c => c.Id == agentId).Select(c => c.AgentBank).SingleOrDefault();
                var getCustomerBankName = _context.Accounts.Where(c => c.CustomerId == customers.Id).SingleOrDefault();
                var getBankName = _context.Accounts.Where(c => c.CustomerId == customers.Id).Select(c => c.Bank.BankName).SingleOrDefault();
                var getCustomerAccountType = _context.Accounts.Where(c => c.CustomerId == customers.Id).Select(c => c.AccountCategory.Name).SingleOrDefault();
                if (getAgentBankName.Equals(getBankName) && getCustomerBankName.AccountStatus == "Blocked")
                {
                    count += 1;
                }

            }
            return Task.FromResult<int>(count);
        }


        public Task<int> TotalNumberOfActivatedAccount(Guid agentId)
        {
            var count = 0;
            var getCustomers = _context.Customers.Where(c => c.IsCustomer).ToList();
            foreach (var customers in getCustomers)
            {
                var getAgentBankName = _context.Customers.Where(c => c.Id == agentId).Select(c => c.AgentBank).SingleOrDefault();
                var getCustomerBankName = _context.Accounts.Where(c => c.CustomerId == customers.Id).SingleOrDefault();
                var getBankName = _context.Accounts.Where(c => c.CustomerId == customers.Id).Select(c => c.Bank.BankName).SingleOrDefault();
                var getCustomerAccountType = _context.Accounts.Where(c => c.CustomerId == customers.Id).Select(c => c.AccountCategory.Name).SingleOrDefault();
                if (getAgentBankName.Equals(getBankName) && getCustomerBankName.AccountStatus == "Active")
                {
                    count += 1;
                }
            }
            return Task.FromResult<int>(count);
        }

        public Task<List<AccountDto>> GetCustomersWithSavingsAccount(Guid agentId)
        {
            var getCustomerWithSavingsAccount = new List<AccountDto>();
            var getCustomerWithSavingAccount = new List<AccountDto>();
            var getCustomers = _context.Customers.Where(c => c.IsCustomer).ToList();
            var getAgentBank = _context.Customers.Where(a => a.Id == agentId).Select(c => c.AgentBank).SingleOrDefault();
            foreach (var customers in getCustomers)
            {
                getCustomerWithSavingsAccount = _context.Accounts
                    .Where
                    (s => s.CustomerId == customers.Id && s.AccountCategory.Name == "Savings Account" && s.Bank.BankName == getAgentBank)
                    .Select(s => new AccountDto
                    {
                         AccountId = s.AccountId,
                         BankName = s.Bank.BankName,
                         AccountBalance = s.AccountBalance.ToString("c"),
                         AccountStatus = s.AccountStatus,
                         AccountCategoryName = s.AccountCategory.Name,
                         AccountName = s.AccountName,
                         AccountNo = s.AccountNo,
                         CustomerId = s.CustomerId,
                         DateCreated = s.DateCreated
                    }).ToList();
                getCustomerWithSavingAccount.AddRange(getCustomerWithSavingsAccount);
            }
            return Task.FromResult<List<AccountDto>>(getCustomerWithSavingAccount);
        }

        public Task<List<AccountDto>> GetCustomersWithCurrentAccount(Guid agentId)
        {
            var getCustomerWithCurrentsAccount = new List<AccountDto>();
            var getCustomerWithCurrentAccount = new List<AccountDto>();
            var getAllCustomers = _context.Customers.Where(c => c.IsCustomer);
            var getAgentBank = _context.Customers.Where(c => c.Id == agentId).Select(c => c.AgentBank).SingleOrDefault();
            foreach (var item in getAllCustomers)
            {
                getCustomerWithCurrentsAccount = _context.Accounts
                    .Where(c => c.CustomerId == item.Id && c.Bank.BankName == getAgentBank && c.AccountCategory.Name == "Current Account").Select(s => new AccountDto
                    {
                        AccountId = s.AccountId,
                        BankName = s.Bank.BankName,
                        AccountBalance = s.AccountBalance.ToString("c"),
                        AccountStatus = s.AccountStatus,
                        AccountCategoryName = s.AccountCategory.Name,
                        AccountName = s.AccountName,
                        AccountNo = s.AccountNo,
                        CustomerId = s.CustomerId,
                        DateCreated = s.DateCreated
                    }).ToList();
                getCustomerWithCurrentAccount.AddRange(getCustomerWithCurrentsAccount);
            }
            return Task.FromResult<List<AccountDto>>(getCustomerWithCurrentAccount);
        }

        public Task<List<AccountDto>> GetCustomersWithFixedDepositAccount(Guid agentId)
        {
            var getfixedDepositsAccount = new List<AccountDto>();
            var getfixedDepositAccount = new List<AccountDto>();
            var getAgentBank = _context.Customers.Where(c => c.Id == agentId).Select(c => c.AgentBank).SingleOrDefault();
            var getCustomers = _context.Customers.ToList();
            foreach (var customer in getCustomers)
            {
                getfixedDepositsAccount = _context.Accounts
                    .Where(f => f.CustomerId == customer.Id && f.Bank.BankName == getAgentBank && f.AccountCategory.Name == "Fixed Deposit Account").Select(s => new AccountDto
                    {
                        AccountId = s.AccountId,
                        BankName = s.Bank.BankName,
                        AccountBalance = s.AccountBalance.ToString("c"),
                        AccountStatus = s.AccountStatus,
                        AccountCategoryName = s.AccountCategory.Name,
                        AccountName = s.AccountName,
                        AccountNo = s.AccountNo,
                        CustomerId = s.CustomerId,
                        DateCreated = s.DateCreated
                    }).ToList();
                getfixedDepositAccount.AddRange(getfixedDepositAccount);
            }
            return Task.FromResult<List<AccountDto>>(getfixedDepositAccount);
        }

        public Task<List<AccountDto>> GetCustomersWithOtherAccount(Guid agentId)
        {
            var getOthersAccount = new List<AccountDto>();
            var getOtherAccount = new List<AccountDto>();
            var getAgentBank = _context.Customers.Where(c => c.Id == agentId).Select(c => c.AgentBank).SingleOrDefault();
            var getCustomers = _context.Customers.ToList();
            foreach (var customer in getCustomers)
            {
                getOthersAccount = _context.Accounts
                     .Where(a => a.CustomerId == customer.Id && a.Bank.BankName == getAgentBank
                     && (a.AccountCategory.Name != "Savings Account"
                     && a.AccountCategory.Name == "Current Account"
                     && a.AccountCategory.Name == "Fixed Deposit Account")).Select(s => new AccountDto
                     {
                         AccountId = s.AccountId,
                         BankName = s.Bank.BankName,
                         AccountBalance = s.AccountBalance.ToString("c"),
                         AccountStatus = s.AccountStatus,
                         AccountCategoryName = s.AccountCategory.Name,
                         AccountName = s.AccountName,
                         AccountNo = s.AccountNo,
                         CustomerId = s.CustomerId,
                         DateCreated = s.DateCreated
                     }).ToList();
                getOtherAccount.AddRange(getOthersAccount);
            }
            return Task.FromResult<List<AccountDto>>(getOtherAccount);
        }

        public Task<List<TransactionDto>> GetTodayTransaction(Guid agentId)
        {
            var getCustomerTransactions = new List<TransactionDto>();
            var getCustomersTransactions = new List<TransactionDto>();
            var getAgentBankName = _context.Customers.Where(c => c.Id == agentId).Select(c => c.AgentBank).SingleOrDefault();
            var customers = _context.Customers.ToList();
            foreach (var customer in customers)
            {
                var getCustomerAccount = _context.Accounts.Where(c => c.CustomerId == customer.Id).SingleOrDefault();
                getCustomerTransactions = _context.Transactions
                    .Where(c => c.SenderCustomerId == getCustomerAccount.CustomerId && getAgentBankName == getCustomerAccount.Bank.BankName && c.TransactionDate.Day == DateTime.Now.Day)
                    .Select(c => new TransactionDto
                    {
                        
                            SourceAccountName = c.SourceAccountName,
                            SourceAccountNo = c.SourceAccountNo,
                            DestinationAccountName = c.DestinationAccountName,
                            DestinationAccountNo = c.DestinationAccountNo,
                            SourceAccountType = c.SourceAccountType,
                            DestinationAccountType = c.DestinationAccountType,
                            SourceBankName = c.SourceBankName,
                            DestinationBankName = c.DestinationBankName,
                            TransactionId = c.TransactionId,
                            TransactionReference = c.TransactionReference,
                            TransactionStatus = c.TransactionStatus,
                            TransactionDate = c.TransactionDate,
                            Amount = c.Amount.ToString("c"),
                            Charge = c.Charge.ToString("c"),
                            TotalAmount = c.TotalAmount.ToString("c"),
                        
                    }).ToList();
                getCustomersTransactions.AddRange(getCustomersTransactions);
            }
            return Task.FromResult<List<TransactionDto>>(getCustomersTransactions);
        }

        public Task<List<TransactionDto>> GetWeekTransaction(Guid agentId)
        {
            var getCustomerTransactions = new List<TransactionDto>();
            var getCustomersTransactions = new List<TransactionDto>();
            var getAgentBankName = _context.Customers.Where(c => c.Id == agentId).Select(c => c.AgentBank).SingleOrDefault();
            var customers = _context.Customers.ToList();
            foreach (var customer in customers)
            {
                var weekday = GetWeekDay();
                var getCustomerAccount = _context.Accounts.Where(c => c.CustomerId == customer.Id).SingleOrDefault();
                getCustomerTransactions = _context.Transactions
                    .Where(c => c.SenderCustomerId == getCustomerAccount.CustomerId && getAgentBankName == getCustomerAccount.Bank.BankName
                    && c.TransactionDate.Day <= GetWeekDay().Day)
                    .Select(c => new TransactionDto
                    {
                        SourceAccountName = c.SourceAccountName,
                        SourceAccountNo = c.SourceAccountNo,
                        DestinationAccountName = c.DestinationAccountName,
                        DestinationAccountNo = c.DestinationAccountNo,
                        SourceAccountType = c.SourceAccountType,
                        DestinationAccountType = c.DestinationAccountType,
                        SourceBankName = c.SourceBankName,
                        DestinationBankName = c.DestinationBankName,
                        TransactionId = c.TransactionId,
                        TransactionReference = c.TransactionReference,
                        TransactionStatus = c.TransactionStatus,
                        TransactionDate = c.TransactionDate,
                        Amount = c.Amount.ToString("c"),
                        Charge = c.Charge.ToString("c"),
                        TotalAmount = c.TotalAmount.ToString("c"),

                    }).ToList();
                getCustomersTransactions.AddRange(getCustomersTransactions);
            }
            return Task.FromResult<List<TransactionDto>>(getCustomersTransactions);
        }

        public Task<List<TransactionDto>> GetMonthTransaction(Guid agentId)
        {
            var getCustomerTransactions = new List<TransactionDto>();
            var getCustomersTransactions = new List<TransactionDto>();
            var getAgentBankName = _context.Customers.Where(c => c.Id == agentId).Select(c => c.AgentBank).SingleOrDefault();
            var customers = _context.Customers.ToList();
            foreach (var customer in customers)
            {
                var getCustomerAccount = _context.Accounts.Where(c => c.CustomerId == customer.Id).SingleOrDefault();
                getCustomerTransactions = _context.Transactions
                    .Where(c => c.SenderCustomerId == getCustomerAccount.CustomerId && getAgentBankName == getCustomerAccount.Bank.BankName && c.TransactionDate.Month == DateTime.Now.Month)
                    .Select(c => new TransactionDto
                    {

                        SourceAccountName = c.SourceAccountName,
                        SourceAccountNo = c.SourceAccountNo,
                        DestinationAccountName = c.DestinationAccountName,
                        DestinationAccountNo = c.DestinationAccountNo,
                        SourceAccountType = c.SourceAccountType,
                        DestinationAccountType = c.DestinationAccountType,
                        SourceBankName = c.SourceBankName,
                        DestinationBankName = c.DestinationBankName,
                        TransactionId = c.TransactionId,
                        TransactionReference = c.TransactionReference,
                        TransactionStatus = c.TransactionStatus,
                        TransactionDate = c.TransactionDate,
                        Amount = c.Amount.ToString("c"),
                        Charge = c.Charge.ToString("c"),
                        TotalAmount = c.TotalAmount.ToString("c"),

                    }).ToList();
                getCustomersTransactions.AddRange(getCustomersTransactions);
            }
            return Task.FromResult<List<TransactionDto>>(getCustomersTransactions);
        }

        public Task<List<TransactionDto>> GetYearTransaction(Guid agentId)
        {
            var getCustomerTransactions = new List<TransactionDto>();
            var getCustomersTransactions = new List<TransactionDto>();
            var getAgentBankName = _context.Customers.Where(c => c.Id == agentId).Select(c => c.AgentBank).SingleOrDefault();
            var customers = _context.Customers.ToList();
            foreach (var customer in customers)
            {
                var getCustomerAccount = _context.Accounts.Where(c => c.CustomerId == customer.Id).SingleOrDefault();
                getCustomerTransactions = _context.Transactions
                    .Where(c => c.SenderCustomerId == getCustomerAccount.CustomerId && getAgentBankName == getCustomerAccount.Bank.BankName && c.TransactionDate.Year == DateTime.Now.Year)
                    .Select(c => new TransactionDto
                    {

                        SourceAccountName = c.SourceAccountName,
                        SourceAccountNo = c.SourceAccountNo,
                        DestinationAccountName = c.DestinationAccountName,
                        DestinationAccountNo = c.DestinationAccountNo,
                        SourceAccountType = c.SourceAccountType,
                        DestinationAccountType = c.DestinationAccountType,
                        SourceBankName = c.SourceBankName,
                        DestinationBankName = c.DestinationBankName,
                        TransactionId = c.TransactionId,
                        TransactionReference = c.TransactionReference,
                        TransactionStatus = c.TransactionStatus,
                        TransactionDate = c.TransactionDate,
                        Amount = c.Amount.ToString("c"),
                        Charge = c.Charge.ToString("c"),
                        TotalAmount = c.TotalAmount.ToString("c"),

                    }).ToList();
                getCustomersTransactions.AddRange(getCustomersTransactions);
            }
            return Task.FromResult<List<TransactionDto>>(getCustomersTransactions);
        }

        public DateTime GetWeekDay()
        {
            var TodaysDate = DateTime.Now;
            return TodaysDate;
        }
    }
}