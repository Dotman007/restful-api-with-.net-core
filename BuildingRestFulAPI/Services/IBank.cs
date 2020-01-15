using BuildingRestFulAPI.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingRestFulAPI.Services
{
    public interface IBank
    {
        Task<string> CreateBank(CreateBankDTO createBank);
        Task<Guid> GenerateGuid();
    }
}
