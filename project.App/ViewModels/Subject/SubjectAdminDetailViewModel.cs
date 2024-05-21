using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using project.App.Messages;
using project.App.Services;
using project.BL.Facades;
using project.BL.Filters;
using project.BL.Models;
using System.Collections.ObjectModel;

namespace project.App.ViewModels.Subject;

[QueryProperty(nameof(SubjectId), nameof(SubjectId))]
public partial class SubjectAdminDetailViewModel(
    ISubjectFacade subjectFacade,
    INavigationService navigationService,
    IMessengerService messengerService) : TableViewModelBase(messengerService), IRecipient<RatingEditMessage>,
    IRecipient<StudentEditMessage>
{
    protected override FilterPreferences DefaultFilterPreferences =>
        FilterPreferences.Default with { SortByPropertyName = nameof(StudentListModel.Name) };

    // private readonly INavigationService _navigationService;
    public Guid SubjectId { get; set; }
    public ObservableCollection<StudentListModel>? Students { get; set; } = [];
    public string Title { get; set; } = string.Empty;
    public string Sorting { get; set; } = string.Empty;
    protected override async Task LoadDataAsync()
    {
        SubjectAdminDetailModel subject = await subjectFacade.GetAsync(SubjectId, FilterPreferences) ??
                                          SubjectAdminDetailModel.Empty;
        Students = subject.Students;
        Title = subject.Acronym + " - " + subject.Name;
    }

    [RelayCommand]
    private async Task Refresh() => await LoadDataAsync();

    // Navigation

    [RelayCommand]
    private async Task GoToDetail(Guid studentId) =>
        await navigationService.GoToAsync("//subjects/detail/admin/students",
            new Dictionary<string, object?> { { "StudentId", studentId } });

    // Sorting

    [RelayCommand]
    private async Task SortBySurname()
    {
        await ApplyNewSorting(nameof(StudentListModel.Surname));
        Sorting = FilterPreferences.SortByPropertyName;
    }

    [RelayCommand]
    private async Task SortByName() 
    {
        await ApplyNewSorting(nameof(StudentListModel.Name));
        Sorting = FilterPreferences.SortByPropertyName;
    }

    public async void Receive(RatingEditMessage message) => await LoadDataAsync();

    public async void Receive(StudentEditMessage message) => await LoadDataAsync();
}