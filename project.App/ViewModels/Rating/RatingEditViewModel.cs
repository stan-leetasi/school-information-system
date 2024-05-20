using CommunityToolkit.Mvvm.Input;
using project.App.Messages;
using project.App.Services;
using project.BL.Facades;
using project.BL.Models;

namespace project.App.ViewModels.Rating;

[QueryProperty(nameof(RatingId), nameof(RatingId))]
public partial class RatingEditViewModel(
    IRatingFacade ratingFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService)
{
    public Guid RatingId { get; init; } = new();

    public RatingDetailModel Rating { get; set; } = RatingDetailModel.Empty;

    protected override async Task LoadDataAsync()
    {
        RatingDetailModel? getRating = await ratingFacade.GetAsync(RatingId);

        if (getRating != null)
            Rating = getRating;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        await ratingFacade.SaveAsync(Rating);

        MessengerService.Send(new RatingEditMessage() { RatingId = Rating.Id });

        navigationService.SendBackButtonPressed();
    }
}
