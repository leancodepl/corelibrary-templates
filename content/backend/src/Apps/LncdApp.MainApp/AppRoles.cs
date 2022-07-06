using LeanCode.CQRS.Security;

using R = LncdApp.DomainName.Contracts.Auth.Roles;

namespace LncdApp.MainApp;

internal class AppRoles : IRoleRegistration
{
    public IEnumerable<Role> Roles { get; } = new[]
    {
        new Role(R.User, R.User),
    };
}
