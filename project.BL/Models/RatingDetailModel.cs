namespace project.BL.Models;

public record RatingDetailModel : ModelBase
{
    public required string ActivityName { get; set; }
    public required string StudentSurname { get; set; }
    public required string StudentName { get; set; }
    public int Points { get; set; }
    public required string Notes { get; set; }

    public static RatingDetailModel Empty => new()
    {
        Id = Guid.Empty,
        ActivityName = string.Empty,
        StudentSurname = string.Empty,
        StudentName = string.Empty,
        Points = 0,
        Notes = string.Empty,
    };
}

