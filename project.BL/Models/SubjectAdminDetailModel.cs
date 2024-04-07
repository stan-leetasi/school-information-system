using System.Collections.ObjectModel;

namespace project.BL.Models;

public record SubjectAdminDetailModel : ModelBase
{
    public required string Name { get; set; }
    public required string Acronym { get; set; }
    public ObservableCollection<StudentListModel> Students { get; set; } = new();

    public static SubjectAdminDetailModel Empty => new() { Name = string.Empty, Acronym = string.Empty, };
}