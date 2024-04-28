using project.BL.Facades;
using System.Collections.ObjectModel;

namespace project.App.ViewModels;

// TODO: replace with StudentListViewModel
public class StudentLogins
{
    public ObservableCollection<string> Students { get; set; }
    private readonly IStudentFacade _studentFacade;

    public StudentLogins(IStudentFacade studentFacade)
    {
        _studentFacade = studentFacade;
        Students = new ObservableCollection<string>(GetStudentLoginsFromDatabase()); // TODO use Facade
    }

    private List<string> GetStudentLoginsFromDatabase()
    {
        return new List<string> { "xlogin00", "xlogin01", "xlogin02" };
    }
}
