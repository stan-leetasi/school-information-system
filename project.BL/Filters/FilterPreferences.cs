namespace project.BL.Filters;

public record FilterPreferences
{
    /// <summary>
    /// The value in the search bar.
    /// </summary>
    /// <remarks>
    /// It can be a student's name, acronym of a subject, date of activity,...
    /// </remarks>
    public string SearchedTerm = "";
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
}
