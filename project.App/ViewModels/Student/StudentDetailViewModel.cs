using project.App.Services;
using project.BL.Facades;
using project.BL.Models;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using project.App.Messages;
using System.ComponentModel;
using project.DAL.Seeds;
using System;

namespace project.App.ViewModels.Student;

[QueryProperty(nameof(StudentId), nameof(StudentId))]
public partial class StudentDetailViewModel : ViewModelBase
{
    public StudentDetailModel? Student { get; set; } = StudentDetailModel.Empty;
    public Guid StudentId { get; set; } = new Guid();
    public bool StudentView => _navigationService.IsStudentLoggedIn;
    public Uri StudentImageUrl { get; set; } = new Uri("https://i.ibb.co/mJrKVdp/student-photo-placeholder.jpg");
    private readonly IStudentFacade _studentFacade;
    private readonly INavigationService _navigationService;

    public StudentDetailViewModel(IStudentFacade studentFacade,
        INavigationService navigationService,
        IMessengerService messengerService) : base(messengerService)
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
        if (Student.ImageUrl == null || !Student.ImageUrl.AbsoluteUri.Equals("about:blank", StringComparison.OrdinalIgnoreCase))
            StudentImageUrl = Student.ImageUrl;
    }
    
    [RelayCommand]
    private async Task EditStudentInfo()
    {
        await _navigationService.GoToAsync<StudentEditViewModel>(
           new Dictionary<string, object?> { { nameof(StudentId), StudentId } });
    }

    [RelayCommand]
    private async Task DeleteStudent()
    {
        await _studentFacade.DeleteAsync(StudentId);
        await _navigationService.GoToAsync("//students");
    }
}