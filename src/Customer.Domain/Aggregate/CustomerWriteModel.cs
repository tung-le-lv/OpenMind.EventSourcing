using Customer.Domain.Event;
using EventFlow.Aggregates;

namespace Customer.Domain.Aggregate;

public class CustomerWriteModel : AggregateState<CustomerAggregate, CustomerId, CustomerWriteModel>, IAuditable
{
    public string FirstName { get; private set; }
    public string? LastName { get; private set; }
    public string PhoneNumber { get; private set; }
    public string Email { get; private set; }
    public string Address { get; private set; }
    public string BillingAddress { get; private set; }
    public DateTime? DateOfBirth { get; private set; }
    public DateTimeOffset? CreatedAt { get; private set; }
    public DateTimeOffset? ModifiedAt { get; private set;  }
    public DateTimeOffset? DeletedAt { get; private set;  }
    public string? ActionUserId { get; private set;  }


    internal void Apply(CustomerCreatedEvent @event)
    {
        FirstName = @event.FirstName;
        LastName = @event.LastName;
        PhoneNumber = @event.PhoneNumber;
        Email = @event.Email;
        Address = @event.Address;
        BillingAddress = @event.BillingAddress;
        DateOfBirth = @event.DateOfBirth;
        CreatedAt = @event.CreatedAt;
        ActionUserId = @event.CreatedBy;
    }
    
    internal void Apply(CustomerDeletedEvent @event)
    {
        DeletedAt = @event.DeletedAt;
        ActionUserId = @event.DeletedBy;
    }

    internal void Apply(CustomerEmailChangedEvent @event)
    {
        Email = @event.Email;
    }
}
