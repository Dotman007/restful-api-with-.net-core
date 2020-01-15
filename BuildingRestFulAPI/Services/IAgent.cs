using BuildingRestFulAPI.Dtos;
using BuildingRestFulAPI.Models;
using BuildingRestFulAPI.ResponseMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingRestFulAPI.Services
{
    public interface IAgent
    {
        Task<List<AgentDto>> GetCustomerAsync();
        Task<AgentDto> GetCustomerByIdAsync(Guid id);
        Task<CustomerRegistrationResponse> UpdateCustomerAsync(Guid id, AgentDto customer);
        Task<AgentDto> GetCustomerBeforeAsync(Guid id);
        Task<CustomerRegistrationResponse> RegisterAgentAsync(AgentDto customer);
        Task<CustomerRegistrationResponse> DeleteCustomerAsync(Guid id);
        Task<AgentIdDto> GetCustomerSensitiveInfo();
        Task<AgentLoginSuccessDto> AgentLoginAsync(LoginDto login);
        Task<AgentLoginSuccessDto> AgentDashboard();
        Task<int> GetNumberOfCustomers(Guid agentId);
        Task<int> TotalNumberOfSavingsAccount(Guid agentId);
        Task<int> TotalNumberOfCurrentAccount(Guid agentId);
        Task<int> TotalNumberOfFixedDepositAccount(Guid agentId);
        Task<int> TotalNumberOfOtherAccount(Guid agentId);
        Task<int> TotalNumberOfActiveAccount(Guid agentId);
        Task<int> TotalNumberOfBlockedAccount(Guid agentId);
        Task<int> TotalNumberOfActivatedAccount(Guid agentId);
        Task <List<AccountDto>> GetCustomersWithSavingsAccount(Guid agentId);
        Task <List<AccountDto>> GetCustomersWithCurrentAccount(Guid agentId);
        Task<List<AccountDto>> GetCustomersWithFixedDepositAccount(Guid agentId);
        Task<List<AccountDto>> GetCustomersWithOtherAccount(Guid agentId);

        Task<List<TransactionDto>> GetTodayTransaction(Guid agentId);
        Task<List<TransactionDto>> GetWeekTransaction(Guid agentId);
        Task<List<TransactionDto>> GetMonthTransaction(Guid agentId);
        Task<List<TransactionDto>> GetYearTransaction(Guid agentId);

        DateTime  GetWeekDay();
            
        Task<Guid> GenerateGuid();
    }
}
