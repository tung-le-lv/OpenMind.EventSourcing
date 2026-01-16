using Customer.Domain.Aggregate;
using EventFlow.Aggregates;

namespace Customer.Domain.Event;

public sealed class CustomerCreatedEvent(
    string firstName,
    string? lastName,
    string phoneNumber,
    string email,
    string address,
    string billingAddress,
    DateTime? dateOfBirth,
    DateTimeOffset createdAt,
    string createdBy): AggregateEvent<CustomerAggregate, CustomerId>
{
    public string FirstName { get; } = firstName;
    public string? LastName { get; } = lastName;
    public string PhoneNumber { get; } = phoneNumber;
    public string Email { get; } = email;
    public string Address { get; } = address;
    public string BillingAddress { get; } = billingAddress;
    public DateTime? DateOfBirth { get; } = dateOfBirth;
    public DateTimeOffset? CreatedAt { get; } = createdAt;
    public string? CreatedBy { get; } = createdBy;
}
