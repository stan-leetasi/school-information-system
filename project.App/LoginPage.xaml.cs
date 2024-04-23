using CookBook.App.ViewModels;

namespace project.App;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
        BindingContext = new StudentLogins();
    }
    
    void OnPickerSelectedItemChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        var selectedStudent= (string?)picker.SelectedItem;

        if (selectedStudent is not null)
        {
            Application.Current.MainPage = new AppShell();
        }
    }
    
    void OnAdminLogin(object sender, EventArgs e)
    {
        Application.Current.MainPage = new AppShell();
    }
}
