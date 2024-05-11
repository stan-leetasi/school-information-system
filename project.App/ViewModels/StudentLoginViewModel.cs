using project.App.Messages;
using project.App.Services;
using project.BL.Facades;
using project.BL.Models;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;

namespace project.App.ViewModels;

public partial class StudentLoginViewModel : ViewModelBase
{
    private readonly IStudentFacade _studentFacade;
    private readonly INavigationService _navigationService;
    private readonly IMessengerService _messengerService;

    private StudentListModel? _selectedStudent;

    public StudentListModel? SelectedStudent
    {
        get => _selectedStudent;
        set
        {
            _selectedStudent = value;
            OnSelectUser(value);
        }
    }

    public ObservableCollection<StudentListModel> Students { get; set; } = new();

    public StudentLoginViewModel(IStudentFacade studentFacade, INavigationService navigationService,
        IMessengerService messengerService)
        : base(messengerService)
    {
        _studentFacade = studentFacade;
        _navigationService = navigationService;
        _messengerService = messengerService;
        Task.Run(LoadDataAsync);
    }

    private new async Task LoadDataAsync()
    {
        Students = new ObservableCollection<StudentListModel>(await _studentFacade.GetAsync());
    }

    [RelayCommand]
    public void OnSelectUser(StudentListModel? selectedStudent)
    {
        _navigationService.LogIn(selectedStudent?.Id);
        _messengerService.Send(new UserLoggedIn());
    }
}