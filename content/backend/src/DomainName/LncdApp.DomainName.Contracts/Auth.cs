namespace LncdApp.DomainName.Contracts;

public static class Auth
{
    public static class Roles
    {
        public const string User = "user";
    }

    public static class KnownClaims
    {
        public const string UserId = "sub";
        public const string Role = "role";
    }

    public static class Clients
    {
        public const string AdminApp = "admin_app";
        public const string ClientApp = "client_app";
    }

    public static class Scopes
    {
        public const string InternalMainApp = "internal_mainapp";
    }
}
