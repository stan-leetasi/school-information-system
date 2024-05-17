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

namespace project.App.ViewModels.Student;

public partial class StudentListViewModel :
    TableViewModelBase, IRecipient<UserLoggedIn>, IRecipient<RefreshManual>
{
    protected override FilterPreferences DefaultFilterPreferences =>
        FilterPreferences.Default with { SortByPropertyName = nameof(StudentListModel.Name) };

    private readonly IStudentFacade _studentFacade;
    private readonly INavigationService _navigationService;
    public ObservableCollection<StudentListModel> Students { get; set; } = [];
    public bool StudentView { get; protected set; }

    public StudentListViewModel(IStudentFacade studentFacade,
        INavigationService navigationService,
        IMessengerService messengerService) : base(messengerService)
    {
        _studentFacade = studentFacade;
        _navigationService = navigationService;


        StudentView = _navigationService.IsStudentLoggedIn;
    }

    protected override async Task LoadDataAsync()
    {
        Students = (await _studentFacade.GetAsync()).ToObservableCollection();

        // foreach (var student in Students)
        // {
        //     student.PropertyChanged += HandleStudentPropertyChanged!;
        // }
    }

// private void HandleStudentPropertyChanged(object sender, PropertyChangedEventArgs e)
// {
//     if (e.PropertyName == nameof(StudentListModel.IsRegistered))
//     {
//         var student = (StudentListModel)sender;
//         // Guid student = navigationService.LoggedInUser ?? throw new ArgumentNullException();
//         if (student.IsRegistered)
//         {
//             // studentFacade.RegisterStudent(student.Id, student);
//         }
//         else
//         {
//             // studentFacade.UnregisterStudent(student.Id, student);
//         }
//     }
// }

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

    public async void Receive(UserLoggedIn message)
    {
        StudentView = _navigationService.IsStudentLoggedIn;
        ResetFilterPreferences();
        await LoadDataAsync();
    }

    public async void Receive(RefreshManual message)
    {
        await LoadDataAsync();
    }
}