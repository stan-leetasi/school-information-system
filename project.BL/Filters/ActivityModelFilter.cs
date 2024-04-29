using project.BL.Models;
using System.Linq;

namespace project.BL.Filters;
public class ActivityModelFilter : ListModelFilter<ActivityListModel>
{
    /// <remarks>
    /// Supports 4 kinds of dateTime formats
    /// </remarks>
    protected override IEnumerable<ActivityListModel> ApplySearchFilterLogic(IEnumerable<ActivityListModel> listModels,
        string searchedTerm)
    {
        // Unify dateTime format
        searchedTerm = searchedTerm.Trim();

        if (searchedTerm.Contains('/'))
            searchedTerm = searchedTerm.Replace('/', '.');

        if (searchedTerm.Contains('-'))
            searchedTerm = searchedTerm.Replace('-', '.');


        if (searchedTerm.Count(c => c == ' ') > 1 && searchedTerm.Contains(':'))
        {
            searchedTerm = searchedTerm.Replace(' ', '.');

            if (searchedTerm.Contains(':') && searchedTerm.LastIndexOf('.') != -1)
            {
                searchedTerm = searchedTerm.Substring(0, searchedTerm.LastIndexOf('.')) + ' ' + searchedTerm.Substring(searchedTerm.LastIndexOf('.') + 1);
            }
        }

        if(searchedTerm.Contains(' ') && !searchedTerm.Contains(':'))
            searchedTerm = searchedTerm.Replace(' ', '.');


        return listModels.Where(a =>
            a.BeginTime.ToString("dd.MM.yyyy HH:mm").Contains(searchedTerm) ||
            a.BeginTime.ToString("d.M.yyyy H:mm").Contains(searchedTerm) ||
            a.EndTime.ToString("dd.MM.yyyy HH:mm").Contains(searchedTerm) ||
            a.EndTime.ToString("d.M.yyyy H:mm").Contains(searchedTerm)
        );
    }
}