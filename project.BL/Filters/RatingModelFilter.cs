using project.BL.Models;
using UnidecodeSharpFork;

namespace project.BL.Filters;
public class RatingModelFilter : ListModelFilter<RatingListModel>
{
    protected override IEnumerable<RatingListModel> ApplySearchFilterLogic(IEnumerable<RatingListModel> listModels,
        string searchedTerm)
    {
        searchedTerm = searchedTerm.Unidecode().ToLower().Trim();

        return listModels.Where(s =>
            s.StudentName.Unidecode().ToLower().Contains(searchedTerm) ||
            s.StudentSurname.Unidecode().ToLower().Contains(searchedTerm) ||
            (s.StudentName + " " + s.StudentSurname).Unidecode().ToLower().Contains(searchedTerm) ||
            (s.StudentSurname + " " + s.StudentName).Unidecode().ToLower().Contains(searchedTerm)
        );
    }
}