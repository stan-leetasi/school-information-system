﻿using CommunityToolkit.Maui.Core.Extensions;
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

public partial class SubjectListViewModel(
    ISubjectFacade subjectFacade,
    INavigationService navigationService,
    IMessengerService messengerService,
    IAlertService alertService)
    : TableViewModelBase(messengerService), IRecipient<SubjectEditMessage>, IRecipient<SubjectDeleteMessage>
{
    protected override FilterPreferences DefaultFilterPreferences =>
        FilterPreferences.Default with { SortByPropertyName = nameof(SubjectListModel.Acronym) };

    public ObservableCollection<SubjectListModel> Subjects { get; set; } = [];
    public bool StudentView { get; set; } = navigationService.IsStudentLoggedIn;
    public bool AdminView { get; set; } = !navigationService.IsStudentLoggedIn;
    public string Sorting { get; set; } = string.Empty;
    protected override async Task LoadDataAsync()
    {
        AdminView = !navigationService.IsStudentLoggedIn;
        StudentView = navigationService.IsStudentLoggedIn;
        Subjects = (await subjectFacade.GetAsyncListModels(navigationService.LoggedInUser, FilterPreferences))
            .ToObservableCollection();

        foreach (SubjectListModel subject in Subjects)
            subject.PropertyChanged += HandleSubjectPropertyChanged!;
    }

    private async void HandleSubjectPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(SubjectListModel.IsRegistered))
            return;

        SubjectListModel subject = (SubjectListModel)sender;
        Guid student = navigationService.LoggedInUser ?? throw new ArgumentNullException();
        if (subject.IsRegistered) 
            await subjectFacade.RegisterStudent(subject.Id, student);
        else
        {
            var confirm = await alertService.ConfirmAsync("Unregister subject",
                "You will be also unregistered from all activities.");
            if (confirm)
                await subjectFacade.UnregisterStudent(subject.Id, student);
            else
                await LoadDataAsync();
        }
            
    }

    [RelayCommand]
    private async Task GoToCreateAsync() => await navigationService.GoToAsync("/createSubject");

    [RelayCommand]
    private async Task GoToDetailAsync(Guid id)
    {
        await navigationService.GoToAsync<SubjectStudentDetailViewModel>(new Dictionary<string, object?>
        {
            [nameof(SubjectStudentDetailViewModel.SubjectId)] = id
        });
    }

    [RelayCommand]
    private async Task SortByAcronym()
    {
        await ApplyNewSorting(nameof(SubjectListModel.Acronym));
        Sorting = FilterPreferences.SortByPropertyName;
    }

    [RelayCommand]
    private async Task SortByName()
    {
        await ApplyNewSorting(nameof(SubjectListModel.Name));
        Sorting = FilterPreferences.SortByPropertyName;
    }

    [RelayCommand]
    private async Task SortByRegistered()
    {
        await ApplyNewSorting(nameof(SubjectListModel.IsRegistered));
        Sorting = FilterPreferences.SortByPropertyName;
    }

    public async void Receive(SubjectEditMessage message) => await LoadDataAsync();
    
    public async void Receive(SubjectDeleteMessage message) => await LoadDataAsync();
}