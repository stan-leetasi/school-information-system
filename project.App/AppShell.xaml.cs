using CommunityToolkit.Mvvm.Input;
using project.App.Messages;
using project.App.Services;
using project.App.ViewModels.Student;
using project.App.ViewModels.Subject;

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
        => await _navigationService.GoToAsync<StudentListViewModel>();

    [RelayCommand]
    private async Task GoToSubjects()
        => await _navigationService.GoToAsync<SubjectListViewModel>();

    [RelayCommand]
    private async Task GoToAbout()
        => await _navigationService.GoToAsync("//about");

    [RelayCommand]
    private void Refresh()
        => _messengerService.Send(new RefreshManual());

    [RelayCommand]
    private async Task LogOut()
        => await _navigationService.LogOut();
}
