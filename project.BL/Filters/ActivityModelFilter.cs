using project.BL.Models;
using project.Common.Enums;
using System.Linq;

namespace project.BL.Filters;
public class ActivityModelFilter : ListModelFilter<ActivityListModel>
{
    private List<string>? areaNames = null;
    private List<string>? activityTypeNames = null;

    private List<string> GetAreaNames()
    {
        if (areaNames != null) return areaNames;
        areaNames = EnumStringList.GetStringList<SchoolArea>(true);
        return areaNames;
    }
    private List<string> GetActivityTypeNames()
    {
        if (activityTypeNames != null) return activityTypeNames;
        activityTypeNames = EnumStringList.GetStringList<ActivityType>(true);
        return activityTypeNames;
    }

    private List<int> FindValidEnumNames(List<string> names, string searchedTerm)
    {
        List<int> validEnumNames = new List<int>();
        for (int i = 0; i < names.Count; i++)
        {
            if (names[i].Contains(searchedTerm.ToLower())) validEnumNames.Add(i);
        }
        return validEnumNames;
    }

    protected override IEnumerable<ActivityListModel> ApplySearchFilterLogic(IEnumerable<ActivityListModel> listModels,
        string searchedTerm)
    {
        // Unify dateTime format
        searchedTerm = searchedTerm.Trim();

        List<int> validAreas = FindValidEnumNames(GetAreaNames(), searchedTerm);
        if (validAreas.Count > 0) listModels = listModels.Where(a => validAreas.Contains((int)a.Area));

        List<int> validTypes = FindValidEnumNames(GetActivityTypeNames(), searchedTerm);
        if (validTypes.Count > 0) listModels = listModels.Where(a => validTypes.Contains((int)a.Type));

        return listModels;
    }

    protected override IEnumerable<ActivityListModel> ApplyTimeFilterLogic(IEnumerable<ActivityListModel> listModels, DateTime begin,
        DateTime end)
    {
        return listModels.Where(a => a.BeginTime >= begin && a.EndTime <= end);
    }
}