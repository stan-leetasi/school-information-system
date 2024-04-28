﻿using project.App.Models;
using project.App.ViewModels;
using project.App.Views;

namespace project.App.Services;

public class NavigationService : INavigationService
{
    public Guid? LoggedInUser { get; protected set; } = null;
    public IEnumerable<RouteModel> Routes { get; } = new List<RouteModel>
    {
        new("//login", typeof(LoginPage), typeof(ViewModelBase)), // TODO: change ViewModels
        new("//students", typeof(MainPage), typeof(ViewModelBase)), // TODO: change ViewModels
    };

    public async Task GoToAsync<TViewModel>()
        where TViewModel : IViewModel
    {
        var route = GetRouteByViewModel<TViewModel>();
        await Shell.Current.GoToAsync(route);
    }

    public async Task GoToAsync<TViewModel>(IDictionary<string, object?> parameters)
        where TViewModel : IViewModel
    {
        var route = GetRouteByViewModel<TViewModel>();
        await Shell.Current.GoToAsync(route, parameters);
    }

    public async Task GoToAsync(string route)
        => await Shell.Current.GoToAsync(route);

    public async Task GoToAsync(string route, IDictionary<string, object?> parameters)
        => await Shell.Current.GoToAsync(route, parameters);

    public bool SendBackButtonPressed()
        => Shell.Current.SendBackButtonPressed();

    private string GetRouteByViewModel<TViewModel>()
        where TViewModel : IViewModel 
        => Routes.First(route => route.ViewModelType == typeof(TViewModel)).Route;

    public async Task LogIn(Guid? userGuid)
    {
        LoggedInUser = userGuid;
        await GoToAsync("//students");
    }

    public async Task LogOut()
    {
        await GoToAsync("//login");
    }

}