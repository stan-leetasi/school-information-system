using project.App.ViewModels.Student;

namespace project.App.Views.Student;

public partial class StudentEditView 
{
	public StudentEditView(StudentEditViewModel viewModel) 
        : base(viewModel)
	{
		InitializeComponent();
	}
}