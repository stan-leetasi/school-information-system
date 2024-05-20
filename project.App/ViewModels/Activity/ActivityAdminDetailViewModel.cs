using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using project.App.Messages;
using project.App.Services;
using project.App.ViewModels.Rating;
using project.BL.Facades;
using project.BL.Filters;
using project.BL.Models;
using System.Collections.ObjectModel;

namespace project.App.ViewModels.Activity;

[QueryProperty(nameof(ActivityId), nameof(ActivityId))]
public partial class ActivityAdminDetailViewModel(
    IActivityFacade activityFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
    : TableViewModelBase(messengerService), IRecipient<RatingEditMessage>, IRecipient<ActivityEditMessage>
{
    protected override FilterPreferences DefaultFilterPreferences =>
        FilterPreferences.Default with { SortByPropertyName = nameof(RatingListModel.StudentSurname) };

    public ActivityAdminDetailModel Activity { get; private set; } = ActivityAdminDetailModel.Empty;
    public ObservableCollection<RatingListModel> Ratings { get; set; } = [];
    public Guid ActivityId { get; set; }

    protected override async Task LoadDataAsync()
    {
        Activity = await activityFacade.GetAsync(ActivityId, FilterPreferences) ?? ActivityAdminDetailModel.Empty;
        Ratings = Activity.Ratings;
    }

    [RelayCommand]
    private async Task GoToDetailAsync(Guid id)
    {
        RatingListModel rating = Ratings.SingleOrDefault(r => r.Id == id) ??
                                 throw new ArgumentNullException($"Invalid ID of clicked rating");
        await navigationService.GoToAsync<RatingDetailViewModel>(new Dictionary<string, object?>
        {
            [nameof(RatingDetailViewModel.Id)] = rating.Id
        });
    }


    [RelayCommand]
    private async Task GoToEditAsync() =>
        await navigationService.GoToAsync("/editActivity",
            new Dictionary<string, object?>
            {
                [nameof(ActivityEditViewModel.SubjectId)] = Activity.SubjectId,
                [nameof(ActivityEditViewModel.ActivityId)] = ActivityId
            });

    [RelayCommand]
    private async Task Delete()
    {
        await activityFacade.DeleteAsync(ActivityId);
        messengerService.Send(new ActivityEditMessage { ActivityId = ActivityId });
        navigationService.SendBackButtonPressed();
    }

    [RelayCommand]
    private async Task Refresh() => await LoadDataAsync();

    // Sorting

    [RelayCommand]
    private async Task SortBySurname() => await ApplyNewSorting(nameof(RatingListModel.StudentSurname));

    [RelayCommand]
    private async Task SortByName() => await ApplyNewSorting(nameof(RatingListModel.StudentName));

    [RelayCommand]
    private async Task SortByPoints() => await ApplyNewSorting(nameof(RatingListModel.Points));

    public async void Receive(RatingEditMessage message) => await LoadDataAsync();

    public async void Receive(ActivityEditMessage message) => await LoadDataAsync();
}