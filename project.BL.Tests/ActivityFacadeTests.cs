﻿using project.BL.Facades;
using project.BL.Models;
using project.Common.Tests;
using project.Common.Tests.Seeds;
using Microsoft.EntityFrameworkCore;
using project.DAL.Entities;
using Xunit;
using Xunit.Abstractions;
using System.Collections.Generic;
using System.Diagnostics;
using project.Common.Enums;
using project.BL.Filters;

namespace project.BL.Tests;

public sealed class ActivityFacadeTests : FacadeTestsBase
{
    private readonly IActivityFacade _activityFacadeSUT;

    public ActivityFacadeTests(ITestOutputHelper output) : base(output)
    {
        _activityFacadeSUT = new ActivityFacade(UnitOfWorkFactory, ActivityModelMapper, ActivityModelFilter, RatingModelFilter);
    }

    [Fact]
    public async Task Get_ActivityStudentDetailModel_ICS_John()
    {
        //Act
        var ICSCviko = await _activityFacadeSUT.GetAsyncStudentDetail(ActivitiesSeeds.ICSCviko.Id, StudentSeeds.JohnL.Id);

        //Assert
        Assert.True(ICSCviko!.IsRegistered);
        Assert.Equal(RatingsSeeds.ICSCvikoRatingJohnL.Points, ICSCviko.Points);
        Assert.Equal(RatingsSeeds.ICSCvikoRatingJohnL.Notes, ICSCviko.Notes);
    }

    [Fact]
    public async Task Get_ActivityAdminDetailModel_ICSCviko()
    {
        //Act
        var ICSCviko = await _activityFacadeSUT.GetAsync(ActivitiesSeeds.ICSCviko.Id);

        //Assert
        Assert.True(ICSCviko != null);
        Assert.Contains(ICSCviko.Ratings, r => r.Id == RatingsSeeds.ICSCvikoRatingJohnL.Id);
    }

    [Fact]
    public async Task Create_New_Activity_For_Existing_Subject()
    {
        // Arrange
        ActivityAdminDetailModel activity = ActivityAdminDetailModel.Empty with
        {
            SubjectId = SubjectSeeds.ICS.Id,
            Description = "Final Exam"
        };

        //Act
        activity = await _activityFacadeSUT.SaveAsync(activity);

        //Assert
        ActivityAdminDetailModel? detailModel = await _activityFacadeSUT.GetAsync(activity.Id);
        Assert.NotNull(detailModel);
        Assert.Equal("Final Exam", detailModel.Description);
    }

    [Fact]
    public async Task Create_New_Activity_For_Non_Existing_Subject()
    {
        var NonExistingSubjectId = new Guid();
        // Arrange
        ActivityAdminDetailModel activity = ActivityAdminDetailModel.Empty with
        {
            SubjectId = NonExistingSubjectId,
            Description = "Final Exam"
        };

        //Act
        await Assert.ThrowsAsync<DbUpdateException>(async () =>
        {
            await _activityFacadeSUT.SaveAsync(activity);
        });

    }

    [Fact]
    public async Task Remove_Activity()
    {
        var ICSCvikoId = ActivitiesSeeds.ICSCviko.Id;

        await _activityFacadeSUT.DeleteAsync(ICSCvikoId);

        var RemovedActivity = await _activityFacadeSUT.GetAsync(ICSCvikoId);

        Assert.Null(RemovedActivity);
    }

    [Fact]
    public async Task Remove_Non_existing_Activity()
    {
        // Arrange
        var NonExistingActivityID = new Guid();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await _activityFacadeSUT.DeleteAsync(NonExistingActivityID);
        });

        Assert.Equal($"Sequence contains no elements", exception.Message);
    }

    [Fact]
    public async Task Register_Student_For_Activity()
    {
        // Arrange
        var activityId = ActivitiesSeeds.IOSSemka.Id;
        var studentId = StudentSeeds.Elliot.Id;

        // Act
        await _activityFacadeSUT.RegisterStudent(activityId, studentId);

        var ElliotIOSExam = await _activityFacadeSUT.GetAsyncStudentDetail(activityId, studentId);

        //Assert
        Assert.NotNull(ElliotIOSExam);
        Assert.True(ElliotIOSExam!.IsRegistered);
        Assert.Equal(SubjectSeeds.IOS.Id, ElliotIOSExam.SubjectId);
        Assert.Equal(SubjectSeeds.IOS.Name, ElliotIOSExam.SubjectName);
        Assert.Equal(0, ElliotIOSExam.Points);
        Assert.Equal("", ElliotIOSExam.Notes);
        Assert.Equal(ActivityType.FinalExam, ElliotIOSExam.Type);
    }

    [Fact]
    public async Task Register_Student_For_Activity_He_Is_Registered_For()
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await _activityFacadeSUT.RegisterStudent(ActivitiesSeeds.ICSCviko.Id, StudentSeeds.JohnL.Id);
        });

        Assert.Equal("Student is already registered.", exception.Message);
    }

    [Fact]
    public async Task Register_NonExisting_Student_For_Activity()
    {
        // Arrange

        var NonExistingStudent = new Guid();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await _activityFacadeSUT.RegisterStudent(ActivitiesSeeds.ICSCviko.Id, NonExistingStudent);
        });
    }

    [Fact]
    public async Task Unregister_Student_From_Activity()
    {
        // Act
        await _activityFacadeSUT.UnregisterStudent(ActivitiesSeeds.ICSCviko.Id, StudentSeeds.JohnL.Id);

        //Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await _activityFacadeSUT.GetAsyncStudentDetail(ActivitiesSeeds.ICSCviko.Id, StudentSeeds.JohnL.Id);
        });
    }

    [Fact]
    public async Task Unregister_Student_From_Activity_He_Is_Not_Registered_For()
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await _activityFacadeSUT.UnregisterStudent(ActivitiesSeeds.ITSSkuska.Id, StudentSeeds.Terry.Id);
        });

        Assert.Equal("Registration does not exist.", exception.Message);
    }

    [Fact]
    public async Task Unregister_NonExisting_Student_From_Activity()
    {
        // Arrange
        var NonExistingStudent = new Guid();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await _activityFacadeSUT.UnregisterStudent(ActivitiesSeeds.ICSCviko.Id, NonExistingStudent);
        });
    }

    [Fact]
    public async Task Register_Student_For_Non_Existing_Activity()
    {
        // Arrange
        var activityId = new Guid();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            await _activityFacadeSUT.RegisterStudent(activityId, StudentSeeds.Terry.Id);
        });
    }

    [Fact]
    public async Task Unregister_Student_From_Non_Existing_Activity()
    {
        // Arrange
        var activityId = new Guid();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await _activityFacadeSUT.UnregisterStudent(activityId, StudentSeeds.Terry.Id);
        });
    }
    [Fact]
    public async Task Register_Student_For_Activity_Without_Registered_Subject()
    {
        // Arrange
        var subjectId = SubjectSeeds.ITS.Id;
        var activityId = ActivitiesSeeds.ITSSkuska.Id;
        var studentId = StudentSeeds.Terry.Id;

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await _activityFacadeSUT.RegisterStudent(activityId, studentId);
        });
    }
}


