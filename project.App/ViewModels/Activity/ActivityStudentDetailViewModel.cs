using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using project.App.Messages;
using project.App.Services;
using project.BL.Facades;
using project.BL.Models;

namespace project.App.ViewModels.Activity;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class ActivityStudentDetailViewModel(
    IActivityFacade activityFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService), IRecipient<ActivityEditMessage>, IRecipient<RefreshManual>
{
    public Guid Id { get; set; }
    public ActivityStudentDetailModel? Activity { get; set; }


    protected override async Task LoadDataAsync()
    {
        if (navigationService.LoggedInUser == null) return; // To prevent loading when logged in admin edits an activity.
        Activity = await activityFacade.GetAsyncStudentDetail(Id, navigationService.LoggedInUser);
    }

    [RelayCommand]
    private async Task Refresh() => await LoadDataAsync();

    public async void Receive(ActivityEditMessage message) => await LoadDataAsync();

    public async void Receive(RefreshManual message) => await LoadDataAsync();

}