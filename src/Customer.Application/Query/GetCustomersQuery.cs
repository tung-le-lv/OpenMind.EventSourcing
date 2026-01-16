namespace Customer.Application.Query;

// QueryProcessor is currently only for JsonApiDotNetCore to handle filtering, sorting, sparse fieldsets, and pagination.

// public class GetCustomersQuery(IReadModelQueryProcessor<CustomerReadModel> queryProcessor)
//     : IQuery<IReadOnlyCollection<CustomerDto>>
// {
//     public IReadModelQueryProcessor<CustomerReadModel> QueryProcessor { get; } = queryProcessor;
// }
//
// public class GetCustomersQueryHandler(IDbContextProvider<CustomerReadDbContext> dbContextProvider)
//     : IQueryHandler<GetCustomerByIdQuery, IReadOnlyCollection<CustomerDto>>
// {
//     public async Task<IReadOnlyCollection<CustomerDto>> ExecuteQueryAsync(GetCustomerByIdQuery query, CancellationToken cancellationToken)
//     {
//         using (var context = dbContextProvider.CreateContext())
//         {
//             IQueryable<CustomerReadModel> queryable = context.CustomerReadModel.AsNoTracking();
//
//             // Process filters
//             var processingDate = systemClock.GetUtcNow();
//             queryable = queryable.Where(x => x.Period.Start!.Value <= processingDate && x.Period.End!.Value >= processingDate);
//             queryable = queryable.Where(x => !unacceptableStatuses.Contains(x.Status));
//             queryable = query.QueryProcessor.ProcessFilters(queryable);
//
//             // Process sorting
//             queryable = queryable.OrderBy(x => x.Period.End!.Value).ThenBy(x => x.CreatedDate);
//             queryable = query.QueryProcessor.ProcessSorting(queryable);
//
//             // Process sparse fieldset projection
//             queryable = query.QueryProcessor.ProcessSparseFieldsetProjection(
//                 queryable,
//                 fieldsToIgnore: new[] { nameof(UpcomingRenewal.PolicyHolder) });
//
//             // Process pagination
//             if (this.jsonApiOptions.IncludeTotalResourceCount)
//             {
//                 this.paginationContext.TotalResourceCount = await queryable.CountAsync(cancellationToken);
//             }
//             queryable = query.QueryProcessor.ProcessPaging(queryable);
//
//             return await queryable.ToArrayAsync(cancellationToken);
//         }
//     }
// }
