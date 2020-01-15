using BuildingRestFulAPI.DAL;
using BuildingRestFulAPI.Dtos;
using BuildingRestFulAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BuildingRestFulAPI.Services
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        internal StoreContext _context;
        internal DbSet<TEntity> _dbSet;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GenericRepository(StoreContext context, DbSet<TEntity> dbSet, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _dbSet = dbSet;
            _httpContextAccessor = httpContextAccessor;
        }


        public async virtual Task<LoginSuccessDto> LoginAsync(LoginDto login)
        {
            var result = await _context.Customers.Where(c=>c.Email == login.Username && c.Password == login.Password).Select(c => new LoginSuccessDto
            {
                Firstname = c.Firstname,
                Lastname = c.Lastname,
                Gender = c.Gender,
                Dob = c.Dob,
                Email = c.Email,
                Fax = c.Fax,
                MainAddressId = c.MainAddressId,
                NewsLetterOpted = c.NewsLetterOpted,
                Telephone = c.Telephone,
                Password = c.Password,
            }).SingleOrDefaultAsync();
            if (result == null)
            {
                var message = new LoginSuccessDto
                {
                    Message = "Invalid username or password"
                };
            }
            return result;
        }

        public async virtual Task<LoginSuccessDto> Dashboard(string username)
        {
            var user = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            var result = await _context.Customers.Where(c => c.Email == username).Select(c => new LoginSuccessDto
            {
                Firstname = c.Firstname,
                Lastname = c.Lastname,
                Gender = c.Gender,
                Dob = c.Dob,
                Email = c.Email,
                Fax = c.Fax,
                MainAddressId = c.MainAddressId,
                NewsLetterOpted = c.NewsLetterOpted,
                Telephone = c.Telephone
            }).SingleOrDefaultAsync();
            return result;
        }



        public async virtual Task<AgentLoginSuccessDto> AgentDashboard(string username)
        {
            var user = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            var getUserPassword = _context.Customers.Where(c => c.Email == user).Select(c => c.Password).SingleOrDefault();
            var result = await _context.Customers.Where(c => c.Email == username && c.Password == getUserPassword && c.IsAgent == true).Select(c => new AgentLoginSuccessDto
            {
                Firstname = c.Firstname,
                Lastname = c.Lastname,
                Gender = c.Gender,
                Dob = c.Dob,
                Email = c.Email,
                Fax = c.Fax,
                MainAddressId = c.MainAddressId,
                NewsLetterOpted = c.NewsLetterOpted,
                Telephone = c.Telephone,
                CustomerId = c.Id,
                AgentBank = c.AgentBank,
                AgentName = c.AgentName,
                AgentUserName = c.AgentUserName,
                Message = "Login Successful"
            }).SingleOrDefaultAsync();
            return result;
        }



        public async virtual Task<LoginResponseMessage> ManagementDashboard(string username)
        {
            var user = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var getRoleId = _context.Managements.Where(c => c.Username == user).Select(c => c.RoleId).SingleOrDefault();
            var result = await _context.Managements.Where(c => c.Username == username).Select(c => new LoginResponseMessage
            {
                Username = c.Username,
                UserId = c.Id,
                RoleName = _context.ManagementRoles.Where(d=>d.RoleId == getRoleId).Select(d=>d.RoleName).SingleOrDefault(),
            }).SingleOrDefaultAsync();
            return result;
        }

        public async virtual Task<TEntity> GetCustomerById(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async virtual Task<List<TEntity>> GetCustomerAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async virtual Task PostCustomer(TEntity entity)
        {
            await _dbSet.AddAsync(entity);

        }

        public async virtual Task<TEntity> DeleteCustomer(Guid id)
        {
            TEntity entity = await GetCustomerById(id);
            _dbSet.Remove(entity);
            return entity;
        }

        public async virtual Task<TEntity> UpdateCustomerAsync(Guid id, TEntity customer)
        {
            //var customers = GetCustomerById(id);
            await _context.SaveChangesAsync(true);
            return customer;
        }




        //public virtual void DeleteCustomer(Guid id)
        //{
        //    TEntity entityToDelete = GetCustomerById(id);
        //    DeleteCustomer(entityToDelete);
        //}

        //public virtual void DeleteCustomer(TEntity entityToDelete)
        //{
        //    if (_context.Entry(entityToDelete).State == EntityState.Detached)
        //    {
        //        _dbSet.Attach(entityToDelete);
        //    }
        //    _dbSet.Remove(entityToDelete);
        //}

    }
}
