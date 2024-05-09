
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using project.App.Messages;
using project.App.Services;
using project.BL.Facades;
using project.BL.Filters;
using project.BL.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace project.App.ViewModels.Subject;

public partial class SubjectListViewModel : ViewModelBase, IRecipient<UserLoggedIn>
{
    private readonly ISubjectFacade _subjectFacade;
    private readonly INavigationService _navigationService;
    public ObservableCollection<SubjectListModel> Subjects { get; set; } = null!;
    public bool StudentView { get; protected set; }
    private readonly FilterPreferences _defaultFilterPreferences = FilterPreferences.Default with { SortByPropertyName = nameof(SubjectListModel.Acronym) };
    public FilterPreferences FilterPreferences { get; set; }

    public SubjectListViewModel(ISubjectFacade subjectFacade,
        INavigationService navigationService,
        IMessengerService messengerService) : base(messengerService)
    {
        _subjectFacade = subjectFacade;
        _navigationService = navigationService;

        StudentView = _navigationService.IsStudentLoggedIn;
        FilterPreferences = _defaultFilterPreferences;
        FilterPreferences.PropertyChanged += HandleSearchBarChange!;
    }

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();
        Subjects = (await _subjectFacade.GetAsyncListModels(studentId: null, filterPreferences: FilterPreferences)).ToObservableCollection();

        foreach (var subject in Subjects)
        {
            subject.PropertyChanged += HandleSubjectPropertyChanged!;
        }
    }

    private void HandleSubjectPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SubjectListModel.IsRegistered))
        {
            var subject = (SubjectListModel)sender;
            // Guid student = navigationService.LoggedInUser ?? throw new ArgumentNullException();
            if (subject.IsRegistered)
            {
                // subjectFacade.RegisterStudent(subject.Id, student);
            }
            else
            {
                // subjectFacade.UnregisterStudent(subject.Id, student);
            }
        }
    }

    private async Task SortList(string propertyName)
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

    private async void HandleSearchBarChange(object sender, PropertyChangedEventArgs e)
    {
        await LoadDataAsync();
    }

    [RelayCommand]
    private async Task Refresh()
    {
        await LoadDataAsync();
    }

    [RelayCommand]
    private Task GoToDetailAsync(Guid id)
    {
        return Task.CompletedTask;
    }

    [RelayCommand]
    private async Task SortByAcronym() => await SortList(nameof(SubjectListModel.Acronym));

    [RelayCommand]
    private async Task SortByName() => await SortList(nameof(SubjectListModel.Name));

    [RelayCommand]
    private async Task SortByRegistered() => await SortList(nameof(SubjectListModel.IsRegistered));

    public void Receive(UserLoggedIn message)
    {
        StudentView = _navigationService.IsStudentLoggedIn;
        FilterPreferences = _defaultFilterPreferences;
        FilterPreferences.PropertyChanged += HandleSearchBarChange!;
    }
}
