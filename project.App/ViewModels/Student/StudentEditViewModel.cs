using CommunityToolkit.Mvvm.Input;
using project.App.Services;
using project.BL.Facades;
using project.BL.Models;
using project.App.Messages;
using System;

namespace project.App.ViewModels.Student;

[QueryProperty(nameof(StudentId), nameof(StudentId))]
public partial class StudentEditViewModel: ViewModelBase
{
    public StudentDetailModel Student { get; set; } = StudentDetailModel.Empty;
    public Guid StudentId { get; set; } = new Guid();
    public string StudentImageUrl { get; set; } = string.Empty;
    private readonly IStudentFacade _studentFacade;
    private readonly INavigationService _navigationService;

    public StudentEditViewModel(IStudentFacade studentFacade,
    INavigationService navigationService,
    IMessengerService messengerService): base(messengerService)
    {
        _studentFacade = studentFacade;
        _navigationService = navigationService;
    }

    protected override async Task LoadDataAsync()
    {
        var getStudent = await _studentFacade.GetAsync(StudentId);

        if (getStudent != null)
            Student = getStudent;

        // Don't display empty URL
        if(Student.ImageUrl == null || !Student.ImageUrl.AbsoluteUri.Equals("about:blank", StringComparison.OrdinalIgnoreCase))
            StudentImageUrl = Student.ImageUrl.ToString();
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        // Save only a valid URL
        if (Uri.TryCreate(StudentImageUrl, UriKind.Absolute, out _))
            Student.ImageUrl = new Uri(StudentImageUrl);
        else
            Student.ImageUrl = new Uri("about:blank");
        
        await _studentFacade.SaveAsync(Student);

        MessengerService.Send(new StudentEditMessage() { StudentId = Student.Id });

        _navigationService.SendBackButtonPressed();
    }

}