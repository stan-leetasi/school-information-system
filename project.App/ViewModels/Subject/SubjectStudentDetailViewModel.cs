using project.App.Services;
using project.BL.Facades;
using project.BL.Filters;
using project.BL.Models;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
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
    : TableViewModelBase(messengerService) // TODO , IRecipient<ActivityEditMessage>
{
    protected override FilterPreferences DefaultFilterPreferences =>
        FilterPreferences.Default with { SortByPropertyName = nameof(ActivityListModel.BeginTime) };

    public bool StudentView => navigationService.IsStudentLoggedIn;
    public bool AdminView => !navigationService.IsStudentLoggedIn;
    public Guid SubjectId { get; set; }
    public string Title { get; private set; } = string.Empty;
    public ObservableCollection<ActivityListModel> Activities { get; set; } = [];

    protected override async Task LoadDataAsync()
    {
        SubjectStudentDetailModel subject =
            await subjectFacade.GetAsyncStudentDetail(SubjectId, navigationService.LoggedInUser, FilterPreferences) ??
            SubjectStudentDetailModel.Empty;

        Activities = subject.Activities.ToObservableCollection();
        Title = subject.Acronym + " - " + subject.Name;

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

    // TODO public async void Receive(ActivityEditMessage message) => await LoadDataAsync();
}