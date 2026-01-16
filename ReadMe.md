## Overview

A sample application demonstrating the **Event Sourcing** pattern using the [EventFlow](https://github.com/eventflow/EventFlow).

## Resources

- [EventFlow Documentation](https://eventflow.net/)
- [Event Sourcing Pattern - Microsoft](https://docs.microsoft.com/en-us/azure/architecture/patterns/event-sourcing)
- [CQRS Pattern - Martin Fowler](https://martinfowler.com/bliki/CQRS.html)

## What is Event Sourcing?

Event Sourcing is an architectural pattern where the state of an application is determined by a sequence of events rather than just storing the current state. Instead of updating a record in place, every change is captured as an immutable event and appended to an event store.

### Traditional CRUD vs Event Sourcing

| Traditional CRUD | Event Sourcing |
|------------------|----------------|
| Store current state only | Store all events that led to current state |
| Update/Delete overwrites data | Events are immutable, append-only |
| No history by default | Complete audit trail built-in |
| Single source of truth | Events are the source of truth |

### Key Benefits

- **Complete Audit Trail**: Every change is recorded as an event, providing full traceability
- **Temporal Queries**: Reconstruct the state at any point in time
- **Event-Driven Architecture**: Natural fit for microservices and CQRS
- **Debugging**: Replay events to understand how the system reached its current state

## EventFlow Library

[EventFlow](https://github.com/eventflow/EventFlow) is a popular .NET library for building event-sourced applications. It provides:

- **Aggregates**: Domain objects that encapsulate business logic and emit events
- **Commands & Command Handlers**: Encapsulate user intentions and execute business logic
- **Events**: Immutable records of state changes
- **Read Models**: Optimized projections for querying
- **Event Store**: Pluggable storage (PostgreSQL, SQL Server, EventStore, etc.)

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

## Project Structure

```
src/
├── Customer.API/           # REST API endpoints
├── Customer.Application/   # Commands, Queries, Event Handlers
├── Customer.Domain/        # Aggregates, Events, Read Models
├── Customer.Infrastructure/# Database context, Migrations
└── Customer.Contract/      # Avro schemas for Kafka events
```

## Tech Stack

- **.NET 9** - Runtime
- **EventFlow** - Event Sourcing framework
- **PostgreSQL** - Event Store & Read Model database
- **KafkaFlow** - Kafka client for event publishing
- **Redpanda** - Kafka-compatible streaming platform

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
