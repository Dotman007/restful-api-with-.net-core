using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingRestFulAPI.IdentityServerConfiguration.Services
{
    public interface IConfig
    {
        IEnumerable<ApiResource> GetApiResources();
        IEnumerable<Client> GetClients();
    }
}
