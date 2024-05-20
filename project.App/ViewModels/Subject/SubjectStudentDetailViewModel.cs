using project.App.Services;
using project.BL.Facades;
using project.BL.Filters;
using project.BL.Models;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using project.App.Messages;
using project.App.ViewModels.Activity;
using project.BL;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace project.App.ViewModels.Subject;

[QueryProperty(nameof(SubjectId), nameof(SubjectId))]
public partial class SubjectStudentDetailViewModel(
    ISubjectFacade subjectFacade,
    IActivityFacade activityFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
    : TableViewModelBase(messengerService), IRecipient<StudentEditMessage>, IRecipient<ActivityEditMessage>,
        IRecipient<SubjectEditMessage>
{
    protected override FilterPreferences DefaultFilterPreferences =>
        FilterPreferences.Default with { SortByPropertyName = nameof(ActivityListModel.BeginTime) };

    public bool StudentView { get; set; } = navigationService.IsStudentLoggedIn;
    public bool AdminView { get; set; } = !navigationService.IsStudentLoggedIn;
    public bool AllowActivityRegistration { get; set; }
    public Guid SubjectId { get; set; }
    public string Title { get; private set; } = string.Empty;

    public SubjectAdminDetailModel? SubjectAdminDetailModel { get; set; }
    public ObservableCollection<ActivityListModel> Activities { get; set; } = [];

    public DateTime BeginDate
    {
        get { return FilterPreferences.BeginTime.Date; }
        set { FilterPreferences.BeginTime = CombineDateTime(value, BeginTime); }
    }

    public DateTime EndDate
    {
        get { return FilterPreferences.EndTime.Date; }
        set { FilterPreferences.EndTime = CombineDateTime(value, EndTime); }
    }

    public TimeSpan BeginTime
    {
        get { return FilterPreferences.BeginTime.TimeOfDay; }
        set { FilterPreferences.BeginTime = CombineDateTime(FilterPreferences.BeginTime, value); }
    }

    public TimeSpan EndTime
    {
        get { return FilterPreferences.EndTime.TimeOfDay; }
        set { FilterPreferences.EndTime = CombineDateTime(FilterPreferences.EndTime, value); }
    }

    private static DateTime CombineDateTime(DateTime date, TimeSpan time)
    {
        return new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds);
    }

    protected override async Task LoadDataAsync()
    {
        SubjectStudentDetailModel subject =
            await subjectFacade.GetAsyncStudentDetail(SubjectId, navigationService.LoggedInUser, FilterPreferences) ??
            SubjectStudentDetailModel.Empty;

        SubjectAdminDetailModel = await subjectFacade.GetAsync(SubjectId, filterPreferences: null) ??
                                  SubjectAdminDetailModel.Empty;

        AdminView = !navigationService.IsStudentLoggedIn;
        StudentView = navigationService.IsStudentLoggedIn;

        Activities = subject.Activities.ToObservableCollection();
        Title = subject.Acronym + " - " + subject.Name;
        AllowActivityRegistration = subject.IsRegistered;

        foreach (ActivityListModel activity in Activities)
            activity.PropertyChanged += HandleActivityPropertyChanged!;
    }

    private async void HandleActivityPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(ActivityListModel.IsRegistered))
            return;

        ActivityListModel activity = (ActivityListModel)sender;
        Guid studentId = navigationService.LoggedInUser ?? Guid.Empty;
        if (activity.IsRegistered)
            await activityFacade.RegisterStudent(activity.Id, studentId);
        else
            await activityFacade.UnregisterStudent(activity.Id, studentId);
        await LoadDataAsync();
    }

    [RelayCommand]
    private async Task Refresh() => await LoadDataAsync();

    // Navigation

    [RelayCommand]
    private async Task GoToDetailAsync(Guid id)
    {
        ActivityListModel activity = Activities.First(a => a.Id == id);
        if (StudentView && activity.IsRegistered)
            await navigationService.GoToAsync<ActivityStudentDetailViewModel>(
                new Dictionary<string, object?> { [nameof(ActivityStudentDetailViewModel.Id)] = id });
        else if (AdminView)
            await navigationService.GoToAsync<ActivityAdminDetailViewModel>(
                new Dictionary<string, object?> { [nameof(ActivityAdminDetailViewModel.ActivityId)] = id });
    }

    [RelayCommand]
    private async Task GoToAdminDetail(Guid id) =>
        await navigationService.GoToAsync<SubjectAdminDetailViewModel>(
            new Dictionary<string, object?> { [nameof(SubjectAdminDetailViewModel.SubjectId)] = id });

    [RelayCommand]
    private async Task GoToEditAsync()
    {
        await navigationService.GoToAsync<SubjectEditViewModel>(new Dictionary<string, object?>
        {
            [nameof(SubjectEditViewModel.SubjectId)] = SubjectId
        });
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (SubjectAdminDetailModel is not null)
        {
            await subjectFacade.DeleteAsync(SubjectAdminDetailModel.Id);

            MessengerService.Send(new SubjectDeleteMessage());

            navigationService.SendBackButtonPressed();
        }
    }

    [RelayCommand]
    private async Task GoToCreateActivityAsync()
    {
        await navigationService.GoToAsync("/createActivity",
            new Dictionary<string, object?>
            {
                [nameof(ActivityEditViewModel.SubjectId)] = SubjectId,
                [nameof(ActivityEditViewModel.ActivityId)] = Guid.Empty
            });
    }



    // Sorting
    [RelayCommand]
    private async Task SortByRegisteredStudents() =>
        await ApplyNewSorting(nameof(ActivityListModel.RegisteredStudents));

    [RelayCommand]
    private async Task SortByType() => await ApplyNewSorting(nameof(ActivityListModel.Type));

    [RelayCommand]
    private async Task SortByArea() => await ApplyNewSorting(nameof(ActivityListModel.Area));

    [RelayCommand]
    private async Task SortByPoints() => await ApplyNewSorting(nameof(ActivityListModel.Points));

    [RelayCommand]
    private async Task SortByEndTime() => await ApplyNewSorting(nameof(ActivityListModel.EndTime));

    [RelayCommand]
    private async Task SortByBeginTime() => await ApplyNewSorting(nameof(ActivityListModel.BeginTime));

    [RelayCommand]
    private async Task SortByIsRegistered() => await ApplyNewSorting(nameof(ActivityListModel.IsRegistered));

    public async void Receive(ActivityEditMessage message) => await LoadDataAsync();

    public async void Receive(StudentEditMessage message) => await LoadDataAsync();

    public async void Receive(SubjectEditMessage message) => await LoadDataAsync();
}