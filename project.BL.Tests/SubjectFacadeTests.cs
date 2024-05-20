using project.BL.Facades;
using project.BL.Filters;
using project.BL.Models;
using project.Common.Tests;
using project.Common.Tests.Seeds;
using project.DAL.Entities;
using project.DAL.Mappers;
using Xunit.Abstractions;

namespace project.BL.Tests;

public sealed class SubjectFacadeTests : FacadeTestsBase
{
    private readonly ISubjectFacade _subjectFacadeSUT;
    private readonly SubjectAdminDetailModel _localAdmin;

    public SubjectFacadeTests(ITestOutputHelper output) : base(output)
    {
        _subjectFacadeSUT = new SubjectFacade(UnitOfWorkFactory, SubjectModelMapper, SubjectModelFilter, StudentModelFilter, ActivityModelFilter);
        _localAdmin =
            new SubjectAdminDetailModel { Id = SubjectSeeds.ICS.Id, Name = "NewName", Acronym = "NewAcronym", };
    }

    [Fact]
    public async Task Get_SubjectListModels_For_John()
    {
        //Act
        var listModels = await _subjectFacadeSUT.GetAsyncListModels(StudentSeeds.JohnL.Id);

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
        var detailModel = await _subjectFacadeSUT.GetAsyncStudentDetail(SubjectSeeds.ICS.Id, StudentSeeds.JohnL.Id);
        var ICSCviko = detailModel!.Activities.Single(a => a.Id == ActivitiesSeeds.ICSCviko.Id);

        //Assert
        Assert.True(detailModel.IsRegistered);
        Assert.NotNull(ICSCviko);
        Assert.True(ICSCviko.IsRegistered);
        Assert.Equal(RatingsSeeds.ICSCvikoRatingJohnL.Points, ICSCviko.Points);
    }

    [Fact]
    public async Task Get_SubjectStudentDetailModel_Of_ICS_For_Admin()
    {
        //Act
        var detailModel = await _subjectFacadeSUT.GetAsyncStudentDetail(SubjectSeeds.ICS.Id, null);
        var ICSCviko = detailModel!.Activities.Single(a => a.Id == ActivitiesSeeds.ICSCviko.Id);

        //Assert
        Assert.NotNull(detailModel);
        Assert.Equal(SubjectSeeds.ICS.Id, detailModel.Id);
        Assert.Equal(SubjectSeeds.ICS.Name, detailModel.Name);
        Assert.Equal(SubjectSeeds.ICS.Acronym, detailModel.Acronym);
        Assert.NotEmpty(detailModel.Activities);
        Assert.Equal(ActivitiesSeeds.ICSCviko.Area, ICSCviko.Area);
        Assert.Equal(ActivitiesSeeds.ICSCviko.Id, ICSCviko.Id);
        Assert.Equal(ActivitiesSeeds.ICSCviko.BeginTime, ICSCviko.BeginTime);
        Assert.Equal(ActivitiesSeeds.ICSCviko.EndTime, ICSCviko.EndTime);
        Assert.False(ICSCviko.IsRegistered);
        Assert.Equal(2, ICSCviko.RegisteredStudents);
        Assert.Equal(ActivitiesSeeds.ICSCviko.Type, ICSCviko.Type);
    }

    [Fact]
    public async Task Get_SubjectAdminDetailModel_Of_ICS()
    {
        //Act
        var detailModel = await _subjectFacadeSUT.GetAsync(SubjectSeeds.ICS.Id);

        //Assert
        Assert.NotNull(detailModel);
        Assert.Equal(SubjectSeeds.ICS.Id, detailModel.Id);
        Assert.Equal(SubjectSeeds.ICS.Name, detailModel.Name);
        Assert.Equal(SubjectSeeds.ICS.Acronym, detailModel.Acronym);
        Assert.Equal(2, detailModel.Students.Count);
        Assert.Single(detailModel.Students, s => s.Id == StudentSeeds.JohnL.Id);
        Assert.Single(detailModel.Students, s => s.Id == StudentSeeds.Terry.Id);
        var studentJohn = detailModel.Students.Single(s => s.Id == StudentSeeds.JohnL.Id);
        var studentTerry = detailModel.Students.Single(s => s.Id == StudentSeeds.Terry.Id);
        Assert.Equal(StudentSeeds.JohnL.Name, studentJohn.Name);
        Assert.Equal(StudentSeeds.JohnL.Surname, studentJohn.Surname);
        Assert.Equal(StudentSeeds.Terry.Name, studentTerry.Name);
        Assert.Equal(StudentSeeds.Terry.Surname, studentTerry.Surname);
    }

