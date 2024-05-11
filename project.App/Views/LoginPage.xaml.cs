using project.App.ViewModels;

namespace project.App.Views;

public partial class LoginPage
{
    public LoginPage(StudentLoginViewModel viewModel)
        : base(viewModel)
    {
        InitializeComponent();
    }
}