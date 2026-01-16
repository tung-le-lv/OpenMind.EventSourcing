### EF migration
Connection string for migration is hardcoded in class CustomerReadDbContextFactory.
```powershell
cd Customer/Customer.Infrastructure
dotnet ef database update
```
Add migration:
```powershell
 dotnet ef migrations add MigrationName
```

### Outbox messaging
Manually run following sql scripts for Outbox table
https://github.com/AlexeyRaga/kafkaflow-contrib/tree/main/src/Contrib.KafkaFlow.Outbox.Postgres/schema 

```sql
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

DO
$do$
BEGIN
    IF
NOT EXISTS (
            SELECT schema_name
            FROM information_schema.schemata
            WHERE schema_name = 'outbox'
        ) THEN
            EXECUTE ('CREATE SCHEMA "outbox"');
END IF;
END
$do$;

CREATE TABLE "outbox"."outbox"
(
    sequence_id     SERIAL       NOT NULL,
    topic_name      VARCHAR(255) NOT NULL,
    partition       INT NULL,
    message_key     BYTEA NULL,
    message_headers TEXT NULL,
    message_body    BYTEA NULL,
    date_added_utc  TIMESTAMP WITHOUT TIME ZONE NOT NULL CONSTRAINT df_Outbox_Outbox_date_added_utc DEFAULT (now() AT TIME ZONE 'utc'),
    CONSTRAINT pk_Outbox_Outbox PRIMARY KEY (sequence_id),
    CONSTRAINT ck_Outbox_Outbox_Headers_Not_Blank_Or_Empty CHECK (TRIM(message_headers) <> ''),
    CONSTRAINT ck_Outbox_Outbox_topic_name_not_blank_or_empty CHECK (TRIM(topic_name) <> '')
);

ALTER TABLE "outbox"."outbox" ALTER COLUMN "sequence_id" TYPE BIGINT;
```