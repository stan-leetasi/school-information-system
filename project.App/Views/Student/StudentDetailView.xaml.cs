using project.App.ViewModels.Student;

namespace project.App.Views.Student;

public partial class StudentDetailView
{
    public StudentDetailView(StudentDetailViewModel viewModel)
        : base(viewModel)
    {
        InitializeComponent();
    }
}