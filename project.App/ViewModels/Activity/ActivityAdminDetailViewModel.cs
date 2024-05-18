using CommunityToolkit.Mvvm.Messaging;
using project.App.Messages;
using project.App.Services;
using project.BL.Facades;
using project.BL.Models;
using System.Collections.ObjectModel;

namespace project.App.ViewModels.Activity;

[QueryProperty(nameof(ActivityId), nameof(ActivityId))]
public partial class ActivityAdminDetailViewModel(
    IActivityFacade activityFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
    : TableViewModelBase(messengerService)
{
    public ActivityAdminDetailModel? Activity { get; set; }
    public ObservableCollection<RatingListModel>? Ratings { get; set; }

    public Guid ActivityId { get; set; }
    public string? Title { get; set; }

    protected override async Task LoadDataAsync()
    {
        Activity = await activityFacade.GetAsync(ActivityId);
        Ratings = Activity?.Ratings;
        //Title = Activity?.Acronym + " - " + Subject?.Name;
    }
}