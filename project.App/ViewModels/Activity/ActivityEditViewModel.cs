using CommunityToolkit.Mvvm.Input;
using project.App.Messages;
using project.App.Services;
using project.App.ViewModels.Subject;
using project.BL.Facades;
using project.BL.Models;
using project.Common.Enums;

namespace project.App.ViewModels.Activity;

[QueryProperty(nameof(ActivityId), nameof(ActivityId))]
[QueryProperty(nameof(SubjectId), nameof(SubjectId))]
public partial class ActivityEditViewModel(
    IActivityFacade activityFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService)
{
    public Guid ActivityId { get; init; }
    public Guid SubjectId { get; init; }


    public ActivityAdminDetailModel Activity { get; set; } = ActivityAdminDetailModel.Empty;

    public List<SchoolArea> SchoolAreas { get; set; } = new((SchoolArea[])Enum.GetValues(typeof(SchoolArea)));

    protected override async Task LoadDataAsync()
    {
        if (ActivityId != Guid.Empty)
            Activity = await activityFacade.GetAsync(ActivityId) ?? Activity;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        Activity.Ratings = [];
        Activity.SubjectId = SubjectId;
        await activityFacade.SaveAsync(Activity);
        MessengerService.Send(new SubjectEditMessage { SubjectId = SubjectId });
        if (ActivityId == new Guid())
            await navigationService.GoToAsync<SubjectStudentDetailViewModel>(
                new Dictionary<string, object?> { { nameof(SubjectStudentDetailViewModel.SubjectId), SubjectId } });
        else
            navigationService.SendBackButtonPressed();
    }
}