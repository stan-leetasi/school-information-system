using CommunityToolkit.Mvvm.Input;
using project.App.Services;
using project.BL.Facades;
using project.BL.Models;
using project.App.Messages;

namespace project.App.ViewModels.Subject;

[QueryProperty(nameof(SubjectId), nameof(SubjectId))]
public partial class SubjectEditViewModel(
    ISubjectFacade subjectFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService)
{
    public SubjectAdminDetailModel Subject { get; set; } = SubjectAdminDetailModel.Empty;

    public Guid SubjectId { get; init; } = new();

    protected override async Task LoadDataAsync()
    {
        Subject = await subjectFacade.GetAsync(SubjectId)
                  ?? SubjectAdminDetailModel.Empty;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        Subject.Students = new();

        await subjectFacade.SaveAsync(Subject);

        MessengerService.Send(new SubjectEditMessage() { SubjectId = Subject.Id });

        if (SubjectId == new Guid())
        {
            await navigationService.GoToAsync<SubjectListViewModel>();
        }
        else
        {
            navigationService.SendBackButtonPressed();
        }
    }
}