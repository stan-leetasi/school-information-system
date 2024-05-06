using project.Common.Enums;
using System.Collections.ObjectModel;

namespace project.BL.Models;

public record ActivityAdminDetailModel : ModelBase
{
    public required DateTime BeginTime { get; set; }
    public required DateTime EndTime { get; set; }
    public required SchoolArea Area { get; set; }
    public required ActivityType Type { get; set; }
    public required string Description { get; set; }
    public required Guid SubjectId { get; set; }
    public required string SubjectName { get; init; }

    public ObservableCollection<RatingListModel> Ratings { get; set; } = new();

    public static ActivityAdminDetailModel Empty => new()
    {
        Id = Guid.Empty,
        BeginTime = DateTime.MinValue,
        EndTime = DateTime.MinValue,
        Area = SchoolArea.None,
        Type = ActivityType.None,
        SubjectId = Guid.Empty,
        SubjectName = string.Empty,
        Description = string.Empty,
    };
}
