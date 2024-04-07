using project.Common.Enums;

namespace project.BL.Models;

public record ActivityListModel : ModelBase
{
    public required DateTime BeginTime { get; set; }
    public required DateTime EndTime { get; set; }
    public required SchoolArea Area { get; set; }
    public required ActivityType Type { get; set; }
    public int RegisteredStudents { get; set; }

    // Values depending on the logged-in user:
    public int Points { get; set; }
    public bool IsRegistered { get; set; }

    public static ActivityListModel Empty => new()
    {
        Id = Guid.NewGuid(),
        BeginTime = DateTime.MinValue,
        EndTime = DateTime.MinValue,
        Area = SchoolArea.None,
        Type = ActivityType.None,
        RegisteredStudents = 0,

        Points = 0,
        IsRegistered = false,
    };
}
