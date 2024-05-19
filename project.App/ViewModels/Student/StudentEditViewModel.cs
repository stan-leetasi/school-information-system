using CommunityToolkit.Mvvm.Input;
using project.App.Services;
using project.BL.Facades;
using project.BL.Models;
using project.App.Messages;

namespace project.App.ViewModels.Student;

[QueryProperty(nameof(StudentId), nameof(StudentId))]
public partial class StudentEditViewModel(
    IStudentFacade studentFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService)
{
    public StudentDetailModel Student { get; set; } = StudentDetailModel.Empty;
    public Guid StudentId { get; init; } = new();
    public string StudentImageUrl { get; set; } = string.Empty;
    public string StudentImageUrlTemp { get; set; } = string.Empty;

    protected override async Task LoadDataAsync()
    {
        StudentDetailModel? getStudent = await studentFacade.GetAsync(StudentId);

        if (getStudent != null)
            Student = getStudent;

        // Display only valid image URLs
        if (Student.ImageUrl != null &&
            !Student.ImageUrl.AbsoluteUri.Equals("about:blank", StringComparison.OrdinalIgnoreCase))
        {
            StudentImageUrl = Student.ImageUrl.ToString();
            StudentImageUrlTemp = StudentImageUrl;
        }
    }

    [RelayCommand]
    private void UpdateImage()
    {
        if(Uri.TryCreate(StudentImageUrlTemp, UriKind.Absolute, out _))
            StudentImageUrl = StudentImageUrlTemp;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        // Save only a valid URL
        Student.ImageUrl = Uri.TryCreate(StudentImageUrl, UriKind.Absolute, out _)
            ? new Uri(StudentImageUrl)
            : new Uri("about:blank");

        await studentFacade.SaveAsync(Student);

        MessengerService.Send(new StudentEditMessage { StudentId = Student.Id });

        await navigationService.GoToAsync<StudentDetailViewModel>(
            new Dictionary<string, object?> { { nameof(StudentDetailViewModel.StudentId), Student.Id } });
    }
}