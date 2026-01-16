using Customer.Domain.Event;
using Customer.Domain.Result;
using Customer.Domain.Specification;
using EventFlow.Aggregates;

namespace Customer.Domain.Aggregate;

public class CustomerAggregate : AggregateRoot<CustomerAggregate, CustomerId>
{
    private readonly CustomerWriteModel aggregateState;

    public CustomerAggregate(
        CustomerId id,
        CustomerWriteModel aggregateState) : base(id)
    {
        this.aggregateState = aggregateState;
        Register(this.aggregateState);
    }

    public void CreateCustomer(
        string firstName,
        string? lastName,
        string phoneNumber,
        string email,
        string address,
        string billingAddress,
        DateTime? dateOfBirth,
        string createdBy)
    {
        if (IsNew is false)
        {
            return;
        }

        Emit(new CustomerCreatedEvent(
            firstName,
            lastName,
            phoneNumber,
            email,
            address,
            billingAddress,
            dateOfBirth,
            createdAt: DateTimeOffset.UtcNow,
            createdBy));
    }

    public void DeleteCustomer(string deletedBy)
    {
        Emit(new CustomerDeletedEvent(deletedAt: DateTimeOffset.UtcNow, deletedBy));
    }

    public ChangeEmailResult ChangeEmailAddress(string newEmailAddress)
    {
        var specification = new ChangeEmailAddressSpecification(newEmailAddress);
        if (specification.IsSatisfiedBy(aggregateState) is false)
        {
            return specification.WhyIsNotSatisfiedBy(aggregateState)!;
        }

        Emit(new CustomerEmailChangedEvent(newEmailAddress));

        return new ChangeEmailResult.Success();
    }
}
