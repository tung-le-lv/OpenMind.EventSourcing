using Customer.Application.Commands;
using Customer.Application.Dto;
using Customer.Domain.Aggregate;
using EventFlow;
using EventFlow.Queries;
using OpenMind.Common.Authentication;
using OpenMind.Common.CQRS.EventFlow;

namespace Customer.API;

public static class CustomerEndpoints
{
    public static void MapCustomerEndpoints(this WebApplication app)
    {
        app.MapGet("/customer/{id:guid}", async (Guid id, IQueryProcessor queryProcessor) =>
            {
                var customer = await queryProcessor
                    .ProcessAsync(new GetReadModelByIdQuery<CustomerReadModel, CustomerDto?>(CustomerId.With(id)), CancellationToken.None)
                    .ConfigureAwait(false);

                if (customer is null)
                {
                    return Results.NotFound("Customer not found.");
                }

                return Results.Ok(customer);
            })
            .WithName("GetCustomer")
            .RequireAuthorization();

        app.MapPost("/customer", async (CreateCustomerRequest request, ICommandBus commandBus, HttpContext httpContext) =>
            {
                var createdBy = httpContext.User.GetUserId() ?? string.Empty;
                var command = new CreateCustomerCommand(
                    CustomerId.NewComb(),
                    request.FirstName,
                    request.LastName,
                    request.PhoneNumber,
                    request.Email,
                    request.Address,
                    request.BillingAddress,
                    request.DateOfBirth,
                    createdBy);

                var result = await commandBus.PublishAsync(command, CancellationToken.None);

                return result switch
                {
                    CreateCustomerResult.EmailAddressRequired => Results.BadRequest("Email address is required."),
                    CreateCustomerResult.Success successResult => Results.Ok(successResult.CustomerId),
                    _ => Results.InternalServerError("Unknown error.")
                };
            })
            .WithName("CreateCustomer")
            .RequireAuthorization();
        
        app.MapDelete("/customer/{id:guid}", async (Guid id, ICommandBus commandBus, HttpContext httpContext) =>
            {
                var deletedBy = httpContext.User.GetUserId() ?? string.Empty;
                var command = new DeleteCustomerCommand(CustomerId.With(id), deletedBy);
                var result = await commandBus.PublishAsync(command, CancellationToken.None);

                return result switch
                {
                    DeleteCustomerResult.CustomerNotFound => Results.NotFound(),
                    DeleteCustomerResult.Success successResult => Results.Ok(successResult.CustomerId),
                    _ => Results.InternalServerError("Unknown error.")
                };
            })
            .WithName("DeleteCustomer")
            .RequireAuthorization();

        app.MapPost("/customer/{id:guid}/email",
                async (Guid id, UpdateCustomerEmailRequest request, ICommandBus commandBus) =>
                {
                    if (string.IsNullOrWhiteSpace(request.Email))
                    {
                        return Results.BadRequest();
                    }

                    var command = new ChangeCustomerEmailAddressCommand(CustomerId.With(id), request.Email);

                    var result = await commandBus.PublishAsync(command, CancellationToken.None);

                    return result switch
                    {
                        ChangeEmailResult.CustomerNotFound => Results.NotFound("Customer not found."),
                        ChangeEmailResult.NewEmailMustBeDifferentFromTheCurrent => Results.UnprocessableEntity(
                            "New email must be different from the current email."),
                        ChangeEmailResult.Success => Results.Ok(),
                        _ => Results.InternalServerError("Unknown error.")
                    };
                })
            .WithName("UpdateCustomerEmail")
            .RequireAuthorization();
    }
}

public record CreateCustomerRequest(
    string FirstName,
    string? LastName,
    string PhoneNumber,
    string Email,
    string Address,
    string BillingAddress,
    DateTime? DateOfBirth
);

public record UpdateCustomerEmailRequest(string Email);