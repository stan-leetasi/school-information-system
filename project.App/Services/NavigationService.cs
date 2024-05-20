using project.App.Models;
using project.App.ViewModels;
using project.App.ViewModels.Activity;
using project.App.ViewModels.Rating;
using project.App.ViewModels.Student;
using project.App.ViewModels.Subject;
using project.App.Views;
using project.App.Views.Activity;
using project.App.Views.Rating;
using project.App.Views.Student;
using project.App.Views.Subject;

namespace project.App.Services;

public class NavigationService : INavigationService
{
    public Guid? LoggedInUser { get; protected set; } = null;
    public bool IsStudentLoggedIn => LoggedInUser != null;

    public IEnumerable<RouteModel> Routes { get; } = new List<RouteModel>
    {
        new("//about", typeof(AboutPage), typeof(ViewModelBase)),
        new("//login", typeof(LoginPage), typeof(StudentLoginViewModel)),
        new("//students", typeof(StudentListView), typeof(StudentListViewModel)),
        new("//students/detail", typeof(StudentDetailView), typeof(StudentDetailViewModel)),
        new("//students/edit",typeof(StudentEditView), typeof(StudentEditViewModel)),
        new("//subjects", typeof(SubjectListView), typeof(SubjectListViewModel)),
        new("//subjects/detail", typeof(SubjectStudentDetailView), typeof(SubjectStudentDetailViewModel)),
        new("//subjects/detail/editSubject",typeof(SubjectEditView), typeof(SubjectEditViewModel)),
        new("//subjects/detail/admin", typeof(SubjectAdminDetailView), typeof(SubjectAdminDetailViewModel)),
        new("//subjects/detail/admin/students", typeof(StudentDetailView), typeof(StudentDetailViewModel)),
        new("//subjects/detail/activity", typeof(ActivityStudentDetailView), typeof(ActivityStudentDetailViewModel)),
        new("//subjects/detail/activity_admin", typeof(ActivityAdminDetailView), typeof(ActivityAdminDetailViewModel)),
        new("//subjects/detail/activity_admin/rating", typeof(RatingDetailView), typeof(RatingDetailViewModel)),
        new("//subjects/detail/activity_admin/rating/edit",typeof(RatingEditView), typeof(RatingEditViewModel)),
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
        await GoToAsync<SubjectListViewModel>();
    }

    public async Task LogOut()
    {
        await GoToAsync<StudentLoginViewModel>();
    }
}