using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace project.BL.Filters;

public record FilterPreferences : INotifyPropertyChanged
{
    /// <summary>
    /// The value in the search bar.
    /// </summary>
    /// <remarks>
    /// It can be a student's name, acronym of a subject, date of activity,...
    /// </remarks>
    public string SearchedTerm { get; set; } = "";

    /// <summary>
    /// Name of the property according to which the items should be sorted.
    /// </summary>
    public string SortByPropertyName = "";

    public bool DescendingOrder;

    public bool FilterByTime { get; set; }
    public DateTime BeginTime { get; set; }
    public DateTime EndTime { get; set; }

    public static FilterPreferences Default { get; } = new()
    {
        SearchedTerm = "",
        SortByPropertyName = "",
        DescendingOrder = false,
        FilterByTime = false,
        BeginTime = DateTime.Now,
        EndTime = DateTime.Now + TimeSpan.FromDays(7)
    };

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
