namespace project.BL.Models;

public record RatingListModel : ModelBase
{
    public required Guid StudentId { get; set; }
    public required string StudentSurname { get; set; }
    public required string StudentName { get; set; }
    public int Points { get; set; }

    public static RatingListModel Empty => new()
    {
        Id = Guid.Empty,
        StudentId = Guid.Empty,
        StudentSurname = string.Empty,
        StudentName = string.Empty,
        Points = 0,
    };
}

