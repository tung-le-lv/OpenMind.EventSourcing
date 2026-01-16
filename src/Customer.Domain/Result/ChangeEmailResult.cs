namespace Customer.Domain.Result;

public abstract record ChangeEmailResult
{
    public sealed record Success : ChangeEmailResult;

    public sealed record NewEmailIsTheSameAsCurrent : ChangeEmailResult;
}
