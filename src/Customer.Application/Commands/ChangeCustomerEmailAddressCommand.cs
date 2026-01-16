using Customer.Domain.Aggregate;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;

namespace Customer.Application.Commands;

public class ChangeCustomerEmailAddressCommand(
    CustomerId aggregateId,
    string email) : Command<CustomerAggregate, CustomerId, ChangeEmailResult>(aggregateId)
{
    public string Email { get; } = email;
}

public sealed class ChangeCustomerEmailAddressCommandHandler
    : CommandHandler<CustomerAggregate, CustomerId, ChangeEmailResult, ChangeCustomerEmailAddressCommand>
{
    public override Task<ChangeEmailResult> ExecuteCommandAsync(CustomerAggregate aggregate,
        ChangeCustomerEmailAddressCommand cmd,
        CancellationToken cancellationToken)
    {
        if (aggregate.IsNew)
        {
            return Task.FromResult<ChangeEmailResult>(new ChangeEmailResult.CustomerNotFound());
        }

        var result = aggregate.ChangeEmailAddress(cmd.Email);

        return result is Domain.Result.ChangeEmailResult.NewEmailIsTheSameAsCurrent
            ? Task.FromResult<ChangeEmailResult>(new ChangeEmailResult.NewEmailMustBeDifferentFromTheCurrent())
            : Task.FromResult<ChangeEmailResult>(new ChangeEmailResult.Success(aggregate.Id.GetGuid()));
    }
}

public abstract record ChangeEmailResult(bool IsSuccess) : IExecutionResult
{
    public sealed record Success(Guid CustomerId) : ChangeEmailResult(true);

    public sealed record CustomerNotFound() : ChangeEmailResult(false);

    public sealed record NewEmailMustBeDifferentFromTheCurrent() : ChangeEmailResult(false);
}
