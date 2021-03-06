using LeanCode.DomainModels.Model;
using LeanCode.Time;

namespace LncdApp.DomainName.Domain.Example.Events;

public class ExampleEvent : IDomainEvent
{
    public Guid Id { get; private init; }
    public DateTime DateOccurred { get; private init; }

    public ExampleEvent()
    {
        Id = Guid.NewGuid();
        DateOccurred = TimeProvider.Now;
    }
}
