using Customer.Domain.Aggregate;
using EventFlow.Aggregates;

namespace Customer.Domain.Event;

public sealed class CustomerEmailChangedEvent(string email) : AggregateEvent<CustomerAggregate, CustomerId>
{
    public string Email { get; } = email;
}
