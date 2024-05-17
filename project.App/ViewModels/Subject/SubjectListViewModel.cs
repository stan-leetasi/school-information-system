using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using project.App.Messages;
using project.App.Services;
using project.BL.Facades;
using project.BL.Filters;
using project.BL.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace project.App.ViewModels.Subject;

public partial class SubjectListViewModel : TableViewModelBase, IRecipient<UserLoggedIn>, IRecipient<RefreshManual>
{
    protected override FilterPreferences DefaultFilterPreferences =>
        FilterPreferences.Default with { SortByPropertyName = nameof(SubjectListModel.Acronym) };

    private readonly ISubjectFacade _subjectFacade;
    private readonly INavigationService _navigationService;
    public ObservableCollection<SubjectListModel> Subjects { get; set; } = [];
    public bool StudentView { get; protected set; }
    public bool AdminView { get; protected set; }

    public SubjectListViewModel(
        ISubjectFacade subjectFacade,
        INavigationService navigationService,
        IMessengerService messengerService) : base(messengerService)
    {
        _subjectFacade = subjectFacade;
        _navigationService = navigationService;

        StudentView = _navigationService.IsStudentLoggedIn;
        AdminView = !_navigationService.IsStudentLoggedIn;
    }

    protected override async Task LoadDataAsync()
    {
        Subjects = (await _subjectFacade.GetAsyncListModels(studentId: _navigationService.LoggedInUser, filterPreferences: FilterPreferences))
            .ToObservableCollection();

        foreach (var subject in Subjects)
        {
            subject.PropertyChanged += HandleSubjectPropertyChanged!;
        }
    }

    private void HandleSubjectPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SubjectListModel.IsRegistered))
        {
            var subject = (SubjectListModel)sender;
            Guid student = _navigationService.LoggedInUser ?? throw new ArgumentNullException();
            if (subject.IsRegistered)
            {
                _subjectFacade.RegisterStudent(subject.Id, student);
            }
            else
            {
                _subjectFacade.UnregisterStudent(subject.Id, student);
            }
        }
    }

    [RelayCommand]
    private Task AddSubject() => Task.CompletedTask;

    [RelayCommand]
    private Task GoToDetailAsync(Guid id) =>
        _navigationService.GoToAsync<SubjectStudentDetailViewModel>(new Dictionary<string, object?> { [nameof(SubjectStudentDetailViewModel.SubjectId)] = id });

    [RelayCommand]
    private async Task SortByAcronym() => await ApplyNewSorting(nameof(SubjectListModel.Acronym));

    [RelayCommand]
    private async Task SortByName() => await ApplyNewSorting(nameof(SubjectListModel.Name));

    [RelayCommand]
    private async Task SortByRegistered() => await ApplyNewSorting(nameof(SubjectListModel.IsRegistered));

    public async void Receive(UserLoggedIn message)
    {
        StudentView = _navigationService.IsStudentLoggedIn;
        AdminView = !_navigationService.IsStudentLoggedIn;
        ResetFilterPreferences();
        await LoadDataAsync();
    }

    public async void Receive(RefreshManual message)
    {
        await LoadDataAsync();
    }
}