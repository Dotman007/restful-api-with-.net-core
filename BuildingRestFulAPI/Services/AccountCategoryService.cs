using BuildingRestFulAPI.DAL;
using BuildingRestFulAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingRestFulAPI.Services
{
    public class AccountCategoryService : IAccountCategory
    {
        private readonly StoreContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        internal DbSet<AccountCategory> _dbSet;
        public AccountCategoryService(StoreContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            this._dbSet = _context.Set<AccountCategory>();
        }
        public async Task<string> CreateAccountCategory(CreateAccountCategoryDTO categoryDTO)
        {
            try
            {
                var accountCat = new AccountCategory
                {
                    AccountCategoryId = GenerateGuid().Result,
                    Name = categoryDTO.Name
                };
                GenericRepository<AccountCategory> generic = new GenericRepository<AccountCategory>(_context,this._dbSet,_httpContextAccessor);
                await generic.PostCustomer(accountCat);
                await _context.SaveChangesAsync();
                return "Account Category Added Successfully";
                
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
