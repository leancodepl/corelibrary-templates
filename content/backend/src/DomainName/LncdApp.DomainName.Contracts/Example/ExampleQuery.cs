using LeanCode.Contracts;
using LeanCode.Contracts.Security;

namespace LncdApp.DomainName.Contracts.Example;

[AllowUnauthorized]
public class ExampleQuery : IQuery<QueryResultDTO>
{
    public string Name { get; set; }
}

public class QueryResultDTO
{
    public string Greeting { get; set; }
}
