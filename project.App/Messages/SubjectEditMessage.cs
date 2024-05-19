namespace project.App.Messages;

public record SubjectEditMessage
{
    public required Guid SubjectId { get; init; }
}