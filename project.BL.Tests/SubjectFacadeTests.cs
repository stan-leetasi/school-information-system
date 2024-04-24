using project.BL.Facades;
using project.BL.Filters;
using project.BL.Models;
using project.Common.Tests;
using project.Common.Tests.Seeds;
using Xunit.Abstractions;

namespace project.BL.Tests;

public sealed class SubjectFacadeTests : FacadeTestsBase
{
    private readonly ISubjectFacade _subjectFacadeSut;
    private readonly SubjectAdminDetailModel _localAdmin;

    public SubjectFacadeTests(ITestOutputHelper output) : base(output)
    {
        _subjectFacadeSut = new SubjectFacade(UnitOfWorkFactory, SubjectModelMapper, SubjectModelFilter, StudentModelFilter, ActivityModelFilter);
        _localAdmin =
            new SubjectAdminDetailModel { Id = SubjectSeeds.ICS.Id, Name = "NewName", Acronym = "NewAcronym", };
    }

    [Fact]
    public async Task Get_SubjectListModels_For_John()
    {
        //Act
        var listModels = await _subjectFacadeSut.GetAsyncListModels(StudentSeeds.John.Id);

        IEnumerable<SubjectListModel> subjectListModels = listModels as SubjectListModel[] ?? listModels.ToArray();
        var ICS = subjectListModels.SingleOrDefault(s => s.Id == SubjectSeeds.ICS.Id);
        var IOS = subjectListModels.SingleOrDefault(s => s.Id == SubjectSeeds.IOS.Id);

        //Assert
        Assert.True(ICS!.IsRegistered && !IOS!.IsRegistered);
    }

    [Fact]
    public async Task Get_SubjectStudentDetailModel_Of_ICS_For_John()
    {
        //Act
        var detailModel = await _subjectFacadeSut.GetAsyncStudentDetail(SubjectSeeds.ICS.Id, StudentSeeds.John.Id);
        var ICSCviko = detailModel!.Activities.Single(a => a.Id == ActivitiesSeeds.ICSCviko.Id);

        //Assert
        Assert.NotNull(ICSCviko);
        Assert.True(ICSCviko.IsRegistered);
        Assert.Equal(RatingsSeeds.ICSRating.Points, ICSCviko.Points);
    }

    [Fact]
    public async Task Get_SubjectStudentDetailModel_Of_ICS_For_Admin()
    {
        //Act
        var detailModel = await _subjectFacadeSut.GetAsyncStudentDetail(SubjectSeeds.ICS.Id, null);
        var ICSCviko = detailModel!.Activities.Single(a => a.Id == ActivitiesSeeds.ICSCviko.Id);

        //Assert
        Assert.NotNull(detailModel);
        Assert.Equal(SubjectSeeds.ICS.Id, detailModel.Id);
        Assert.Equal(SubjectSeeds.ICS.Name, detailModel.Name);
        Assert.Equal(SubjectSeeds.ICS.Acronym, detailModel.Acronym);
        Assert.Single(detailModel.Activities);
        Assert.Equal(ActivitiesSeeds.ICSCviko.Area, ICSCviko.Area);
        Assert.Equal(ActivitiesSeeds.ICSCviko.Id, ICSCviko.Id);
        Assert.Equal(ActivitiesSeeds.ICSCviko.BeginTime, ICSCviko.BeginTime);
        Assert.Equal(ActivitiesSeeds.ICSCviko.EndTime, ICSCviko.EndTime);
        Assert.False(ICSCviko.IsRegistered);
        Assert.Equal(1, ICSCviko.RegisteredStudents);
        Assert.Equal(ActivitiesSeeds.ICSCviko.Type, ICSCviko.Type);
    }

    [Fact]
    public async Task Get_SubjectAdminDetailModel_Of_ICS()
    {
        //Act
        var detailModel = await _subjectFacadeSut.GetAsync(SubjectSeeds.ICS.Id);

        //Assert
        Assert.NotNull(detailModel);
        Assert.Equal(SubjectSeeds.ICS.Id, detailModel.Id);
        Assert.Equal(SubjectSeeds.ICS.Name, detailModel.Name);
        Assert.Equal(SubjectSeeds.ICS.Acronym, detailModel.Acronym);
        Assert.Equal(2, detailModel.Students.Count);
        Assert.Single(detailModel.Students, s => s.Id == StudentSeeds.John.Id);
        Assert.Single(detailModel.Students, s => s.Id == StudentSeeds.Terry.Id);
        var studentJohn = detailModel.Students.Single(s => s.Id == StudentSeeds.John.Id);
        var studentTerry = detailModel.Students.Single(s => s.Id == StudentSeeds.Terry.Id);
        Assert.Equal(StudentSeeds.John.Name, studentJohn.Name);
        Assert.Equal(StudentSeeds.John.Surname, studentJohn.Surname);
        Assert.Equal(StudentSeeds.Terry.Name, studentTerry.Name);
        Assert.Equal(StudentSeeds.Terry.Surname, studentTerry.Surname);
    }

