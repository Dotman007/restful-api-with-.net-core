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
    public class CustomerRepository : ICustomer
    {
        private readonly StoreContext _context;
        internal DbSet<Customer> dbSet;
        //private readonly HttpContext _http;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomerRepository(StoreContext context, /*HttpContext http,*/ IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            this.dbSet = _context.Set<Customer>();
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<CustomerRegistrationResponse> DeleteCustomerAsync(Guid id)
        {
            try
            {
                var responseDelete = new CustomerRegistrationResponse
                {
                    Response = "Customer Details deleted successfully",
                };
                GenericRepository<Customer> generic = new GenericRepository<Customer>(_context, this.dbSet, _httpContextAccessor);
                var customer = await generic.DeleteCustomer(id);
                if (customer != null)
                {
                    _context.Customers.Remove(customer);
                    await _context.SaveChangesAsync();
                }
                return responseDelete;
            }
            catch (Exception ex)
            {
                var responseError = new CustomerRegistrationResponse
                {
                    Response = ex.Message,
                };
                return responseError;
                throw ex;
            }
        }

        public async Task<Customer> GetCustomerByIdAsync(Guid id)
        {
            try
            {
                Customer customer = await _context.Customers.FindAsync(id);
                var date = DateTime.Parse(customer.Dob.ToString("yyyy-MM-dd"));
                customer.Dob = date;
                return customer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CustomerIdDto> GetCustomerSensitiveInfo()
        {
            var user = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //var ident = _http.User.Identity as ClaimsIdentity;
            //var getUserId = ident.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var getInfo = await  _context.Customers.Where(c => c.Id.ToString() == user).Select(c=> new CustomerIdDto {
                Id = c.Id.ToString(),
                Username = c.Email,
                Password = c.Password,
                FirstName = c.Firstname
            }).SingleOrDefaultAsync();
            return getInfo;
        }

        //public string GetCustomerId()
        //{
        //    var ident = User.Identity as ClaimsIdentity;
        //    var userId = ident.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        //    return userId;
        //}

        public async Task<List<Customer>> GetCustomerAsync()
        {
            try
            {
                GenericRepository<Customer> generic = new GenericRepository<Customer>(_context, this.dbSet, _httpContextAccessor);
                var customers = await generic.GetCustomerAsync();
                return customers;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CustomerRegistrationResponse> RegisterCustomerAsync(CustomerDto customerdto)
        {
            try
            {
                var customer = new Customer
                {
                    Id = GenerateGuid().Result,
                    MainAddressId = GenerateGuid().Result,
                    Firstname = customerdto.Firstname,
                    Lastname = customerdto.Lastname,
                    CustomerUserName = customerdto.Firstname +  " " + customerdto.Lastname,
                    IsCustomer = true,
                    Dob = customerdto.Dob,
                    Email = customerdto.Email,
                    Gender = customerdto.Gender,
                    Fax = customerdto.Fax,
                    IsAgent = false,
                    NewsLetterOpted = true,
                    Password = customerdto.Password,
                    Telephone = customerdto.Telephone
                };
                GenericRepository<Customer> generic = new GenericRepository<Customer>(_context, this.dbSet, _httpContextAccessor);
                
                if (_context.Customers.Any(c => c.Email == customer.Email))
                {
                    var responseEmailExist = new CustomerRegistrationResponse
                    {
                        Response = "Customer email already exist",
                    };
                    return responseEmailExist;
                }
                var responseSuccess = new CustomerRegistrationResponse
                {
                    Response = "Customer Details successfully added",
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

        public async Task<CustomerRegistrationResponse> UpdateCustomerAsync(Guid id, Customer customer)
        {
            try
            {

                var customers = GetCustomerBeforeAsync(id);
                if (customers.Result == null)
                {
                    var resp = new CustomerRegistrationResponse
                    {
                        StatusCode = "33",
                        ResponseStatus = "no content",
                        Response = "The User id is not valid"
                    };
                    return resp;
                }
                if (!string.IsNullOrEmpty(customer.Firstname))
                {
                    customers.Result.Firstname = customer.Firstname;
                }
                if (!string.IsNullOrEmpty(customer.Lastname))
                {
                    customers.Result.Lastname = customer.Lastname;
                }
                if (!string.IsNullOrEmpty(customer.Telephone))
                {
                    customers.Result.Telephone = customer.Telephone;
                }
                if (!string.IsNullOrEmpty(customer.Gender))
                {
                    customers.Result.Gender = customer.Gender;
                }
                if (!string.IsNullOrEmpty(customer.Email))
                {
                    customers.Result.Email = customer.Email;
                }
                if (!string.IsNullOrEmpty(customer.Fax))
                {
                    customers.Result.Fax = customer.Fax;
                }
                if (!string.IsNullOrEmpty(customer.MainAddressId.ToString()))
                {
                    customers.Result.MainAddressId = customer.MainAddressId;
                }
                GenericRepository<Customer> generic = new GenericRepository<Customer>(_context, this.dbSet, _httpContextAccessor);
                await  generic.UpdateCustomerAsync(id, customer);
                var updateResponse = new CustomerRegistrationResponse
                {
                    Response = "Customer Record Updated Successfully",
                    StatusCode = "00",
                    ResponseStatus = "success"
                };
                return updateResponse;
            }
            catch (Exception ex)
            {
                var updateErro = new CustomerRegistrationResponse
                {
                    Response = "Customer Details was not updated",
                    StatusCode = "01",
                    ResponseStatus = "failed"
                };
                return updateErro;
                throw ex;
            }
        }
        public async Task<LoginSuccessDto> LoginAsync(LoginDto login)
        {
            //GenericRepository<Customer> generic = new GenericRepository<Customer>(_context, this.dbSet, _httpContextAccessor);
            //var result = await generic.LoginAsync(login);
            //if (result == null)
            //{
            //    return null;
            //}
            //return result;
            var result = await _context.Customers.Where(c => c.Email == login.Username && c.Password == login.Password && c.IsCustomer == true).Select(c => new LoginSuccessDto
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
                CustomerId = c.Id
            }).SingleOrDefaultAsync();
            if (result == null)
            {
                var message = new LoginSuccessDto
                {
                    Message = "Invalid username or password"
                };
                return message;
            }
            return result;
        }
        
        public async Task<Customer> GetCustomerBeforeAsync(Guid id)
        {
            GenericRepository<Customer> generic = new GenericRepository<Customer>(_context, this.dbSet, _httpContextAccessor);
            var customer = await generic.GetCustomerById(id);
            if (customer == null)
            {
                return null;
            }
            return customer;
        }

        public Task<Guid> GenerateGuid()
        {
            var generatedGuid = Guid.NewGuid();
            return Task.FromResult(generatedGuid);
        }

        public async Task<LoginSuccessDto> Dashboard()
        {
            GenericRepository<Customer> generic = new GenericRepository<Customer>(_context, this.dbSet, _httpContextAccessor);
            var user = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            var customer = await generic.Dashboard(user);
            if (customer == null)
            {
                return null;
            }
            return customer;
        }
    }
}

