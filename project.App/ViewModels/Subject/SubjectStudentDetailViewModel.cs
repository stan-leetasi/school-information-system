using project.App.Services;
using project.BL.Facades;
using project.BL.Filters;
using project.BL.Models;
using CommunityToolkit.Mvvm.Input;
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
    : TableViewModelBase(messengerService)
{
    protected override FilterPreferences DefaultFilterPreferences =>
        FilterPreferences.Default with { SortByPropertyName = nameof(ActivityListModel.BeginTime) };

    public bool StudentView => navigationService.IsStudentLoggedIn;
    public bool AdminView => !navigationService.IsStudentLoggedIn;
    public Guid SubjectId { get; set; }
    public string Title { get; private set; } = string.Empty;
    public SubjectStudentDetailModel Subject { get; private set; } = SubjectStudentDetailModel.Empty;
    public ObservableCollection<ActivityListModel> Activities { get; set; } = [];

    protected override async Task LoadDataAsync()
    {
        Subject = await subjectFacade.GetAsyncStudentDetail(SubjectId, navigationService.LoggedInUser) ??
                  SubjectStudentDetailModel.Empty;
        Activities = Subject.Activities.ToObservableCollection();
        Title = Subject.Acronym + " - " + Subject.Name;

        foreach (var activity in Activities)
            activity.PropertyChanged += HandleActivityPropertyChanged!;
    }

    private void HandleActivityPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ActivityListModel.IsRegistered))
        {
            var activity = (ActivityListModel)sender;
            Guid studentId = navigationService.LoggedInUser ?? throw new ArgumentNullException();
            if (activity.IsRegistered)
                activityFacade.RegisterStudent(activity.Id, studentId);
            else
                activityFacade.UnregisterStudent(activity.Id, studentId);
        }
    }

    [RelayCommand]
    private async Task Refresh() => await LoadDataAsync();

    // Navigation

    [RelayCommand]
    private async Task GoToDetailAsync(Guid id)
    {
        if (StudentView)
            await navigationService.GoToAsync<ActivityStudentDetailViewModel>(
                new Dictionary<string, object?> { [nameof(ActivityStudentDetailViewModel.Id)] = id });
        else
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
}