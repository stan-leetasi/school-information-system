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


    public ActivityAdminDetailModel Activity { get; private set; } = new()
    {
        BeginTime = DateTime.Now,
        EndTime = DateTime.Now,
        Area = SchoolArea.None,
        Type = ActivityType.None,
        SubjectId = Guid.Empty,
        SubjectName = string.Empty,
        Description = string.Empty,
    };

    public List<SchoolArea> SchoolAreasList => Enum.GetValues(typeof(SchoolArea)).Cast<SchoolArea>().ToList();
    public List<ActivityType> TypesList => Enum.GetValues(typeof(ActivityType)).Cast<ActivityType>().ToList();


    protected override async Task LoadDataAsync()
    {
        if (ActivityId != Guid.Empty)
            Activity = await activityFacade.GetAsync(ActivityId) ?? Activity;
    }

    public DateTime BeginDate
    {
        get { return Activity.BeginTime.Date; }
        set { Activity.BeginTime = CombineDateTime(value, BeginTime); }
    }

    public DateTime EndDate
    {
        get { return Activity.EndTime.Date; }
        set { Activity.EndTime = CombineDateTime(value, EndTime); }
    }

    public TimeSpan BeginTime
    {
        get { return Activity.BeginTime.TimeOfDay; }
        set { Activity.BeginTime = CombineDateTime(Activity.BeginTime, value); }
    }

    public TimeSpan EndTime
    {
        get { return Activity.EndTime.TimeOfDay; }
        set { Activity.EndTime = CombineDateTime(Activity.EndTime, value); }
    }

    private static DateTime CombineDateTime(DateTime date, TimeSpan time)
    {
        return new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds);
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