using Customer.Domain.Aggregate;
using Customer.Domain.Event;
using Customer.Infrastructure;
using EventFlow.Aggregates;
using EventFlow.Subscribers;
using KafkaFlow;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpenMind.Customer.Integration.Events;
using OpenMind.Kafka.Producers;

namespace Customer.Application.DomainEventsSubscriber;

// EventFlow event handler isn't transactional consistent with read or write model context.
public class CustomerDomainEventHandler(
    CustomerReadDbContext dbContext,
    IMessageProducer<IOutboxEventsProducer> outboxPublisher,
    ILogger<CustomerDomainEventHandler> logger) 
    : ISubscribeSynchronousTo<CustomerAggregate, CustomerId, CustomerCreatedEvent>, ISubscribeSynchronousTo<CustomerAggregate, CustomerId, CustomerDeletedEvent>
{
    public async Task HandleAsync(IDomainEvent<CustomerAggregate, CustomerId, CustomerCreatedEvent> @event, CancellationToken cancellationToken)
    {
        var customerId = @event.AggregateIdentity.Value;

        await outboxPublisher.ProduceAsync(customerId, new
            CustomerCreated
            {
                Id = @event.AggregateIdentity.GetGuid(),
                FirstName = @event.AggregateEvent.FirstName,
                LastName = @event.AggregateEvent.LastName,
                PhoneNumber = @event.AggregateEvent.PhoneNumber,
                Email = @event.AggregateEvent.Email,
                Address = @event.AggregateEvent.Address,
                BillingAddress = @event.AggregateEvent.BillingAddress
            }
        );

        logger.LogInformation("Successfully published CustomerCreated event with CustomerId: {Id}", customerId);
    }
    
    public async Task HandleAsync(IDomainEvent<CustomerAggregate, CustomerId, CustomerDeletedEvent> @event, CancellationToken cancellationToken)
    {
        var customerId = @event.AggregateIdentity.Value;
        
        await RemoveCustomerReadModelAsync(customerId, cancellationToken);

        await outboxPublisher.ProduceAsync(customerId, new
            CustomerDeleted
            {
                Id = @event.AggregateIdentity.GetGuid(),
                DeletedAt = @event.AggregateEvent.DeletedAt.UtcDateTime
            }
        );

        logger.LogInformation("Successfully published CustomerDeleted event with CustomerId: {Id}", customerId);
    }
    
    private async Task RemoveCustomerReadModelAsync(string customerId, CancellationToken cancellationToken)
    {
        var customerReadModel = await dbContext.CustomerReadModels
            .FirstOrDefaultAsync(p => p.Id == customerId, cancellationToken);

        if (customerReadModel != null)
        {
            dbContext.CustomerReadModels.Remove(customerReadModel);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
