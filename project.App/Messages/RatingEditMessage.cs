namespace project.App.Messages;

public record RatingEditMessage
{
    public required Guid RatingId { get; init; }
}