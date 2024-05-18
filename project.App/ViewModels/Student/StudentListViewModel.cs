using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.Input;
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
    : TableViewModelBase(messengerService)
{
    protected override FilterPreferences DefaultFilterPreferences =>
        FilterPreferences.Default with { SortByPropertyName = nameof(StudentListModel.Name) };

    public ObservableCollection<StudentListModel> Students { get; set; } = [];

    protected override async Task LoadDataAsync() =>
        Students = (await studentFacade.GetAsync(FilterPreferences)).ToObservableCollection();

    // Commands

    [RelayCommand]
    private async Task Refresh() => await LoadDataAsync();

    [RelayCommand]
    private Task AddStudent() => Task.CompletedTask;

    // Navigation

    [RelayCommand]
    private Task GoToDetailAsync(Guid id) =>
        navigationService.GoToAsync<ViewModels.Student.StudentDetailViewModel>(
            new Dictionary<string, object?> { { nameof(StudentDetailViewModel.StudentId), id } });

    // Sorting

    [RelayCommand]
    private async Task SortBySurname() => await ApplyNewSorting(nameof(StudentListModel.Surname));

    [RelayCommand]
    private async Task SortByName() => await ApplyNewSorting(nameof(StudentListModel.Name));
}