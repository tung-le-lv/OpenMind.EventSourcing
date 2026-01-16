using Customer.Domain.Aggregate;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;

namespace Customer.Application.Commands;

public class CreateCustomerCommand(
    CustomerId aggregateId,
    string firstName,
    string? lastName,
    string phoneNumber,
    string email,
    string address,
    string billingAddress,
    DateTime? dateOfBirth,
    string createdBy) : Command<CustomerAggregate, CustomerId, CreateCustomerResult>(aggregateId)
{
    public string FirstName { get; } = firstName;
    public string? LastName { get; } = lastName;
    public string PhoneNumber { get; } = phoneNumber;
    public string Email { get; } = email;
    public string Address { get; } = address;
    public string BillingAddress { get; } = billingAddress;
    public DateTime? DateOfBirth { get; } = dateOfBirth;
    public string CreatedBy { get; } = createdBy;
}

public sealed class CreateCustomerCommandHandler
    : CommandHandler<CustomerAggregate, CustomerId, CreateCustomerResult, CreateCustomerCommand>
{
    public override Task<CreateCustomerResult> ExecuteCommandAsync(
        CustomerAggregate aggregate,
        CreateCustomerCommand cmd,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(cmd.Email))
        {
            return Task.FromResult<CreateCustomerResult>(new CreateCustomerResult.EmailAddressRequired());
        }

        aggregate.CreateCustomer(
            cmd.FirstName,
            cmd.LastName,
            cmd.PhoneNumber,
            cmd.Email,
            cmd.Address,
            cmd.BillingAddress,
            cmd.DateOfBirth,
            cmd.CreatedBy);

        return Task.FromResult<CreateCustomerResult>(new CreateCustomerResult.Success(aggregate.Id.GetGuid()));
    }
}

public abstract record CreateCustomerResult(bool IsSuccess) : IExecutionResult
{
    public sealed record Success(Guid CustomerId) : CreateCustomerResult(true);

    public sealed record EmailAddressRequired() : CreateCustomerResult(false);
}
