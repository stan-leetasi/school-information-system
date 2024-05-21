using project.App.Messages;
using project.App.Services;
using project.BL.Facades;
using project.BL.Models;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace project.App.ViewModels;

public partial class StudentLoginViewModel : ViewModelBase, IRecipient<StudentEditMessage>
{
    private readonly IStudentFacade _studentFacade;
    private readonly INavigationService _navigationService;

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
    }

    protected override async Task LoadDataAsync()
    {
        Students = new ObservableCollection<StudentListModel>(await _studentFacade.GetAsync());
    }

    [RelayCommand]
    public void OnSelectUser(StudentListModel? selectedStudent)
    {
        _navigationService.LogIn(selectedStudent?.Id);
        MessengerService.Send(new UserLoggedIn());
    }

    [RelayCommand]
    private async Task OnRefresh()
    {
        _selectedStudent = null;
        await LoadDataAsync();
    }

    public async void Receive(StudentEditMessage message) => await LoadDataAsync();
}