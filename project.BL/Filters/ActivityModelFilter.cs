using project.BL.Models;

namespace project.BL.Filters;
public class ActivityModelFilter : ListModelFilter<ActivityListModel>
{
    protected override IEnumerable<ActivityListModel> ApplySearchFilterLogic(IEnumerable<ActivityListModel> listModels,
        string searchedTerm)
    {
        searchedTerm = searchedTerm.ToLower();

        return listModels.Where(a =>
            a.BeginTime.ToShortDateString().Replace(" ", "").Contains(searchedTerm) ||
            a.BeginTime.ToShortTimeString().Contains(searchedTerm) ||
            a.EndTime.ToShortDateString().Replace(" ", "").Contains(searchedTerm) ||
            a.EndTime.ToShortTimeString().Contains(searchedTerm)
        );
    }
}