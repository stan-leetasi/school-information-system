using project.BL.Facades;
using project.BL.Models;
using project.Common.Tests;
using project.Common.Tests.Seeds;
using Microsoft.EntityFrameworkCore;
using project.DAL.Entities;
using Xunit;
using Xunit.Abstractions;
using System.Collections.Generic;
using System.Diagnostics;

namespace project.BL.Tests;

public sealed class ActivityFacadeTests : FacadeTestsBase
{
    private readonly IActivityFacade _activityFacadeSUT;

    public ActivityFacadeTests(ITestOutputHelper output) : base(output)
    {
        _activityFacadeSUT = new ActivityFacade(UnitOfWorkFactory, ActivityModelMapper);
    }

    [Fact]
    public async Task Get_ActivityListModels_For_ICS_John()
    {
        //Act
        IEnumerable<ActivityListModel> listModels = await _activityFacadeSUT.GetAsync(SubjectSeeds.ICS.Id, StudentSeeds.John.Id);

        var ICSCviko = listModels.SingleOrDefault(s => s.Id == ActivitiesSeeds.ICSCviko.Id);

        //Assert
        Assert.Equal(1, ICSCviko!.RegisteredStudents);
        Assert.True(ICSCviko.IsRegistered);
        Assert.Equal(RatingsSeeds.ICSRating.Points, ICSCviko.Points);
    }

    [Fact]
    public async Task Get_ActivityStudentDetailModel_ICS_John()
    {
        //Act
        var ICSCviko = await _activityFacadeSUT.GetAsyncStudentDetail(ActivitiesSeeds.ICSCviko.Id, StudentSeeds.John.Id);

        //Assert
        Assert.True(ICSCviko!.IsRegistered);
        Assert.Equal(RatingsSeeds.ICSRating.Points, ICSCviko.Points);
        Assert.Equal(RatingsSeeds.ICSRating.Notes, ICSCviko.Notes);
    }

    [Fact]
    public async Task Get_ActivityAdminDetailModel_ICS_John()
    {
        //Act
        var ICSCviko = await _activityFacadeSUT.GetAsyncAdminDetail(ActivitiesSeeds.ICSCviko.Id);

        //Assert
        Assert.True(ICSCviko != null);
        Assert.Contains(ICSCviko.Ratings, r => r.Id == RatingsSeeds.ICSRating.Id);
    }
}
