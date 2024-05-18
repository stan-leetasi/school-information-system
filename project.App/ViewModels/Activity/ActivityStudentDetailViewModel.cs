using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using project.App.Messages;
using project.App.Services;
using project.BL.Facades;
using project.BL.Models;
using project.Common.Enums;
using System.Diagnostics;

namespace project.App.ViewModels.Activity;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class ActivityStudentDetailViewModel : ViewModelBase
{
    private readonly IActivityFacade _activityFacade;
    private readonly INavigationService _navigationService;
    
    public Guid Id { get; set; }
    public ActivityStudentDetailModel? Activity { get; set; }

    public ActivityStudentDetailViewModel(
        IActivityFacade activityFacade,
        INavigationService navigationService,
        IMessengerService messengerService)
        : base(messengerService)
    {
        _activityFacade = activityFacade;
        _navigationService = navigationService;
    }


    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();
        Activity = await _activityFacade.GetAsyncStudentDetail(Id, _navigationService.LoggedInUser, null);
    }
    
    [RelayCommand]
    private async Task Refresh() => await LoadDataAsync();
}