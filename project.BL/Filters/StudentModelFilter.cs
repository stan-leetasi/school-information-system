using project.BL.Models;
using UnidecodeSharpFork;

namespace project.BL.Filters;
public class StudentModelFilter : ListModelFilter<StudentListModel>
{
    protected override IEnumerable<StudentListModel> ApplySearchFilterLogic(IEnumerable<StudentListModel> listModels,
        string searchedTerm)
    {
        searchedTerm = searchedTerm.Unidecode().ToLower().Trim();

        return listModels.Where(s =>
            s.Name.Unidecode().ToLower().Contains(searchedTerm) ||
            s.Surname.Unidecode().ToLower().Contains(searchedTerm) ||
            (s.Name + " " + s.Surname).Unidecode().ToLower().Contains(searchedTerm) ||
            (s.Surname + " " + s.Name).Unidecode().ToLower().Contains(searchedTerm)
        );
    }
}