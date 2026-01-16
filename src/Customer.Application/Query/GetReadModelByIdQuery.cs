using EventFlow.Core;
using EventFlow.Queries;
using EventFlow.ReadStores;

namespace Customer.Application.Query;

public class GetReadModelByIdQuery<TReadModel, TResult>(IIdentity id, IReadModelQueryProjector<TReadModel>? queryProcessor = null)
    : IQuery<TResult?> where TReadModel : class, IReadModel
{
    public IIdentity Id { get; } = id;
    public IReadModelQueryProjector<TReadModel>? QueryProcessor { get; } = queryProcessor;
}
