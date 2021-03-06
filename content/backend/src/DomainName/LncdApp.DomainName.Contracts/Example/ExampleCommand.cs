using LeanCode.Contracts;
using LeanCode.Contracts.Security;

namespace LncdApp.DomainName.Contracts.Example;

[AllowUnauthorized]
public class ExampleCommand : ICommand
{
    public string Arg { get; set; }

    public static class ErrorCodes
    {
        public const int EmptyArg = 1;
    }
}
