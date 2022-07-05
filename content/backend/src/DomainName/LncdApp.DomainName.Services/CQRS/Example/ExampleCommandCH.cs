using FluentValidation;
using LeanCode.CQRS.Execution;
using LeanCode.CQRS.Validation.Fluent;
using LncdApp.DomainName.Contracts.Example;

namespace LncdApp.DomainName.Services.CQRS.Example
{
    public class ExampleCommandCV : ContextualValidator<ExampleCommand>
    {
        public ExampleCommandCV()
        {
            RuleFor(cmd => cmd.Arg)
                .NotEmpty()
                    .WithCode(ExampleCommand.ErrorCodes.EmptyArg);
        }
    }

    public class ExampleCommandCH : ICommandHandler<DomainNameContext, ExampleCommand>
    {
        private readonly Serilog.ILogger logger = Serilog.Log.ForContext<ExampleCommandCH>();

        public Task ExecuteAsync(DomainNameContext context, ExampleCommand command)
        {
            logger.Information("ExampleCommandCH called");
            return Task.CompletedTask;
        }
    }
}
