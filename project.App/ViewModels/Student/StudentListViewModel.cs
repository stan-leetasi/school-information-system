using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using project.App.Messages;
using project.App.Services;
using project.BL.Facades;
using project.BL.Filters;
using project.BL.Models;
using System.Collections.ObjectModel;

namespace project.App.ViewModels.Student;

public partial class StudentListViewModel(
    IStudentFacade studentFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
    : TableViewModelBase(messengerService), IRecipient<StudentEditMessage>
{
    protected override FilterPreferences DefaultFilterPreferences =>
        FilterPreferences.Default with { SortByPropertyName = nameof(StudentListModel.Name) };

    public ObservableCollection<StudentListModel> Students { get; set; } = [];
    public bool AdminView { get; set; } = !navigationService.IsStudentLoggedIn;
    public string Sorting { get; set; } = string.Empty;

    protected override async Task LoadDataAsync()
    {
        Students = (await studentFacade.GetAsync(FilterPreferences)).ToObservableCollection();
        AdminView = !navigationService.IsStudentLoggedIn;
    }
    // Commands

    [RelayCommand]
    private async Task Refresh() => await LoadDataAsync();

    public async void Receive(StudentEditMessage message) => await LoadDataAsync();

    // Navigation

    [RelayCommand]
    private Task GoToDetailAsync(Guid id) =>
        navigationService.GoToAsync<StudentDetailViewModel>(
            new Dictionary<string, object?> { { nameof(StudentDetailViewModel.StudentId), id } });

    [RelayCommand]
    private async Task GoToCreateAsync() => await navigationService.GoToAsync("/edit");

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
}