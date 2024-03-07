namespace project.DAL.Entities;

public record RatingEntity : IEntity
{
    public required Guid Id { get; set; }

    public required ushort Points { get; set; }
    public required string Notes { get; set; }

    public required Guid ActivityId { get; set; }
    public required ActivityEntity? Activity { get; init; }

    public required Guid StudentId { get; set; }
    public required StudentEntity? Student { get; init; }
}