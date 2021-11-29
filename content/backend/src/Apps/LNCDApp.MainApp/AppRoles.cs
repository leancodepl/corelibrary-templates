using LeanCode.CQRS.Security;

using R = LNCDApp.DomainName.Contracts.Auth.Roles;

namespace LNCDApp.MainApp
{
    internal class AppRoles : IRoleRegistration
    {
        public IEnumerable<Role> Roles { get; } = new[]
        {
            new Role(R.User, R.User),
        };
    }
}
