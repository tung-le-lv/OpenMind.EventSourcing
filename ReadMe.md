## Event Sourcing

Event Sourcing is an architectural pattern where the state of an application is determined by a sequence of events rather than just storing the current state. Instead of updating a record in place, every change is captured as an immutable event and appended to an event store.

### CONTEXT AND PROLEM
- The traditional approach to persistence:
- Maps classes to database tables.
- Fields of those classes to table columns.
- Instances of those classes to rows in those tables.

### THE TROUBLE WITH TRADITIONAL PERSISTENCE
- Object-Relational impedance mismatch.
- Lack of aggregate history.
- Implementing audit logging.
- Lack of event publishing.

### EVENT SOURCING PATTERN
- When an application creates or updates an aggregate, it inserts the events emitted
by the aggregate into the EVENTS table.
- An application loads an aggregate from the event store by retrieving its events and
replaying them to build its latest state.
- Loading an aggregate consists of the following three steps:
- Load the events for the aggregate.
- Create an aggregate instance by using its default constructor.
- Iterate through the events, calling apply().

### EVENT SOURCING CHALLENGES
- Handling concurrent updates.
- Performance.
- Idempotent message processing.
- Evolving domain events.

### BENEFITS OF EVENT SOURCING
- Reliably publishes domain events.
- Preserves the history of aggregates.
- Mostly avoids the O/R impedance mismatch problem.
- Provides developers with a time machine.

## EventFlow Library

[EventFlow](https://github.com/eventflow/EventFlow) is a popular .NET library for building event-sourced applications. It provides:

- **Aggregates**: Domain objects that encapsulate business logic and emit events
- **Commands & Command Handlers**: Encapsulate user intentions and execute business logic
- **Events**: Immutable records of state changes
- **Read Models**: Optimized projections for querying
- **Event Store**: Pluggable storage (PostgreSQL, SQL Server, EventStore, etc.)

## References

- [EventFlow Documentation](https://eventflow.net/)
- [Event Sourcing Pattern - Microsoft](https://docs.microsoft.com/en-us/azure/architecture/patterns/event-sourcing)
- [CQRS Pattern - Martin Fowler](https://martinfowler.com/bliki/CQRS.html)

### How It Works in This Project

```
┌─────────────┐     ┌──────────────┐     ┌─────────────────┐
│   Command   │────▶│  Aggregate   │────▶│  Domain Event   │
│ (Intent)    │     │ (Business    │     │ (State Change)  │
└─────────────┘     │  Logic)      │     └────────┬────────┘
                    └──────────────┘              │
                                                  ▼
                    ┌──────────────┐     ┌─────────────────┐
                    │  Read Model  │◀────│  Event Store    │
                    │ (Query View) │     │ (PostgreSQL)    │
                    └──────────────┘     └────────┬────────┘
                                                  │
                                                  ▼
                                         ┌─────────────────┐
                                         │     Kafka       │
                                         │ (Integration)   │
                                         └─────────────────┘
```

### Example Flow: Creating a Customer

1. **Command**: `CreateCustomerCommand` is sent with customer details
2. **Aggregate**: `CustomerAggregate` validates and emits `CustomerCreatedEvent`
3. **Event Store**: Event is persisted to PostgreSQL
4. **Read Model**: `CustomerReadModel` is updated for fast queries
5. **Integration**: Event is published to Kafka for other services

## Getting Started

### Prerequisites

- .NET 9 SDK
- Docker & Docker Compose

### Run Infrastructure

```bash
docker compose --profile infra up -d
```

This starts:
- **Redpanda** (Kafka) on port 9092
- **PostgreSQL** on port 5433
- **Redpanda Console** on port 8080

### Apply Database Migrations

```bash
cd src/Customer.API
dotnet ef database update --project ../Customer.Infrastructure
```

### Run the API

```bash
cd src/Customer.API
dotnet run
```

### API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/customer` | Create a new customer |
| GET | `/customer/{id}` | Get customer by ID |
| POST | `/customer/{id}/email` | Update customer email |
| DELETE | `/customer/{id}` | Delete a customer |
