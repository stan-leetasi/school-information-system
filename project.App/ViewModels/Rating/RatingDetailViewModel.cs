using CommunityToolkit.Mvvm.Input;
using project.App.Services;
using project.BL.Facades;
using project.BL.Models;

namespace project.App.ViewModels.Rating;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class RatingDetailViewModel : ViewModelBase
{
    private readonly IRatingFacade _ratingFacade;
    private readonly INavigationService _navigationService;
    public Guid Id { get; set; }
    public RatingDetailModel? Rating { get; set; }

    public RatingDetailViewModel(IRatingFacade ratingFacade,
        INavigationService navigationService,
        IMessengerService messengerService)
        : base(messengerService)
    {
        _ratingFacade = ratingFacade;
        _navigationService = navigationService;
    }
    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();
        
        Rating = await _ratingFacade.GetAsync(Id);
    }
    
    [RelayCommand]
    private async Task Refresh() => await LoadDataAsync();
    
    [RelayCommand]
    private async Task GoToEditAsync()
    {
        if (Rating is not null)
        {
            // await _navigationService.GoToAsync("/edit",
            //     new Dictionary<string, object?> { [nameof(RatingEditViewModel.Recipe)] = Recipe with { } });
        }
    }
}