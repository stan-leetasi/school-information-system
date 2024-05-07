
using CommunityToolkit.Maui.Core.Extensions;
using project.App.Services;
using project.BL.Facades;
using project.BL.Models;
using System.Collections.ObjectModel;

namespace project.App.ViewModels.Subject;

public partial class SubjectListViewModel(
    ISubjectFacade subjectFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService)
{
    public ObservableCollection<SubjectListModel> Subjects { get; set; } = null!;

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        Subjects = (await subjectFacade.GetAsyncListModels(studentId:null)).ToObservableCollection();
    }
}
