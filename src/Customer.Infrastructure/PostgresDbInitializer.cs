using EventFlow.PostgreSql;
using EventFlow.PostgreSql.EventStores;

namespace Customer.Infrastructure;

public class PostgresDbInitializer(IPostgreSqlDatabaseMigrator eventFlowDbMigrator)
{
    public async Task Initialize()
    {
        await EventFlowEventStoresPostgreSql.MigrateDatabaseAsync(eventFlowDbMigrator, CancellationToken.None);
    }
}
