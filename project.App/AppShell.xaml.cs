using CommunityToolkit.Mvvm.Input;
using project.App.Services;

namespace project.App;

public partial class AppShell : Shell
{
    private readonly INavigationService _navigationService;
    public AppShell(INavigationService navigationService)
    {
        InitializeComponent();

        _navigationService = navigationService;
    }

    // TODO: change routes in commands
    [RelayCommand]
    private async Task GoToStudents()
        => await _navigationService.GoToAsync("//students");

    [RelayCommand]
    private async Task GoToSubjects()
        => await _navigationService.GoToAsync("//students");
    [RelayCommand]
    private async Task LogOut()
        => await _navigationService.LogOut();
}
