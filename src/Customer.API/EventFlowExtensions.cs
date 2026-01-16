using Customer.Application.Commands;
using Customer.Application.DomainEventsSubscriber;
using Customer.Application.Query;
using Customer.Domain.Aggregate;
using Customer.Domain.Event;
using Customer.Infrastructure;
using EventFlow.EntityFramework;
using EventFlow.EntityFramework.Extensions;
using EventFlow.Extensions;
using EventFlow.PostgreSql.Connections;
using EventFlow.PostgreSql.Extensions;

namespace Customer.API;

public static class EventFlowExtensions
{
    public static void ConfigureEventFlow(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddTransient<CustomerWriteModel>();
        services.AddScoped<IDbContextProvider<CustomerReadDbContext>, CustomerReadDbContextProvider>();
        
        services.AddEventFlow(options => options
                .AddEvents(typeof(CustomerCreatedEvent).Assembly)
                .AddCommands(typeof(CreateCustomerCommand).Assembly)
                .AddCommandHandlers(typeof(CreateCustomerCommandHandler).Assembly)
                .AddSubscribers(typeof(CustomerDomainEventHandler).Assembly)
                .AddQueryHandlers(typeof(GetCustomerByIdQueryHandler).Assembly)
                .ConfigurePostgreSql(PostgreSqlConfiguration.New.SetConnectionString(builder.Configuration.GetConnectionString("DefaultConnection")))
                .UsePostgreSqlEventStore()
                .ConfigureEntityFramework(EntityFrameworkConfiguration.New)
                .UseEntityFrameworkReadModel<CustomerReadModel, CustomerReadDbContext>()
                .Configure(c => c.ThrowSubscriberExceptions = true)
                .AddDefaults(typeof(CustomerAggregate).Assembly)
        );
    }
}
