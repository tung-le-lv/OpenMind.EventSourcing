using Customer.Application.Dto;
using Customer.Application.Mapper;
using Customer.Domain.Aggregate;
using Customer.Infrastructure;
using EventFlow.EntityFramework;
using EventFlow.Queries;
using Microsoft.EntityFrameworkCore;

namespace Customer.Application.Query;

public class GetCustomerByIdQueryHandler(IDbContextProvider<CustomerReadDbContext> dbContextProvider)
    : IQueryHandler<GetReadModelByIdQuery<CustomerReadModel, CustomerDto?>, CustomerDto?>
{
    public async Task<CustomerDto?> ExecuteQueryAsync(GetReadModelByIdQuery<CustomerReadModel, CustomerDto?> query,
        CancellationToken cancellationToken)
    {
        await using var context = dbContextProvider.CreateContext();

        var customer = await context.CustomerReadModels
            .AsNoTracking()
            .Where(q => q.Id == query.Id.Value)
            .SingleOrDefaultAsync(cancellationToken);

        return customer is null ? null : CustomerMapper.ToDto(customer);
    }
}
