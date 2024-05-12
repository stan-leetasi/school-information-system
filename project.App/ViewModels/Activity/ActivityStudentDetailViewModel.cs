using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using project.App.Messages;
using project.App.Services;
using project.BL.Facades;
using project.BL.Models;
using project.Common.Enums;
using System.Diagnostics;

namespace project.App.ViewModels.Activity;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class ActivityStudentDetailViewModel : ViewModelBase, IRecipient<UserLoggedIn>
{
    private readonly IActivityFacade _activityFacade;
    private readonly INavigationService _navigationService;
    
    public Guid Id { get; set; }
    public ActivityStudentDetailModel? Activity { get; set; }

    public ActivityStudentDetailViewModel(
        IActivityFacade activityFacade,
        INavigationService navigationService,
        IMessengerService messengerService)
        : base(messengerService)
    {
        _activityFacade = activityFacade;
        _navigationService = navigationService;
    }


    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();
        // TODO: receive relevant ActivityStudentDetailModel.Id
        // Debug.WriteLine(Id);
        
        // Activity = await _activityFacade.GetAsyncStudentDetail(Id, _navigationService.LoggedInUser, null);

        Activity = new ActivityStudentDetailModel
        {
            BeginTime = DateTime.Now,
            EndTime = DateTime.Now.AddHours(1),
            Description = "Description",
            Area = SchoolArea.Classroom103,
            Type = ActivityType.FinalExam,
            SubjectId = default,
            SubjectName = "Subject name",
            Notes = "There are no notes",
        };
    }

    public async void Receive(UserLoggedIn message)
    {
        await LoadDataAsync();
    }
    
    [RelayCommand]
    private async Task Refresh() => await LoadDataAsync();
    
    // TODO: impelment registration checkbox
}