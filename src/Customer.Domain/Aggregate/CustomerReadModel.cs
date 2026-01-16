using System.ComponentModel.DataAnnotations;
using Customer.Domain.Event;
using EventFlow.Aggregates;
using EventFlow.ReadStores;

namespace Customer.Domain.Aggregate;

public class CustomerReadModel :
    IReadModel,
    IAuditable,
    IAmReadModelFor<CustomerAggregate, CustomerId, CustomerCreatedEvent>,
    IAmReadModelFor<CustomerAggregate, CustomerId, CustomerEmailChangedEvent>
{
    [Required]
    public string Id { get; private set; } = null!;

    [Required]
    [MaxLength(100)]
    public string? FirstName { get; private set; }

    [MaxLength(100)]
    public string? LastName { get; private set; }

    [Required]
    public string? PhoneNumber { get; private set; }

    [MaxLength(100)]
    public string? Email { get; private set; }

    [Required]
    [MaxLength(200)]
    public string? Address { get; private set; }

    [MaxLength(200)]
    public string? BillingAddress { get; private set; }

    public DateTime? DateOfBirth { get; private set; }
    
    public DateTimeOffset? CreatedAt { get; private set; }
    public DateTimeOffset? ModifiedAt { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }
    public string? ActionUserId { get; private set; }

    public Task ApplyAsync(IReadModelContext context,
        IDomainEvent<CustomerAggregate, CustomerId, CustomerCreatedEvent> domainEvent,
        CancellationToken cancellationToken)
    {
        var @event = domainEvent.AggregateEvent;

        Id = domainEvent.AggregateIdentity.Value;
        FirstName = @event.FirstName;
        LastName = @event.LastName;
        PhoneNumber = @event.PhoneNumber;
        Email = @event.Email;
        Address = @event.Address;
        BillingAddress = @event.BillingAddress;
        DateOfBirth = @event.DateOfBirth.HasValue 
            ? DateTime.SpecifyKind(@event.DateOfBirth.Value, DateTimeKind.Utc) 
            : null;
        CreatedAt = @event.CreatedAt;
        ActionUserId = @event.CreatedBy;

        return Task.CompletedTask;
    }

    public Task ApplyAsync(IReadModelContext context,
        IDomainEvent<CustomerAggregate, CustomerId, CustomerEmailChangedEvent> domainEvent,
        CancellationToken cancellationToken)
    {
        var @event = domainEvent.AggregateEvent;
        Email = @event.Email;
        return Task.CompletedTask;
    }
}
