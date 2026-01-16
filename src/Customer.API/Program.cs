using Customer.API;
using Customer.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var svcName = builder.Configuration.GetValue<string>("SERVICE_NAME") ?? "Customer";

builder.Services.AddOpenApi();

builder.Services.AddSerilog(builder.Configuration, builder.Host, svcName, "OpenMind");
builder.Services.ConfigureKafkaFlow(builder.Configuration);

builder.Services.AddDbContext<CustomerReadDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddTransient<PostgresDbInitializer>();

builder.Services.ConfigureEventFlow(builder);

builder.Services.AddProblemDetails();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbInitializer = scope.ServiceProvider.GetRequiredService<PostgresDbInitializer>();
    await dbInitializer.Initialize();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseStatusCodePages();
app.MapCustomerEndpoints();

app.Run();

