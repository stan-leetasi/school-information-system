using CommunityToolkit.Mvvm.Input;
using project.App.Messages;
using project.App.Services;

namespace project.App;

public partial class AppShell : Shell
{
    private readonly INavigationService _navigationService;
    private readonly IMessengerService _messengerService;
    public AppShell(INavigationService navigationService, IMessengerService messengerService)
    {
        InitializeComponent();

        _navigationService = navigationService;
        _messengerService = messengerService;
    }

    // TODO: change routes in commands
    [RelayCommand]
    private async Task GoToStudents()
        => await _navigationService.GoToAsync("//students");

    [RelayCommand]
    private async Task GoToSubjects()
        => await _navigationService.GoToAsync("//subjects");

    [RelayCommand]
    private async Task GoToHome()
        => await _navigationService.GoToAsync("//home");

    [RelayCommand]
    private void Refresh()
        => _messengerService.Send(new RefreshManual());

    [RelayCommand]
    private async Task LogOut()
        => await _navigationService.LogOut();
}
