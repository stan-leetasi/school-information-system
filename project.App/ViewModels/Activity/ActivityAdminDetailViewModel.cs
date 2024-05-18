using CommunityToolkit.Mvvm.Messaging;
using project.App.Messages;
using project.App.Services;
using project.BL.Facades;
using project.BL.Models;
using System.Collections.ObjectModel;

namespace project.App.ViewModels.Activity;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class ActivityAdminDetailViewModel :
    TableViewModelBase, IRecipient<UserLoggedIn>, IRecipient<RefreshManual>
{
    private readonly IActivityFacade _facade;
    private readonly INavigationService _navigationService;


    public ActivityAdminDetailModel? Activity { get; set; }
    public ObservableCollection<RatingListModel>? Ratings { get; set; }

    public Guid Id { get; set; }
    public string? Title { get; set; }

    public ActivityAdminDetailViewModel(
        IActivityFacade activityFacade,
        INavigationService navigationService,
        IMessengerService messengerService) : base(messengerService)
    {
        _facade = activityFacade;
        _navigationService = navigationService;
    }

    protected override async Task LoadDataAsync()
    {
        Activity = await _facade.GetAsync(Id);
        Ratings = Activity?.Ratings;
        //Title = Activity?.Acronym + " - " + Subject?.Name;
    }

    public async void Receive(UserLoggedIn message)
    {
        ResetFilterPreferences();
        await LoadDataAsync();
    }

    public async void Receive(RefreshManual message)
    {
        await LoadDataAsync();
    }
}