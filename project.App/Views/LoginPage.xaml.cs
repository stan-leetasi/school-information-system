using project.App.Messages;
using project.App.ViewModels;
using project.App.Services;
using project.BL.Facades;

namespace project.App.Views;

public partial class LoginPage : ContentPage
{
    private readonly INavigationService _navigationService;
    private readonly IMessengerService _messengerService;

    public LoginPage(INavigationService navigationService, IMessengerService messengerService, IStudentFacade studentFacade)
    {
        InitializeComponent();
        BindingContext = new StudentLogins(studentFacade);
        _navigationService = navigationService;
        _messengerService = messengerService;
    }
    
    void OnPickerSelectedItemChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        var selectedStudent= (string?)picker.SelectedItem;

        if (selectedStudent != null && Application.Current != null)
        {
            _navigationService.LogIn(Guid.Empty); // TODO: correct Guid of the student trying to log in
            _messengerService.Send(new UserLoggedIn());
        }
    }
    
    void OnAdminLogin(object sender, EventArgs e)
    {
        if (Application.Current == null) return;
        _navigationService.LogIn(null); // userGuid = null ... admin
        _messengerService.Send(new UserLoggedIn());
    }
}
