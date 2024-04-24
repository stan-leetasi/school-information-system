using CookBook.App.ViewModels;

namespace project.App.Views;

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

        if (selectedStudent != null && Application.Current != null)
        {
            Application.Current.MainPage = new AppShell();
        }
    }
    
    void OnAdminLogin(object sender, EventArgs e)
    {
        if (Application.Current == null) return; 
        Application.Current.MainPage = new AppShell();
    }
}
