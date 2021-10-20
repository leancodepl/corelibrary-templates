using System;
using System.Threading.Tasks;
using LNCDApp.DomainName.Contracts;
using LNCDApp.DomainName.Services.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;

namespace LNCDApp.DomainName.Services.Services
{
    public class IdentityUserManager
    {
        public const int MinimalPasswordLength = 8;
        private readonly UserManager<AuthUser> userManager;
        private readonly Serilog.ILogger logger = Serilog.Log.ForContext<IdentityUserManager>();

        public IdentityUserManager(UserManager<AuthUser> userManager)
        {
            this.userManager = userManager;
        }

        public static bool ValidatePassword(string password)
        {
            return password.Length >= MinimalPasswordLength;
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await userManager.FindByEmailAsync(email) is not null;
        }

        public async Task RegisterUser(Guid id, string email, string password)
        {
            var user = new AuthUser
            {
                Id = id,
                Email = email,
                UserName = email,
                Claims =
                {
                    new IdentityUserClaim<Guid>()
                    {
                        ClaimType = Auth.KnownClaims.Role,
                        ClaimValue = Auth.Roles.User,
                    },
                },
            };

            var result = await userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                logger.Error("Failed to create identity user, {@Errors}", result.Errors);
                throw new InvalidOperationException("Failed to create identity user");
            }
        }
    }
}
