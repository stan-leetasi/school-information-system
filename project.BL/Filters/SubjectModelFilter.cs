using project.BL.Models;

namespace project.BL.Filters;
public class SubjectModelFilter : ListModelFilter<SubjectListModel>
{
    protected override IEnumerable<SubjectListModel> ApplySearchFilterLogic(IEnumerable<SubjectListModel> listModels,
        string searchedTerm)
    {
        return listModels.Where(s =>
            s.Acronym.ToLower().Contains(searchedTerm) ||
            s.Name.ToLower().Contains(searchedTerm)
        );
    }
}