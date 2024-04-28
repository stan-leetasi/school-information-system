using project.App.Models;
using project.App.ViewModels;

namespace project.App.Services;

public interface INavigationService
{
    public Guid? LoggedInUser { get; }
    IEnumerable<RouteModel> Routes { get; }

    Task GoToAsync<TViewModel>(IDictionary<string, object?> parameters)
        where TViewModel : IViewModel;

    Task GoToAsync(string route);
    bool SendBackButtonPressed();
    Task GoToAsync(string route, IDictionary<string, object?> parameters);

    Task GoToAsync<TViewModel>()
        where TViewModel : IViewModel;

    Task LogIn(Guid? userGuid);

    Task LogOut();
}