using Customer.Domain.Aggregate;
using Customer.Domain.Result;
using OpenMind.DDD.BuildingBlocks;

namespace Customer.Domain.Specification;

public class ChangeEmailAddressSpecification(string newEmail) : Specification<ChangeEmailResult?, CustomerWriteModel>
{
    protected override ChangeEmailResult? IsNotSatisfiedBecause(CustomerWriteModel state)
    {
        if (state.Email == newEmail)
        {
            return new ChangeEmailResult.NewEmailIsTheSameAsCurrent();
        }

        return null;
    }
}
