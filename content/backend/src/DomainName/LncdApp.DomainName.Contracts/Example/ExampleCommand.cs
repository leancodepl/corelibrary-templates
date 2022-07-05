using LeanCode.CQRS;
using LeanCode.CQRS.Security;

namespace LncdApp.DomainName.Contracts.Example
{
    [AllowUnauthorized]
    public class ExampleCommand : IRemoteCommand
    {
        public string Arg { get; set; }

        public static class ErrorCodes
        {
            public const int EmptyArg = 1;
        }
    }
}
