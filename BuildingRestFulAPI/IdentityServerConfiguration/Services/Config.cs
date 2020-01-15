using BuildingRestFulAPI.IdentityServerConfiguration.Services;
using IdentityModel;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingRestFulAPI.IdentityServerConfiguration
{
    public class Config 
    {
        public  static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource
                (
                    "BuildingRestFulAPI.ReadAccess",
                    "BuildingRestFulAPI API",
                    new List<string>
                    {
                        JwtClaimTypes.Id,
                        JwtClaimTypes.Email,
                        JwtClaimTypes.Name,
                        JwtClaimTypes.GivenName,
                        JwtClaimTypes.FamilyName
                    }),new ApiResource("BuildingRestFulAPI.FullAccess","BuildingRestFulAPI API")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                new Client
                {
                    Enabled = true,
                    ClientName = "HTML Page Client",
                    ClientId="htmlClient",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets=
                    {
                        new Secret("secretpassword".Sha256())
                    },
                    AllowedScopes={ "BuildingRestFulAPI.ReadAccess" }
                }
            };
        }
    }
}
