using project.BL.Models;

namespace project.BL.Filters;
public class RatingModelFilter : ListModelFilter<RatingListModel>
{
    protected override IEnumerable<RatingListModel> ApplySearchFilterLogic(IEnumerable<RatingListModel> listModels,
        string searchedTerm)
    {
        searchedTerm = searchedTerm.ToLower();
        return listModels.Where(s =>
            s.StudentName.ToLower().Contains(searchedTerm) ||
            s.StudentSurname.ToLower().Contains(searchedTerm) ||
            (s.StudentName + " " + s.StudentSurname).ToLower().Contains(searchedTerm) ||
            (s.StudentSurname + " " + s.StudentName).ToLower().Contains(searchedTerm)
        );
    }
}