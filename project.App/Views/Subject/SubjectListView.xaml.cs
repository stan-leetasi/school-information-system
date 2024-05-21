using project.App.ViewModels.Subject;

namespace project.App.Views.Subject;

public partial class SubjectListView
{
    public SubjectListView(SubjectListViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}