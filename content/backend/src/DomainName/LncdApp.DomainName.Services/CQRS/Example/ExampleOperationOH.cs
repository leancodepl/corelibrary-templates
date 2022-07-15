using LeanCode.CQRS.Execution;
using LncdApp.DomainName.Contracts.Example;

namespace LncdApp.DomainName.Services.CQRS.Example;

public class ExampleOperationOH : IOperationHandler<DomainNameContext, ExampleOperation, OperationResultDTO>
{
    private readonly Serilog.ILogger logger = Serilog.Log.ForContext<ExampleOperationOH>();

    public Task<OperationResultDTO> ExecuteAsync(DomainNameContext context, ExampleOperation operation)
    {
        logger.Information("ExampleOperationOH called");
        return Task.FromResult(new OperationResultDTO
        {
            Greeting = $"Hi {operation.Name}!",
        });
    }
}
