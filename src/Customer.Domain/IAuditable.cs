namespace Customer.Domain;

public interface IAuditable
{
    DateTimeOffset? CreatedAt { get; }
    DateTimeOffset? ModifiedAt { get; }
    DateTimeOffset? DeletedAt { get; }
    string? ActionUserId { get; }
}