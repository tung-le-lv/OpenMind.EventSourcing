using EventFlow.Core;

namespace Customer.Domain.Aggregate;

public class CustomerId : Identity<CustomerId>
{
    public CustomerId(string value) : base(value)
    {
    }
}
