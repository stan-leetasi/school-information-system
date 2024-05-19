using CommunityToolkit.Mvvm.Messaging;
using project.App.Messages;
using project.App.Services;
using project.BL.Filters;
using System.ComponentModel;

namespace project.App.ViewModels;

public abstract class TableViewModelBase : ViewModelBase, IRecipient<UserLoggedIn>, IRecipient<RefreshManual>
{
    protected abstract FilterPreferences DefaultFilterPreferences { get; }
    public FilterPreferences FilterPreferences { get; set; } = FilterPreferences.Default;

    protected TableViewModelBase(IMessengerService messengerService)
        : base(messengerService) => ResetFilterPreferences();

    protected void ResetFilterPreferences()
    {
        FilterPreferences = DefaultFilterPreferences;
        FilterPreferences.PropertyChanged += HandleSearchBarChange!;
    }

    private async void HandleSearchBarChange(object sender, PropertyChangedEventArgs e) => await LoadDataAsync();

    protected async Task ApplyNewSorting(string propertyName)
    {
        if (FilterPreferences.SortByPropertyName == propertyName)
            FilterPreferences.DescendingOrder = !FilterPreferences.DescendingOrder;
        else
        {
            FilterPreferences.SortByPropertyName = propertyName;
            FilterPreferences.DescendingOrder = false;
        }

        await LoadDataAsync();
    }

    public async void Receive(UserLoggedIn message)
    {
        ResetFilterPreferences();
        await LoadDataAsync();
    }

    public virtual async void Receive(RefreshManual message) => await LoadDataAsync();
}