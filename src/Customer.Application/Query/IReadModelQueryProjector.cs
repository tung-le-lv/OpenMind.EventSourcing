namespace Customer.Application.Query;

public interface IReadModelQueryProjector<TReadModel>
{
    IQueryable<TReadModel> ProcessSparseFieldsetProjection(
        IQueryable<TReadModel> entities,
        string[]? fieldsToIgnore = null,
        IDictionary<string, string[]>? fieldMapping = null);
}

public interface IReadModelQueryProcessor<TReadModel> : IReadModelQueryProjector<TReadModel>
{
    IQueryable<TReadModel> ProcessFilters(IQueryable<TReadModel> entities);
    IQueryable<TReadModel> ProcessSorting(IQueryable<TReadModel> entities);
    IQueryable<TReadModel> ProcessPaging(IQueryable<TReadModel> entities);
}