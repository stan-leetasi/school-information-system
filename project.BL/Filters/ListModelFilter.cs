using project.BL.Models;
using System.Reflection;

namespace project.BL.Filters;
public abstract class ListModelFilter<TListModel> : IListModelFilter<TListModel>
where TListModel : IModel
{
    public virtual IEnumerable<TListModel> ApplyFilter(IEnumerable<TListModel> listModels,
        FilterPreferences filterPreferences)
    {
        if (filterPreferences.FilterByTime)
        {
            listModels = ApplyTimeFilterLogic(listModels, filterPreferences.BeginTime, filterPreferences.EndTime);
        }

        if (filterPreferences.SearchedTerm != string.Empty)
        {
            listModels = ApplySearchFilterLogic(listModels, filterPreferences.SearchedTerm);
        }

        if (filterPreferences.SortByPropertyName != string.Empty)
        {
            listModels = SortModels(listModels, filterPreferences.SortByPropertyName, filterPreferences.DescendingOrder);
        }

        return listModels;
    }

    protected abstract IEnumerable<TListModel> ApplySearchFilterLogic(IEnumerable<TListModel> listModels, string searchedTerm);

    protected virtual IEnumerable<TListModel> ApplyTimeFilterLogic(IEnumerable<TListModel> listModels, DateTime begin,
        DateTime end)
    {
        return listModels;
    }

    protected virtual IEnumerable<TListModel> SortModels(IEnumerable<TListModel> models, string sortByPropertyName,
        bool descending)
    {
        PropertyInfo? property = typeof(TListModel).GetProperty(sortByPropertyName) ?? throw new ArgumentException($"{typeof(TListModel).Name} does not have property {sortByPropertyName}.");
        if (descending) return models.OrderByDescending(item => property.GetValue(item));
        else return models.OrderBy(item => property.GetValue(item));
    }
}