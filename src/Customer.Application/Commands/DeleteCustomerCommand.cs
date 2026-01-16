using Customer.Domain.Aggregate;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;

namespace Customer.Application.Commands;

public class DeleteCustomerCommand(CustomerId aggregateId, string deletedBy)
    : Command<CustomerAggregate, CustomerId, DeleteCustomerResult>(aggregateId)
{
    public string DeletedBy { get; private set; } = deletedBy;
}

public sealed class DeleteCustomerCommandHandler
    : CommandHandler<CustomerAggregate, CustomerId, DeleteCustomerResult, DeleteCustomerCommand>
{
    public override Task<DeleteCustomerResult> ExecuteCommandAsync(
        CustomerAggregate aggregate,
        DeleteCustomerCommand cmd,
        CancellationToken cancellationToken)
    {
        if (aggregate.IsNew)
        {
            return Task.FromResult<DeleteCustomerResult>(new DeleteCustomerResult.CustomerNotFound());
        }

        aggregate.DeleteCustomer(cmd.DeletedBy);

        return Task.FromResult<DeleteCustomerResult>(new DeleteCustomerResult.Success(aggregate.Id.GetGuid()));
    }
}

public abstract record DeleteCustomerResult(bool IsSuccess) : IExecutionResult
{
    public sealed record Success(Guid CustomerId) : DeleteCustomerResult(true);
    public sealed record CustomerNotFound() : DeleteCustomerResult(false);
}
