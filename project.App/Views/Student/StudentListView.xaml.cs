using project.App.ViewModels.Student;

namespace project.App.Views.Student;

public partial class StudentListView
{
    public StudentListView(StudentListViewModel viewModel)
        : base(viewModel)
    {
        InitializeComponent();
    }
}