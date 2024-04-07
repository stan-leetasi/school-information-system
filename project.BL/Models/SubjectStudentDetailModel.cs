using System.Collections.ObjectModel;

namespace project.BL.Models;

public record SubjectStudentDetailModel : ModelBase
{
    public required string Name { get; set; }
    public required string Acronym { get; set; }

    public ObservableCollection<ActivityListModel> Activities { get; set; } = new();

    public static SubjectStudentDetailModel Empty => new() { Name = string.Empty, Acronym = string.Empty, };
}
