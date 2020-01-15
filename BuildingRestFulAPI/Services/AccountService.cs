using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BuildingRestFulAPI.DAL;
using BuildingRestFulAPI.Dtos;
using BuildingRestFulAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BuildingRestFulAPI.Services
{
    public class AccountService : IAccount
    {
        private readonly StoreContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        internal DbSet<Account> _dbSet;
        public AccountService(StoreContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            this._dbSet = _context.Set<Account>();
        }

        public Task<AccountSuccess> ActivateAccount(string AccountNo)
        {
            
            throw new NotImplementedException();
        }

        public async Task<List<AccountDetailsDTO>> AllMyAccounts(Guid? getAccount)
        {
            var myAccounts = await _context.Accounts.Where(c => c.CustomerId == getAccount).Select(c => new AccountDetailsDTO
            {
                CustomerId = c.CustomerId,
                AccountBalance = c.AccountBalance.ToString("c"),
                AccountNo = c.AccountNo,
                AccountName = c.AccountName,
                BankName = c.Bank.BankName,
                AccountCategoryName = c.AccountCategory.Name,
                AccountStatus = c.AccountStatus
            }).ToListAsync();
            return myAccounts;
        }

        public async Task<AccountSuccess> CreateAccount(CreateAccountDTO createAccount)
        {
            try
            {
                var user = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
                var account = new Account
                {
                    AccountId = GenerateId().Result,
                    AccountBalance = 2000.00M,
                    AccountCategoryId = _context.AccountCategories.Where(c=>c.Name == createAccount.AccountCategoryName).Select(c=>c.AccountCategoryId).SingleOrDefault(),
                    AccountName = GetAccountName().Result,
                    AccountStatus = "Blocked",
                    BankId = _context.Banks.Where(b=>b.BankName == createAccount.BankName).Select(b=>b.BankId).SingleOrDefault(),
                    CustomerId = GetCustomerId().Result,
                    DateCreated = DateTime.Now,
                    IsActive = false,
                    AccountNo = GenerateAccountNo(GetCustomerId().Result, createAccount.BankName)

                };
                var getAccountCategoryId = await _context.AccountCategories.Where(c => c.Name == createAccount.AccountCategoryName).Select(c => c.AccountCategoryId).SingleOrDefaultAsync();
                var getBankId = await _context.Banks.Where(c => c.BankName == createAccount.BankName).Select(c => c.BankId).SingleOrDefaultAsync();
                var checkAccountDuplicate = _context.Accounts.Where(c => c.CustomerId == account.CustomerId && c.BankId == getBankId && c.AccountCategory.AccountCategoryId == getAccountCategoryId).Count();
                if (checkAccountDuplicate >= 1)
                {
                    var duplicateResponse = new AccountSuccess
                    {
                        Response = $"You already have a {createAccount.AccountCategoryName} with {createAccount.BankName}",
                        Status = false
                    };
                    return duplicateResponse;
                }
                GenericRepository<Account> generic = new GenericRepository<Account>(_context, this._dbSet, _httpContextAccessor);
                await generic.PostCustomer(account);
                await _context.SaveChangesAsync();
                var response = new AccountSuccess
                {
                    Response = "Account Created Successfully",
                    Status = true
                };
                return response;
            }
            catch (Exception ex)
            {
                var response = new AccountSuccess
                {
                    Response = ex.Message,
                    Status = false
                };
                return response;
                throw ex;
            }
           

        }

       

        public string GenerateAccountNo(Guid id, string bankName)
        {
            var getAccountId = GetCustomerId().Result;
            var getBankName =  _context.Banks.Where(c => c.BankName == bankName).SingleOrDefault();
            var random = new Random();
            var result = random.Next(1000000, 9000000);
            return getBankName.AccountNumberPrefix + result;
        }

        public Task<Guid> GenerateId()
        {
            var generatedGuid = Guid.NewGuid();
            return Task.FromResult(generatedGuid);
        }

        public async Task<AccountDetailsDTO> GetAccount(GetAccountDTO getAccount)
        {
            try
            {
                var getCustomerAccount = await _context.Accounts.Where(c => c.CustomerId == getAccount.CustomerId && c.AccountCategory.Name == getAccount.AccountTypeName && c.Bank.BankName == getAccount.BankName).Select(c => new AccountDetailsDTO
                {
                    AccountName = c.AccountName,
                    AccountBalance = c.AccountBalance.ToString("c"),
                    AccountNo = c.AccountNo,
                    AccountStatus = c.AccountStatus,
                    AccountCategoryName = c.AccountCategory.Name,
                    BankName = c.Bank.BankName,
                    CustomerId = c.CustomerId,
                    DateCreated = c.DateCreated.ToShortDateString()
                }).SingleOrDefaultAsync();
                if (getCustomerAccount == null)
                {
                    return null;
                }
                else
                {
                    return getCustomerAccount;
                }
            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }

        public async Task<string> GetAccountName()
        {
            var user = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            var customerName = await _context.Customers.Where(c => c.Email == user).Select(c => new
            {
                c.Firstname,
                c.Lastname
            }).SingleOrDefaultAsync();
            return customerName.Firstname + " " + customerName.Lastname;
        }

        public List<AccountCategory> GetAllAccountType()
        {
            var accountCategories = _context.AccountCategories.ToList();
            return accountCategories;
        }

        public List<Bank> GetAllBankName()
        {
            var banks = _context.Banks.ToList();
            return banks;
        }

        public List<BankNameDTO> GetAllBankNameById(Guid? getbanks)
        {
            var bankName = _context.Accounts.Where(c => c.CustomerId == getbanks).Select(c => new BankNameDTO {
                BankName = c.Bank.BankName
            }).Distinct().ToList();
            return bankName;
        }


        public string GetBankNameById(Guid id)
        {
            var getBankName = _context.Banks.Where(c => c.BankId == id).SingleOrDefault();
            return getBankName.BankName;
        }

        public async Task<Guid> GetCustomerId()
        {
            var user = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            var customerName = await _context.Customers.Where(c => c.Email == user).Select(c => new
            {
                c.Id,
            }).SingleOrDefaultAsync();
            return customerName.Id;
        }
    }
}
