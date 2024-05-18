﻿using project.App.Models;
using project.App.ViewModels;
using project.App.ViewModels.Activity;
using project.App.ViewModels.Student;
using project.App.ViewModels.Subject;
using project.App.Views;
using project.App.Views.Activity;
using project.App.Views.Student;
using project.App.Views.Subject;

namespace project.App.Services;

public class NavigationService : INavigationService
{
    public Guid? LoggedInUser { get; protected set; } = null;
    public bool IsStudentLoggedIn => LoggedInUser != null;

    public IEnumerable<RouteModel> Routes { get; } = new List<RouteModel>
    {
        new("//home", typeof(MainPage), typeof(ViewModelBase)),
        new("//login", typeof(LoginPage), typeof(StudentLoginViewModel)),
        new("//students", typeof(StudentListView), typeof(StudentListViewModel)),
        new("//subjects", typeof(SubjectListView), typeof(SubjectListViewModel)),
        new("//subjects/detail", typeof(SubjectStudentDetailView), typeof(SubjectStudentDetailViewModel)),
        new("//subjects/detail/admin", typeof(SubjectAdminDetailView), typeof(SubjectAdminDetailViewModel)),
        new("//subjects/detail/activity", typeof(ActivityStudentDetailView), typeof(ActivityStudentDetailViewModel)),
        new("//subjects/detail/activity-admin", typeof(ActivityAdminDetailView), typeof(ActivityAdminDetailViewModel)),
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
        await GoToAsync("//home");
    }

    public async Task LogOut()
    {
        await GoToAsync("//login");
    }
}