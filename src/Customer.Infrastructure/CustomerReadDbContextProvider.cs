using EventFlow.EntityFramework;
using Microsoft.Extensions.DependencyInjection;

namespace Customer.Infrastructure;

public class CustomerReadDbContextProvider(IServiceProvider serviceProvider)
    : IDbContextProvider<CustomerReadDbContext>
{
    public CustomerReadDbContext CreateContext()
    {
        return serviceProvider.GetRequiredService<CustomerReadDbContext>();
    }
}
