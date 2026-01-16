namespace Customer.Application.Dto;

public record CustomerDto(
    string Id,
    string FirstName,
    string? LastName,
    string EmailAddress,
    string Address,
    string BillingAddress,
    DateTime? DateOfBirth);
