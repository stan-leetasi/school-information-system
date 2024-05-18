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
public partial class SubjectStudentDetailViewModel :
    TableViewModelBase, IRecipient<UserLoggedIn>, IRecipient<RefreshManual>
{
    protected override FilterPreferences DefaultFilterPreferences =>
        FilterPreferences.Default with { SortByPropertyName = nameof(ActivityListModel.BeginTime) };

    private readonly ISubjectFacade _subjectFacade;
    private readonly IActivityFacade _activityFacade;
    private readonly INavigationService _navigationService;
    public bool StudentView => _navigationService.IsStudentLoggedIn;
    public bool AdminView => !_navigationService.IsStudentLoggedIn;
    public Guid SubjectId { get; set; }
    public string Title { get; private set; } = string.Empty;
    public SubjectStudentDetailModel Subject { get; private set; } = SubjectStudentDetailModel.Empty;
    public ObservableCollection<ActivityListModel> Activities { get; set; } = [];

    public SubjectStudentDetailViewModel(
        ISubjectFacade subjectFacade,
        IActivityFacade activityFacade,
        INavigationService navigationService,
        IMessengerService messengerService) : base(messengerService)
    {
        _subjectFacade = subjectFacade;
        _activityFacade = activityFacade;
        _navigationService = navigationService;
    }

    protected override async Task LoadDataAsync()
    {
        Subject = await _subjectFacade.GetAsyncStudentDetail(SubjectId, _navigationService.LoggedInUser) ??
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
            Guid studentId = _navigationService.LoggedInUser ?? throw new ArgumentNullException();
            if (activity.IsRegistered)
                _activityFacade.RegisterStudent(activity.Id, studentId);
            else
                _activityFacade.UnregisterStudent(activity.Id, studentId);
        }
    }

    [RelayCommand]
    private async Task Refresh() => await LoadDataAsync();

    public async void Receive(UserLoggedIn message)
    {
        ResetFilterPreferences();
        await LoadDataAsync();
    }

    public async void Receive(RefreshManual message) => await LoadDataAsync();

    // Navigation

    [RelayCommand]
    private async Task GoToDetailAsync(Guid id)
    {
        if (StudentView)
            await _navigationService.GoToAsync<ActivityStudentDetailViewModel>(
                new Dictionary<string, object?> { [nameof(ActivityStudentDetailViewModel.Id)] = id });
        else
            await _navigationService.GoToAsync<ActivityAdminDetailViewModel>(
                new Dictionary<string, object?> { [nameof(ActivityAdminDetailViewModel.Id)] = id });
    }

    [RelayCommand]
    private async Task GoToAdminDetail(Guid id) =>
        await _navigationService.GoToAsync<SubjectAdminDetailViewModel>(
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
    private async Task SortByRegistered() => await ApplyNewSorting(nameof(ActivityListModel.IsRegistered));
}