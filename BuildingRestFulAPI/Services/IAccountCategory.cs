using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingRestFulAPI.Services
{
    public interface IAccountCategory
    {
        Task<string> CreateAccountCategory(CreateAccountCategoryDTO categoryDTO);
        Task<Guid> GenerateGuid();
    }
}
