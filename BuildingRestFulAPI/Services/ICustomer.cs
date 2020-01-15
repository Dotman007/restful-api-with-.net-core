using BuildingRestFulAPI.Dtos;
using BuildingRestFulAPI.Models;
using BuildingRestFulAPI.ResponseMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingRestFulAPI.Services
{
    public interface ICustomer
    {
        Task<List<Customer>> GetCustomerAsync();
        Task<Customer> GetCustomerByIdAsync(Guid id);
        Task<CustomerRegistrationResponse> UpdateCustomerAsync(Guid id, Customer customer);
        Task<Customer> GetCustomerBeforeAsync(Guid id);
        Task<CustomerRegistrationResponse> RegisterCustomerAsync(CustomerDto customer);
        Task<CustomerRegistrationResponse> DeleteCustomerAsync(Guid id);
        Task<CustomerIdDto> GetCustomerSensitiveInfo();
        Task<LoginSuccessDto> LoginAsync(LoginDto login);
        Task<LoginSuccessDto> Dashboard();
        Task<Guid> GenerateGuid();
    }
}
