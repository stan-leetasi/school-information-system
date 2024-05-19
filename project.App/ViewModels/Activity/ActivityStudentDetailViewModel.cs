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
public partial class ActivityStudentDetailViewModel(
    IActivityFacade activityFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService) //TODO , IRecipient<ActivityEditMessage>
{
    public Guid Id { get; set; }
    public ActivityStudentDetailModel? Activity { get; set; }


    protected override async Task LoadDataAsync()
    {
        //await base.LoadDataAsync();
        Activity = await activityFacade.GetAsyncStudentDetail(Id, navigationService.LoggedInUser, null);
    }

    [RelayCommand]
    private async Task Refresh() => await LoadDataAsync();

    // TODO public async void Receive(ActivityEditMessage message) => await LoadDataAsync();
}