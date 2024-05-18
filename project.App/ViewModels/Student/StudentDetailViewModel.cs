using project.App.Services;
using project.BL.Facades;
using project.BL.Models;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using project.App.Messages;
using System.ComponentModel;
using project.DAL.Seeds;

namespace project.App.ViewModels.Student;

[QueryProperty(nameof(StudentId), nameof(StudentId))]
public partial class StudentDetailViewModel : ViewModelBase
{
    public StudentDetailModel? Student { get; set; }
    public Guid StudentId { get; set; } = new Guid();
    public bool StudentView => _navigationService.IsStudentLoggedIn;
    private readonly IStudentFacade _studentFacade;
    private readonly INavigationService _navigationService;

    public StudentDetailViewModel(IStudentFacade studentFacade,
        INavigationService navigationService,
        IMessengerService messengerService) : base(messengerService)
    {
        _studentFacade = studentFacade;
        _navigationService = navigationService;
    }

    protected override async Task LoadDataAsync()
    {
        Student = await _studentFacade.GetAsync(StudentSeeds.Hawkeye.Id);
    }

    [RelayCommand]
    private async Task EditStudent()
    {
        //await _navigationService.GoToAsync<>;
    }

    [RelayCommand]
    private async Task DeleteStudent()
    {
        await _studentFacade.DeleteAsync(StudentId);
        // await _navigationService.GoToAsync<>;
    }
}