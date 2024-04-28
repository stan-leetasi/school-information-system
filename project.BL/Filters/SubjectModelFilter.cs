using project.BL.Models;
using UnidecodeSharpFork;

namespace project.BL.Filters;
public class SubjectModelFilter : ListModelFilter<SubjectListModel>
{
    protected override IEnumerable<SubjectListModel> ApplySearchFilterLogic(IEnumerable<SubjectListModel> listModels,
        string searchedTerm)
    {
        return listModels.Where(s =>
            s.Acronym.Unidecode().ToLower().Contains(searchedTerm.Unidecode().ToLower()) ||
            s.Name.Unidecode().ToLower().Contains(searchedTerm.Unidecode().ToLower())
        );
    }
}