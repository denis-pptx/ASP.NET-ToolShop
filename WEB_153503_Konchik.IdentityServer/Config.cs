using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;

namespace WEB_153503_Konchik.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource("roles", "User roles", new List<string> { "role" })
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("api.read"),
                new ApiScope("api.write"),
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // m2m client credentials flow client
                new Client
                {
                    ClientId = "m2m.client",
                    ClientName = "Client Credentials Client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                    AllowedScopes = { "api.read", "api.write" }
                },

                // interactive client using code flow + pkce
                new Client
                {
                    ClientId = "WEB",
                    ClientSecrets = { new Secret("konchik-secret-key".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = { "https://localhost:7001/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:7001/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:7001/signout-callback-oidc" },

                    AllowOfflineAccess = true,
                    AllowedScopes =  { "openid", "profile", "api.read", "api.write", "roles" },
                },

                new Client
                {
                    ClientId = "blazorApp",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    RedirectUris = { "https://localhost:7151/authentication/login-callback" },
                    PostLogoutRedirectUris = { "https://localhost:7151/authentication/logout-callback" },
                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "api.read", "api.write" }
                },
            };
    }
}