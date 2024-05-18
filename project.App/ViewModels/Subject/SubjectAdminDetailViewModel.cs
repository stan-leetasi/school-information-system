using CommunityToolkit.Mvvm.Input;
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
    IMessengerService messengerService) : TableViewModelBase(messengerService)
{
    protected override FilterPreferences DefaultFilterPreferences =>
        FilterPreferences.Default with { SortByPropertyName = nameof(ActivityListModel.BeginTime) };

    // private readonly INavigationService _navigationService;
    public Guid SubjectId { get; set; }
    public ObservableCollection<StudentListModel>? Students { get; set; } = [];
    public string Title { get; set; } = string.Empty;

    protected override async Task LoadDataAsync()
    {
        SubjectAdminDetailModel subject = await subjectFacade.GetAsync(SubjectId) ?? SubjectAdminDetailModel.Empty;
        Students = subject.Students;
        Title = subject.Acronym + " - " + subject.Name;
    }

    [RelayCommand]
    private async Task Refresh() => await LoadDataAsync();

    // Sorting

    [RelayCommand]
    private async Task SortBySurname() => await ApplyNewSorting(nameof(StudentListModel.Surname));

    [RelayCommand]
    private async Task SortByName() => await ApplyNewSorting(nameof(StudentListModel.Name));
}