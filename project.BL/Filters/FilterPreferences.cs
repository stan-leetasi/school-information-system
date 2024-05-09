using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace project.BL.Filters;

public record FilterPreferences: INotifyPropertyChanged
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
    public bool DescendingOrder = false;

    public static FilterPreferences Default { get; } = new()
    {
        SearchedTerm = "",
        SortByPropertyName = "",
        DescendingOrder = false
    };

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
