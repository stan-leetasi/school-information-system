using project.Common.Enums;

namespace project.BL.Models;

public record ActivityStudentDetailModel : ModelBase
{
    public required DateTime BeginTime { get; set; }
    public required DateTime EndTime { get; set; }
    public required SchoolArea Area { get; set; }
    public required ActivityType Type { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required Guid SubjectId { get; set; }
    public required string SubjectName { get; init; }

    // Values depending on the logged-in user:
    public bool IsRegistered { get; set; }
    public int Points { get; set; }
    public required string Notes { get; set; }


    public static ActivityStudentDetailModel Empty => new()
    {
        Id = Guid.Empty,
        BeginTime = DateTime.MinValue,
        EndTime = DateTime.MinValue,
        Area = SchoolArea.None,
        Type = ActivityType.None,
        Name = string.Empty,
        Description = string.Empty,
        SubjectId = Guid.Empty,
        SubjectName = string.Empty,

        IsRegistered = false,
        Points = 0,
        Notes = string.Empty,
    };
}