    [Fact]
    public async Task Register_John_IOS()
    {
        // Arrange
        var listModels = await _subjectFacadeSut.GetAsyncListModels(StudentSeeds.John.Id);
        var IOS = listModels.SingleOrDefault(s => s.Id == SubjectSeeds.IOS.Id);

        //Act
        await _subjectFacadeSut.RegisterStudent(IOS!.Id, StudentSeeds.John.Id);

        //Assert
        listModels = await _subjectFacadeSut.GetAsyncListModels(StudentSeeds.John.Id);
        Assert.True(listModels.SingleOrDefault(s => s.Id == SubjectSeeds.IOS.Id)!.IsRegistered);
    }

    [Fact]
    public async Task Unregister_Terry_ICS()
    {
        // Arrange
        var subjectListModels = await _subjectFacadeSut.GetAsyncListModels(StudentSeeds.Terry.Id);
        var ICS = subjectListModels.SingleOrDefault(s => s.Id == SubjectSeeds.ICS.Id);

        //Act
        await _subjectFacadeSut.UnregisterStudent(ICS!.Id, StudentSeeds.Terry.Id);

        //Assert
        subjectListModels = await _subjectFacadeSut.GetAsyncListModels(StudentSeeds.Terry.Id);
        Assert.True(!subjectListModels.SingleOrDefault(s => s.Id == SubjectSeeds.ICS.Id)!.IsRegistered);
    }

    // Basic CRUD tests

    [Fact]
    public async Task Create_SubjectDetailModel()
    {
        //Arrange
        var rating = await _subjectFacadeSut.SaveAsync(_localAdmin);

        //Act
        var remote = await _subjectFacadeSut.GetAsync(rating.Id);

        //Assert
        Assert.NotNull(rating);
        Assert.NotNull(remote);
        Assert.Equal(_localAdmin.Id, remote.Id);
        Assert.Equal(_localAdmin.Name, remote.Name);
        Assert.Equal(_localAdmin.Acronym, remote.Acronym);
    }

    [Fact]
    public async Task Update_SubjectDetailModel()
    {
        //Arrange
        var rating = await _subjectFacadeSut.SaveAsync(_localAdmin);

        //Act
        var remote = await _subjectFacadeSut.GetAsync(rating.Id);

        //Assert
        Assert.NotNull(rating);
        Assert.NotNull(remote);
        Assert.Equal(_localAdmin.Id, remote.Id);
        Assert.Equal(_localAdmin.Name, remote.Name);
        Assert.Equal(_localAdmin.Acronym, remote.Acronym);
    }

    [Fact]
    public async Task Delete_Existing_SubjectDetailModel()
    {
        await _subjectFacadeSut.DeleteAsync(SubjectSeeds.ICS.Id);

        var removed = await _subjectFacadeSut.GetAsync(SubjectSeeds.ICS.Id);

        Assert.Null(removed);
    }

    [Fact]
    public async Task Delete_NonExisting_SubjectDetailModel()
    {
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await _subjectFacadeSut.DeleteAsync(new Guid());
        });

        Assert.Equal($"Sequence contains no elements", exception.Message);
    }

    [Fact]
    public async Task Get_SubjectListModels_Sorted_By_Name()
    {
        //Arrange
        FilterPreferences preferences = FilterPreferences.Default with { SortByPropertyName = nameof(SubjectListModel.Name), DescendingOrder = true };

        //Act
        var listModels = (await _subjectFacadeSut.GetAsyncListModels(null, preferences)).ToList();

        //Assert
        SortAssert.IsSorted(listModels, nameof(SubjectListModel.Name), true);
    }

    [Fact]
    public async Task Get_SubjectAdminDetail_Filter_By_Student_Name()
    {
        //Arrange
        FilterPreferences preferences = FilterPreferences.Default with { SearchedTerm = "terry"};

        //Act
        SubjectAdminDetailModel ICS = (await _subjectFacadeSut.GetAsync(SubjectSeeds.ICS.Id, preferences))!;

        //Assert
        Assert.Contains(ICS.Students, s => s.Name == StudentSeeds.Terry.Name);
    }

    [Fact]
    public async Task Get_SubjectStudentDetailModel_Of_ICS_For_Admin_Filtered_By_Date()
    {
        //Arrange
        FilterPreferences preferences = FilterPreferences.Default with { SearchedTerm = "4.3." };

        //Act
        var detailModel = await _subjectFacadeSut.GetAsyncStudentDetail(SubjectSeeds.ICS.Id, null, preferences);
        var ICSCviko = detailModel!.Activities.Single(a => a.Id == ActivitiesSeeds.ICSCviko.Id);

        //Assert
        Assert.NotNull(detailModel);
        Assert.Equal(SubjectSeeds.ICS.Id, detailModel.Id);
        Assert.Equal(SubjectSeeds.ICS.Name, detailModel.Name);
        Assert.Equal(SubjectSeeds.ICS.Acronym, detailModel.Acronym);
        Assert.Single(detailModel.Activities);
        Assert.Equal(ActivitiesSeeds.ICSCviko.Area, ICSCviko.Area);
        Assert.Equal(ActivitiesSeeds.ICSCviko.Id, ICSCviko.Id);
        Assert.Equal(ActivitiesSeeds.ICSCviko.BeginTime, ICSCviko.BeginTime);
        Assert.Equal(ActivitiesSeeds.ICSCviko.EndTime, ICSCviko.EndTime);
        Assert.False(ICSCviko.IsRegistered);
        Assert.Equal(1, ICSCviko.RegisteredStudents);
        Assert.Equal(ActivitiesSeeds.ICSCviko.Type, ICSCviko.Type);
    }
}