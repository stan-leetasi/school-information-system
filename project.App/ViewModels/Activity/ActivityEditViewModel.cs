using CommunityToolkit.Mvvm.Input;
using project.App.Messages;
using project.App.Services;
using project.BL.Facades;
using project.BL.Models;
using project.Common.Enums;

namespace project.App.ViewModels.Activity;

[QueryProperty(nameof(Activity), nameof(Activity))]
public partial class ActivityEditViewModel(
    IActivityFacade activityFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService)
{
    public Guid ActivityId { get; init; } = Guid.Empty;

    public ActivityAdminDetailModel Activity { get; set; } = ActivityAdminDetailModel.Empty;

    public List<SchoolArea> SchoolAreas { get; set; } = new((SchoolArea[])Enum.GetValues(typeof(SchoolArea)));
    protected override async Task LoadDataAsync()
    {
        Activity = await activityFacade.GetAsync(ActivityId)
                   ?? ActivityAdminDetailModel.Empty;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        await activityFacade.SaveAsync(Activity);

        MessengerService.Send(new ActivityEditMessage() {ActivityId = Activity.Id});

        navigationService.SendBackButtonPressed();
    }


}