    [Fact]
    public async Task Register_John_IOS()
    {
        // Arrange
        var listModels = await _subjectFacadeSUT.GetAsyncListModels(StudentSeeds.JohnL.Id);
        var IOS = listModels.SingleOrDefault(s => s.Id == SubjectSeeds.IOS.Id);

        //Act
        await _subjectFacadeSUT.RegisterStudent(IOS!.Id, StudentSeeds.JohnL.Id);

        //Assert
        listModels = await _subjectFacadeSUT.GetAsyncListModels(StudentSeeds.JohnL.Id);
        Assert.True(listModels.SingleOrDefault(s => s.Id == SubjectSeeds.IOS.Id)!.IsRegistered);
    }

    [Fact]
    public async Task Unregister_Terry_ICS()
    {
        // Arrange
        var subjectListModels = await _subjectFacadeSUT.GetAsyncListModels(StudentSeeds.Terry.Id);
        var ICS = subjectListModels.SingleOrDefault(s => s.Id == SubjectSeeds.ICS.Id);

        //Act
        await _subjectFacadeSUT.UnregisterStudent(ICS!.Id, StudentSeeds.Terry.Id);

        //Assert
        subjectListModels = await _subjectFacadeSUT.GetAsyncListModels(StudentSeeds.Terry.Id);
        Assert.True(!subjectListModels.SingleOrDefault(s => s.Id == SubjectSeeds.ICS.Id)!.IsRegistered);

        // Assert that registered activities have been unregistered too.
        await using var uow = UnitOfWorkFactory.Create();
        var ratingRepository = uow.GetRepository<RatingEntity, RatingEntityMapper>();
        Assert.False(ratingRepository.Get().Any(r => r.Id == RatingsSeeds.ICSCvikoRatingTerry.Id));
        Assert.False(ratingRepository.Get().Any(r => r.Id == RatingsSeeds.ICSObhajobaRatingTerry.Id));
    }

    // Basic CRUD tests

    [Fact]
    public async Task Create_SubjectDetailModel()
    {
        //Arrange
        var rating = await _subjectFacadeSUT.SaveAsync(_localAdmin);

        //Act
        var remote = await _subjectFacadeSUT.GetAsync(rating.Id);

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
        var rating = await _subjectFacadeSUT.SaveAsync(_localAdmin);

        //Act
        var remote = await _subjectFacadeSUT.GetAsync(rating.Id);

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
        await _subjectFacadeSUT.DeleteAsync(SubjectSeeds.ICS.Id);

        var removed = await _subjectFacadeSUT.GetAsync(SubjectSeeds.ICS.Id);

        Assert.Null(removed);
    }

