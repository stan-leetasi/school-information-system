using CommunityToolkit.Mvvm.Input;
using project.App.Services;
using project.BL.Facades;
using project.BL.Models;

namespace project.App.ViewModels.Rating;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class RatingDetailViewModel(
    IRatingFacade ratingFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService)
{
    public Guid Id { get; set; }
    public RatingDetailModel? Rating { get; set; }

    protected override async Task LoadDataAsync()
    {
        Rating = await ratingFacade.GetAsync(Id);
    }
    
    [RelayCommand]
    private async Task Refresh() => await LoadDataAsync();
    
    [RelayCommand]
    private async Task GoToEditAsync()
    {
        if (Rating is not null)
        {
            // await navigationService.GoToAsync("/edit",
            //     new Dictionary<string, object?> { [nameof(RatingEditViewModel.Recipe)] = Recipe with { } });
        }
    }
}