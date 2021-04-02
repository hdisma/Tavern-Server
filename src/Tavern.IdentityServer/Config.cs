using IdentityServer4;
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
                    new ApiScope("tavern-api", "Tavern Api")
                };

        public static IEnumerable<Client> Clients =>
                new Client[]
                {
                    new Client
                    {
                        ClientId = "tavern-app",
                        ClientName = "Tavern Client Application",
                        RequireClientSecret = false,
                        AllowedGrantTypes = GrantTypes.Code,
                        RequirePkce = true,
                        AllowAccessTokensViaBrowser = true,
                        RequireConsent = false,

                        RedirectUris =           { "https://localhost:4200/signin-callback", "https://localhost:4200/assets/silent-callback.html" },
                        PostLogoutRedirectUris = { "https://localhost:4200/signout-callback" },
                        AllowedCorsOrigins =     { "https://localhost:4200" },

                        AllowedScopes =
                        {
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile,
                            "tavern-api"
                        },
                        AccessTokenLifetime = 900
                    }
                };
    }
}
