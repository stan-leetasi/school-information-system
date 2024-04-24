using project.BL.Models;

namespace project.BL.Filters;

public interface IListModelFilter<TListModel>
where TListModel: IModel
{
    IEnumerable<TListModel> ApplyFilter(IEnumerable<TListModel> listModels, FilterPreferences filterPreferences);
}