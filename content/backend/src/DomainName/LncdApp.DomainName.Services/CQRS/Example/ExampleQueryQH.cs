using LeanCode.CQRS.Execution;
using LncdApp.DomainName.Contracts.Example;

namespace LncdApp.DomainName.Services.CQRS.Example;

public class ExampleQueryQH : IQueryHandler<DomainNameContext, ExampleQuery, QueryResultDTO>
{
    private readonly Serilog.ILogger logger = Serilog.Log.ForContext<ExampleQueryQH>();

    public Task<QueryResultDTO> ExecuteAsync(DomainNameContext context, ExampleQuery query)
    {
        logger.Information("ExampleQueryQH called");
        return Task.FromResult(new QueryResultDTO
        {
            Greeting = $"Hello {query.Name}!",
        });
    }
}
