using BuildingRestFulAPI.Dtos;
using BuildingRestFulAPI.Models;
using BuildingRestFulAPI.ResponseMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingRestFulAPI.Services
{
    public interface ICustomerRepository : IDisposable
    {
        List<Customer> GetCustomersAsync();
        Task<Customer> GetCustomerByIdAsync(Guid id);
        Task<CustomerRegistrationResponse> UpdateCustomerAsync(Guid id, Customer customer);
        Task<Customer> GetCustomerBeforeAsync(string id);
        Task<CustomerRegistrationResponse> RegisterCustomerAsync(Customer customer);
        Task<CustomerRegistrationResponse> DeleteCustomerAsync(Guid id);
        Task<CustomerIdDto> GetCustomerSensitiveInfo();
        Task<LoginSuccessDto> LoginAsync(LoginDto login);
        Task<Guid> GenerateGuid();
    }
}
