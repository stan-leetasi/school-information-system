using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using project.App.Messages;
using project.App.Services;
using project.BL.Facades;
using project.BL.Models;

namespace project.App.ViewModels.Rating;

[QueryProperty(nameof(SubjectName), nameof(SubjectName))]
[QueryProperty(nameof(Id), nameof(Id))]
public partial class RatingDetailViewModel(
    IRatingFacade ratingFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService), IRecipient<RatingEditMessage>
{
    public Guid Id { get; set; }
    public string SubjectName { get; set; } = string.Empty;
    public string StudentNameWhole { get; set; } = string.Empty;
    public RatingDetailModel Rating { get; private set; } = RatingDetailModel.Empty;

    protected override async Task LoadDataAsync()
    {
        Rating = await ratingFacade.GetAsync(Id) ?? Rating;
        StudentNameWhole = "Student: " + Rating.StudentName + " " + Rating.StudentSurname;
    }

    [RelayCommand]
    private async Task Refresh() => await LoadDataAsync();

    [RelayCommand]
    private async Task GoToEditAsync() => await navigationService.GoToAsync<RatingEditViewModel>(
        new Dictionary<string, object?> { [nameof(RatingEditViewModel.RatingId)] = Id });

    public async void Receive(RatingEditMessage message) => await LoadDataAsync();
}