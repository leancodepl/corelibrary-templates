using LeanCode.Contracts;
using LeanCode.Contracts.Security;

namespace LncdApp.DomainName.Contracts.Example;

[AllowUnauthorized]
public class ExampleOperation : IOperation<OperationResultDTO>
{
    public string Name { get; set; }
}

public class OperationResultDTO
{
    public string Greeting { get; set; }
}
