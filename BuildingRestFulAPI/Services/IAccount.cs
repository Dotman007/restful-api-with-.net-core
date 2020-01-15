using BuildingRestFulAPI.Dtos;
using BuildingRestFulAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingRestFulAPI.Services
{
    public interface IAccount
    {
        Task<AccountSuccess> CreateAccount(CreateAccountDTO createAccount);
        Task<AccountSuccess> ActivateAccount(string AccountNo);
        Task<Guid> GenerateId();
        Task<string> GetAccountName();

        string GetBankNameById(Guid id);
        List<Bank> GetAllBankName();
        List<BankNameDTO> GetAllBankNameById(Guid? getbanks);
        List<AccountCategory> GetAllAccountType();
        Task<List<AccountDetailsDTO>> AllMyAccounts(Guid? getAccount);
        Task<Guid> GetCustomerId();
        string GenerateAccountNo(Guid id, string bankName);

        Task<AccountDetailsDTO> GetAccount(GetAccountDTO getAccount);
    }
}
