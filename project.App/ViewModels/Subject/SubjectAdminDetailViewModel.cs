using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using project.App.Messages;
using project.App.Services;
using project.BL;
using project.BL.Facades;
using project.BL.Filters;
using project.BL.Models;
using System.Collections.ObjectModel;

namespace project.App.ViewModels.Subject;

[QueryProperty(nameof(SubjectId), nameof(SubjectId))]
public partial class SubjectAdminDetailViewModel :
    TableViewModelBase, IRecipient<UserLoggedIn>, IRecipient<RefreshManual>
{
    protected override FilterPreferences DefaultFilterPreferences =>
        FilterPreferences.Default with { SortByPropertyName = nameof(ActivityListModel.BeginTime) };

    private readonly ISubjectFacade _subjectFacade;
    private readonly INavigationService _navigationService;
    public SubjectAdminDetailModel? Subject { get; set; }
    public Guid SubjectId { get; set; }
    public ObservableCollection<StudentListModel>? Students { get; set; } = [];
    public string? Title { get; set; }

    public SubjectAdminDetailViewModel(
        ISubjectFacade subjectFacade,
        INavigationService navigationService,
        IMessengerService messengerService) : base(messengerService)
    {
        _subjectFacade = subjectFacade;
        _navigationService = navigationService;

    }

    [RelayCommand]
    private async Task Refresh() => await LoadDataAsync();

    [RelayCommand]
    private Task AddStudent() => Task.CompletedTask;

    [RelayCommand]
    private Task GoToDetailAsync(Guid id) => Task.CompletedTask;

    [RelayCommand]
    private async Task SortBySurname() => await ApplyNewSorting(nameof(StudentListModel.Surname));

    [RelayCommand]
    private async Task SortByName() => await ApplyNewSorting(nameof(StudentListModel.Name));


    protected override async Task LoadDataAsync()
    {
        Subject = await _subjectFacade.GetAsync(SubjectId);
        Students = Subject?.Students;
        Title = Subject?.Acronym + " - " + Subject?.Name;
    }

    public async void Receive(UserLoggedIn message)
    {
        ResetFilterPreferences();
        await LoadDataAsync();
    }

    public async void Receive(RefreshManual message)
    {
        await LoadDataAsync();
    }
}