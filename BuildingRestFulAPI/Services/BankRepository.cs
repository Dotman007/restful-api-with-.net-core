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
    public class BankRepository : IBank
    {
        private readonly StoreContext _context;
        internal DbSet<Bank> dbSet;
        private readonly HttpContext _http;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BankRepository(StoreContext context, /*HttpContext http,*/ IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            this.dbSet = _context.Set<Bank>();
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> CreateBank(CreateBankDTO createBank)
        {
            try
            {
                var user = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
                var bank = new Bank
                {
                    BankId = GenerateGuid().Result,
                    BankName = createBank.BankName,
                    SortCode = createBank.SortCode,
                    AccountNumberPrefix = createBank.AccountNumberPrefix,
                    AgentName = user,
                    //CustomerId = _context.Customers.Where(c => c.Email == user).Select(c => c.Id).SingleOrDefault()
                };
                GenericRepository<Bank> generic = new GenericRepository<Bank>(_context, this.dbSet, _httpContextAccessor);
                await generic.PostCustomer(bank);
                await _context.SaveChangesAsync();
                return "Bank Created Successfully";
            }
            catch (Exception ex)
            {
                return ex.Message;
                throw ex;
            }
        }
        public Task<Guid> GenerateGuid()
        {
            var generatedGuid = Guid.NewGuid();
            return Task.FromResult(generatedGuid);
        }
    }
}
