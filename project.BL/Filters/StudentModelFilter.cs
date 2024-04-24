using project.BL.Models;

namespace project.BL.Filters;
public class StudentModelFilter : ListModelFilter<StudentListModel>
{
    protected override IEnumerable<StudentListModel> ApplySearchFilterLogic(IEnumerable<StudentListModel> listModels,
        string searchedTerm)
    {
        searchedTerm = searchedTerm.ToLower();
        return listModels.Where(s =>
            s.Name.ToLower().Contains(searchedTerm) ||
            s.Surname.ToLower().Contains(searchedTerm) ||
            (s.Name + " " + s.Surname).ToLower().Contains(searchedTerm) ||
            (s.Surname + " " + s.Name).ToLower().Contains(searchedTerm)
        );
    }
}