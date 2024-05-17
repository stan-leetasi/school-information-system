using project.App.Services;
using project.BL.Facades;
using project.BL.Filters;
using project.BL.Models;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using project.App.Messages;
using project.App.ViewModels.Activity;
using System.ComponentModel;

namespace project.App.ViewModels.Subject;

[QueryProperty(nameof(SubjectId), nameof(SubjectId))]
public partial class SubjectStudentDetailViewModel : TableViewModelBase, IRecipient<UserLoggedIn>
{
    protected override FilterPreferences DefaultFilterPreferences =>
        FilterPreferences.Default with { SortByPropertyName = nameof(ActivityListModel.BeginTime) };

    private readonly ISubjectFacade _subjectFacade;
    private readonly INavigationService _navigationService;

    public SubjectStudentDetailModel? Subject { get; set; }
    public bool StudentView { get; protected set; }
    public Guid SubjectId { get; set; }
    public ObservableCollection<ActivityListModel>? Activities { get; set; }

    public string Title { get; set; }

    public SubjectStudentDetailViewModel(
        ISubjectFacade subjectFacade,
        INavigationService navigationService,
        IMessengerService messengerService) : base(messengerService)
    {
        _subjectFacade = subjectFacade;
        _navigationService = navigationService;
        StudentView = _navigationService.IsStudentLoggedIn;
        Title = "";
    }

    protected override async Task LoadDataAsync()
    {
        Subject = await _subjectFacade.GetAsyncStudentDetail(SubjectId, _navigationService.LoggedInUser);
        Activities = Subject?.Activities.ToObservableCollection();
        Title = Subject?.Acronym + " - " + Subject?.Name;
    }

    private void HandleSubjectPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SubjectListModel.IsRegistered))

        {
            var subject = (SubjectListModel)sender;
            // Guid student = navigationService.LoggedInUser ?? throw new ArgumentNullException();
            if (subject.IsRegistered)
            {
                // subjectFacade.RegisterStudent(subject.Id, student);
            }
            else
            {
                // subjectFacade.UnregisterStudent(subject.Id, student);
            }
        }
    }

    [RelayCommand]
    private async Task Refresh() => await LoadDataAsync();

    public async void Receive(UserLoggedIn message)
    {
        StudentView = _navigationService.IsStudentLoggedIn;
        ResetFilterPreferences();
        await LoadDataAsync();
    }

    // Navigation

    [RelayCommand]
    private Task GoToDetailAsync(Guid id) =>
        _navigationService.GoToAsync<ActivityStudentDetailViewModel>(new Dictionary<string, object?> { ["Id"] = id });

    // Sorting

    [RelayCommand]
    private async Task SortByBeginTime() => await ApplyNewSorting(nameof(ActivityListModel.BeginTime));

    [RelayCommand]
    private async Task SortByEndTime() => await ApplyNewSorting(nameof(ActivityListModel.EndTime));

    [RelayCommand]
    private async Task SortByArea() => await ApplyNewSorting(nameof(ActivityListModel.Area));

    [RelayCommand]
    private async Task SortByType() => await ApplyNewSorting(nameof(ActivityListModel.Type));

    [RelayCommand]
    private async Task SortByRegisteredStudents() =>
        await ApplyNewSorting(nameof(ActivityListModel.RegisteredStudents));

    [RelayCommand]
    private async Task SortByPoints() => await ApplyNewSorting(nameof(ActivityListModel.Points));

    [RelayCommand]
    private async Task SortByRegistered() => await ApplyNewSorting(nameof(ActivityListModel.IsRegistered));
}