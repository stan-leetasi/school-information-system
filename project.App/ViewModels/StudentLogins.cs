using System.Collections.ObjectModel;

namespace CookBook.App.ViewModels;

// TODO: replace with StudentListViewModel
public class StudentLogins
{
    public ObservableCollection<string> Students { get; set; }

    public StudentLogins()
    {
        Students = new ObservableCollection<string>(GetStudentLoginsFromDatabase());
    }

    private List<string> GetStudentLoginsFromDatabase()
    {
        return new List<string> { "xlogin00", "xlogin01", "xlogin02" };
    }
}
