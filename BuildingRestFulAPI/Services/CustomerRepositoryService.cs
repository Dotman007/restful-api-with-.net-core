//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using BuildingRestFulAPI.DAL;
//using BuildingRestFulAPI.Dtos;
//using BuildingRestFulAPI.Models;
//using BuildingRestFulAPI.ResponseMessages;
//using Microsoft.AspNetCore.Http;

//namespace BuildingRestFulAPI.Services
//{
//    public class CustomerRepositoryService : ICustomerRepository, IDisposable
//    {
//        public CustomerRepositoryService()
//        {

//        }
//        private readonly StoreContext _context;
//        //private readonly HttpContext _http;
//        private readonly IHttpContextAccessor _httpContextAccessor;

//        public CustomerRepositoryService(StoreContext context, /*HttpContext http,*/ IHttpContextAccessor httpContextAccessor)
//        {
//            _context = context;
//            //_http = http;
//            _httpContextAccessor = httpContextAccessor;
//        }


//        public Task<CustomerRegistrationResponse> DeleteCustomerAsync(Guid id)
//        {
//            throw new NotImplementedException();
//        }

//        public void Dispose()
//        {
//            throw new NotImplementedException();
//        }

//        public Task<Guid> GenerateGuid()
//        {
//            throw new NotImplementedException();
//        }

//        public Task<Customer> GetCustomerBeforeAsync(Guid id)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<Customer> GetCustomerByIdAsync(Guid id)
//        {
//            throw new NotImplementedException();
//        }

//        public List<Customer> GetCustomersAsync()
//        {
//            throw new NotImplementedException();
//        }

//        public Task<CustomerIdDto> GetCustomerSensitiveInfo()
//        {
//            throw new NotImplementedException();
//        }

//        public Task<LoginSuccessDto> LoginAsync(LoginDto login)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<CustomerRegistrationResponse> RegisterCustomerAsync(Customer customer)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<CustomerRegistrationResponse> UpdateCustomerAsync(Guid id, Customer customer)
//        {
//            throw new NotImplementedException();
//        }

//        public void Save()
//        {
//            _context.SaveChanges();
//        }

//        private bool disposed = false;

//        protected virtual void Dispose(bool disposing)
//        {
//            if (!this.disposed)
//            {
//                if (disposing)
//                {
//                    _context.Dispose();
//                }
//            }
//            this.disposed = true;
//        }

//        public void Dispose()
//        {
//            Dispose(true);
//            GC.SuppressFinalize(this);
//        }
//    }
//}
