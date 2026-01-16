using Customer.Domain.Aggregate;
using EventFlow.Aggregates;

namespace Customer.Domain.Event;

public sealed class CustomerDeletedEvent(DateTimeOffset deletedAt, string deletedBy) : AggregateEvent<CustomerAggregate, CustomerId>
{
    public DateTimeOffset DeletedAt { get; } = deletedAt;
    public string DeletedBy { get; } = deletedBy;
    
}