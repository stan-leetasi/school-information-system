using CommunityToolkit.Mvvm.Input;
using project.App.Messages;
using project.App.Services;
using project.BL.Facades;
using project.BL.Models;
using System.Numerics;

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
    public string RatingPoints { get; set; } = "0";
    public bool InvalidRating { get; set; } = false;

    protected override async Task LoadDataAsync()
    {
        RatingDetailModel? getRating = await ratingFacade.GetAsync(RatingId);

        if (getRating != null)
        {
            Rating = getRating;
            RatingPoints = Rating.Points.ToString();
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        InvalidRating= false;

        if (int.TryParse(RatingPoints, out _))
        {
            Rating.Points = int.Parse(RatingPoints);

            await ratingFacade.SaveAsync(Rating);

            MessengerService.Send(new RatingEditMessage() { RatingId = Rating.Id });

            navigationService.SendBackButtonPressed();
        }
        else
        {
            InvalidRating = true;
        }
        
    }
}
