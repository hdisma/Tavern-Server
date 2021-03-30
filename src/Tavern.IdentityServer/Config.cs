using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tavern.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
                new IdentityResource[]
                {
                        new IdentityResources.OpenId(),
                        new IdentityResources.Profile()
                };

        public static IEnumerable<ApiScope> ApiScopes =>
                new ApiScope[]
                {
                    new ApiScope("test-api", "Test Api")
                };

        public static IEnumerable<Client> Clients =>
                new Client[]
                { };
    }
}
