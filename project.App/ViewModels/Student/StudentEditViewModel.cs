using CommunityToolkit.Mvvm.Input;
using project.App.Services;
using project.BL.Facades;
using project.BL.Models;
using project.App.Messages;

namespace project.App.ViewModels.Student;

[QueryProperty(nameof(Student), nameof(Student))]
public partial class StudentEditViewModel(
    IStudentFacade studentFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService)
{
    public StudentDetailModel Student { get; init; } = StudentDetailModel.Empty;


    [RelayCommand]
    private async Task SaveAsync()
    {
        await studentFacade.SaveAsync(Student);

        MessengerService.Send(new StudentEditMessage() { StudentId = Student.Id });

        navigationService.SendBackButtonPressed();
    }

}