    [Fact]
    public async Task Delete_NonExisting_SubjectDetailModel()
    {
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await _subjectFacadeSUT.DeleteAsync(new Guid());
        });

        Assert.Equal($"Sequence contains no elements", exception.Message);
    }

    [Fact]
    public async Task Get_SubjectListModels_Sorted_By_Name()
    {
        //Arrange
        FilterPreferences preferences = FilterPreferences.Default with { SortByPropertyName = nameof(SubjectListModel.Name), DescendingOrder = true };

        //Act
        var listModels = (await _subjectFacadeSUT.GetAsyncListModels(null, preferences)).ToList();

        //Assert
        SortAssert.IsSorted(listModels, nameof(SubjectListModel.Name), true);
    }

    [Fact]
    public async Task Get_SubjectAdminDetail_Filter_By_Student_Name()
    {
        //Arrange
        FilterPreferences preferences = FilterPreferences.Default with { SearchedTerm = "terry" };

        //Act
        SubjectAdminDetailModel ICS = (await _subjectFacadeSUT.GetAsync(SubjectSeeds.ICS.Id, preferences))!;

        //Assert
        Assert.Contains(ICS.Students, s => s.Name == StudentSeeds.Terry.Name);
    }

    [Fact]
    public async Task Filter_Subjects_By_Name()
    {
        // Arrange
        FilterPreferences preferences = FilterPreferences.Default with { SearchedTerm = "Bezpečnost" };

        // Act
        IEnumerable<SubjectListModel> listModels = await _subjectFacadeSUT.GetAsyncListModels(StudentSeeds.Elliot.Id, preferences);

        // Assert
        Assert.Contains(listModels, s => s.Name == SubjectSeeds.IBS.Name);
    }

    [Fact]
    public async Task Filter_Subjects_Multiple_Results()
    {
        // Arrange
        FilterPreferences preferences = FilterPreferences.Default with { SearchedTerm = "I" };

        // Act
        IEnumerable<SubjectListModel> listModels = await _subjectFacadeSUT.GetAsyncListModels(StudentSeeds.Elliot.Id, preferences);

        // Assert
        Assert.Contains(listModels, s => s.Name == SubjectSeeds.IOS.Name);
        Assert.Contains(listModels, s => s.Name == SubjectSeeds.ICS.Name);
        Assert.Contains(listModels, s => s.Name == SubjectSeeds.IBS.Name);
        Assert.Contains(listModels, s => s.Name == SubjectSeeds.IVS.Name);
        Assert.Contains(listModels, s => s.Name == SubjectSeeds.ITS.Name);
    }

    [Fact]
    public async Task Filter_Subjects_By_Acronym()
    {
        // Arrange
        FilterPreferences preferences = FilterPreferences.Default with { SearchedTerm = "IVS" };

        // Act
        IEnumerable<SubjectListModel> listModels = await _subjectFacadeSUT.GetAsyncListModels(StudentSeeds.Elliot.Id, preferences);

        // Assert
        Assert.Contains(listModels, s => s.Name == SubjectSeeds.IVS.Name);
    }

    [Fact]
    public async Task Filter_Subjects_Non_Existing()
    {
        // Arrange
        FilterPreferences preferences1 = FilterPreferences.Default with { SearchedTerm = "IZU" };
        FilterPreferences preferences2 = FilterPreferences.Default with { SearchedTerm = "Inženýrství" };

        // Act
        IEnumerable<SubjectListModel> listModels1 = await _subjectFacadeSUT.GetAsyncListModels(StudentSeeds.Terry.Id, preferences1);
        IEnumerable<SubjectListModel> listModels2 = await _subjectFacadeSUT.GetAsyncListModels(StudentSeeds.JohnM.Id, preferences2);

        // Assert
        Assert.Empty(listModels1);
        Assert.Empty(listModels2);
    }

    [Fact]
    public async Task Filter_Subject_Activities_SubjectStudentDetailModel_By_Year_ICS()
    {
        // Arrange 
        FilterPreferences preferences = FilterPreferences.Default with { SearchedTerm = "2024" };
        // Act
        var detailModel = await _subjectFacadeSUT.GetAsyncStudentDetail(SubjectSeeds.ICS.Id, StudentSeeds.Terry.Id, preferences);

        // Assert
        Assert.NotNull(detailModel);
        var ICSObhajoba = detailModel!.Activities.Single(a => a.Id == ActivitiesSeeds.ICSObhajoba.Id);
        var ICSCviko = detailModel!.Activities.Single(a => a.Id == ActivitiesSeeds.ICSCviko.Id);

        Assert.NotNull(ICSObhajoba);
        Assert.NotNull(ICSCviko);
        Assert.Equal(ActivitiesSeeds.ICSObhajoba.Id, ICSObhajoba.Id);
        Assert.Equal(ActivitiesSeeds.ICSCviko.Id, ICSCviko.Id);
    }

    [Fact]
    public async Task Filter_Subject_Activities_SubjectStudentDetailModel_By_Time()
    {
        // Arrange 
        FilterPreferences findLabak = FilterPreferences.Default with { FilterByTime = true,
            BeginTime = new DateTime(2025, 4, 5, 8, 0, 0),
            EndTime = new DateTime(2025, 4, 5, 10, 0, 0)
        };
        FilterPreferences dontFindLabak = FilterPreferences.Default with
        {
            FilterByTime = true,
            BeginTime = new DateTime(2025, 4, 5, 8, 0, 1),
            EndTime = new DateTime(2025, 4, 5, 10, 0, 0)
        };
        // Act
        var detailModel = await _subjectFacadeSUT.GetAsyncStudentDetail(SubjectSeeds.IBS.Id, StudentSeeds.Terry.Id, findLabak);
        var detailModelWithoutLabak = await _subjectFacadeSUT.GetAsyncStudentDetail(SubjectSeeds.IBS.Id, StudentSeeds.Terry.Id, dontFindLabak);

        // Assert
        Assert.NotNull(detailModel);
        var IBSLabak = detailModel!.Activities.Single(a => a.Id == ActivitiesSeeds.IBSLabak.Id);
        Assert.NotNull(IBSLabak);
        Assert.Equal(ActivitiesSeeds.IBSLabak.Id, IBSLabak.Id);

        Assert.NotNull(detailModelWithoutLabak);
        Assert.DoesNotContain(detailModelWithoutLabak.Activities, a => a.Id == ActivitiesSeeds.IBSLabak.Id);
    }

    [Fact]
    public async Task Filter_Subject_Activities_SubjectStudentDetailModel_By_ActivityType()
    {
        // Arrange 
        FilterPreferences preferences = FilterPreferences.Default with
        {
            SearchedTerm   = "exer"
        };

        // Act
        var detailModel = await _subjectFacadeSUT.GetAsyncStudentDetail(SubjectSeeds.ICS.Id, null, preferences);

        // Assert
        Assert.NotNull(detailModel);
        Assert.Contains(detailModel.Activities, a => a.Id == ActivitiesSeeds.ICSCviko.Id);
        Assert.DoesNotContain(detailModel.Activities, a => a.Id == ActivitiesSeeds.ICSObhajoba.Id);
    }

    [Fact]
    public async Task Filter_Subject_Activities_SubjectStudentDetailModel_By_SchoolArea_1()
    {
        // Arrange 
        FilterPreferences preferences = FilterPreferences.Default with
        {
            SearchedTerm = "lab"
        };

        // Act
        var detailModel = await _subjectFacadeSUT.GetAsyncStudentDetail(SubjectSeeds.ICS.Id, null, preferences);

        // Assert
        Assert.NotNull(detailModel);
        Assert.Contains(detailModel.Activities, a => a.Id == ActivitiesSeeds.ICSCviko.Id);
        Assert.DoesNotContain(detailModel.Activities, a => a.Id == ActivitiesSeeds.ICSObhajoba.Id);
    }

    [Fact]
    public async Task Filter_Students_Registered_For_Subject_AdminDetailModel_1()
    {
        // Arrange 
        FilterPreferences preferences = FilterPreferences.Default with { SearchedTerm = "e" };
        // Act
        var detailModel = await _subjectFacadeSUT.GetAsync(SubjectSeeds.IOS.Id, preferences);

        // Assert
        Assert.NotNull(detailModel);
        Assert.NotEmpty(detailModel.Students);
        Assert.Contains(detailModel.Students, s => s.Id == StudentSeeds.Elliot.Id);
        Assert.Contains(detailModel.Students, s => s.Id == StudentSeeds.Terry.Id);
    }

    [Fact]
    public async Task Filter_Students_Registered_For_Subject_AdminDetailModel_2()
    {
        // Arrange 
        FilterPreferences preferences = FilterPreferences.Default with { SearchedTerm = "terry" };
        // Act
        var detailModel = await _subjectFacadeSUT.GetAsync(SubjectSeeds.IOS.Id, preferences);

        // Assert
        Assert.NotNull(detailModel);
        Assert.NotEmpty(detailModel.Students);
        Assert.Single(detailModel.Students);
        Assert.Contains(detailModel.Students, s => s.Id == StudentSeeds.Terry.Id);
    }
}