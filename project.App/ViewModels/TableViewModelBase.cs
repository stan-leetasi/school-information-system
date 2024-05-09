﻿using project.App.Services;
using project.BL.Filters;
using System.ComponentModel;

namespace project.App.ViewModels;

public abstract class TableViewModelBase : ViewModelBase
{
    protected virtual FilterPreferences DefaultFilterPreferences => FilterPreferences.Default;
    public FilterPreferences FilterPreferences { get; set; } = FilterPreferences.Default;
    protected TableViewModelBase(IMessengerService messengerService)
        : base(messengerService)
    {
        ResetFilterPreferences();
    }

    protected void ResetFilterPreferences()
    {
        FilterPreferences = DefaultFilterPreferences;
        FilterPreferences.PropertyChanged += HandleSearchBarChange!;
    }

    protected async void HandleSearchBarChange(object sender, PropertyChangedEventArgs e) => await LoadDataAsync();

    protected async Task ApplyNewSorting(string propertyName)
    {
        if (FilterPreferences.SortByPropertyName == propertyName)
        {
            FilterPreferences.DescendingOrder = !FilterPreferences.DescendingOrder;
        }
        else
        {
            FilterPreferences.SortByPropertyName = propertyName;
            FilterPreferences.DescendingOrder = false;
        }
        await LoadDataAsync();
    }
}
