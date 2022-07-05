using IdentityServer4.Models;
using static IdentityServer4.IdentityServerConstants;
using AuthConsts = LncdApp.DomainName.Contracts.Auth;

namespace LncdApp.MainApp.Auth
{
    internal static class ClientsConfiguration
    {
        public static List<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = AuthConsts.Clients.AdminApp,
                    ClientName = "LncdApp Admin App",
                    AllowedGrantTypes = new[] { GrantType.ResourceOwnerPassword },
                    AllowOfflineAccess = true,
                    RequireClientSecret = false,

                    AbsoluteRefreshTokenLifetime = (int)TimeSpan.FromDays(365 * 10).TotalSeconds,

                    AllowedScopes = new List<string>
                    {
                        StandardScopes.OpenId,
                        StandardScopes.Profile,
                        StandardScopes.Email,
                        StandardScopes.Phone,
                        StandardScopes.OfflineAccess,

                        AuthConsts.Scopes.InternalMainApp,
                    },
                },
                new Client
                {
                    ClientId = AuthConsts.Clients.ClientApp,
                    ClientName = "LncdApp Client App",
                    AllowedGrantTypes = new[] { GrantType.ResourceOwnerPassword },
                    AllowOfflineAccess = true,
                    RequireClientSecret = false,

                    AbsoluteRefreshTokenLifetime = (int)TimeSpan.FromDays(365 * 10).TotalSeconds,

                    AllowedScopes = new List<string>
                    {
                        StandardScopes.OpenId,
                        StandardScopes.Profile,
                        StandardScopes.Email,
                        StandardScopes.Phone,
                        StandardScopes.OfflineAccess,

                        AuthConsts.Scopes.InternalMainApp,
                    },
                },
            };
        }

        public static List<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResources.Phone(),
            };
        }

        public static List<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource(AuthConsts.Scopes.InternalMainApp, "Internal Api")
                {
                    Scopes = { AuthConsts.Scopes.InternalMainApp },
                    UserClaims = { AuthConsts.KnownClaims.Role },
                },
            };
        }

        public static List<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope(AuthConsts.Scopes.InternalMainApp, "Full access to the LncdApp Api")
                {
                    UserClaims = { AuthConsts.KnownClaims.Role },
                },
            };
        }
    }
}
