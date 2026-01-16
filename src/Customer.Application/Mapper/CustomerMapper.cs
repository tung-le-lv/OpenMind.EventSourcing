using Customer.Application.Dto;
using Customer.Domain.Aggregate;

namespace Customer.Application.Mapper;

public static class CustomerMapper
{
    public static CustomerDto ToDto(CustomerReadModel customer)
    {
        return new CustomerDto(
            customer.Id,
            customer.FirstName,
            customer.LastName,
            customer.Email,
            customer.Address,
            customer.BillingAddress,
            customer.DateOfBirth
        );
    }
}
