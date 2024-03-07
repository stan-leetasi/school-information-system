using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using project.Common.Tests;
using project.DAL.Entities;
using Xunit;
using Xunit.Abstractions;
using project.Common.Tests.Seeds;
using System;

namespace project.DAL.Tests;

// Testing operations related to activities in the DbContext
public class DbContextActivityTests(ITestOutputHelper output) : DbContextTestsBase(output)
{
    // Test that a new activity can be added and will persist in the database
    [Fact]
    public async Task AddNew_Activity_Persisted()
    {
        //Arrange
        ActivityEntity entity = ActivitiesSeeds.EmptyActivityEntity with
        {
            BeginTime = new DateTime(2024, 3, 4, 9, 0, 0),
            EndTime = new DateTime(2024, 3, 4, 11, 0, 0),
            Description = "ICS Polsemka",
            //Subject = SubjectSeeds.ICS,
            SubjectId = SubjectSeeds.ICS.Id,
            Area = Common.Enums.SchoolArea.MainLectureHall,
            Type = Common.Enums.ActivityType.MidtermExam,
        };


        //Act
        ProjectDbContextSUT.Activities.Add(entity);
        await ProjectDbContextSUT.SaveChangesAsync();


        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntities = await dbx.Activities.SingleAsync(i => i.Id == entity.Id);

        DeepAssert.Equal(entity, actualEntities);
    }

    // Test that all activities retrieved from the database contain ICS Cviko
    [Fact]
    public async Task GetAll_Activities_ContainsICSCviko()
    {
        //Act
        var entities = await ProjectDbContextSUT.Activities.ToArrayAsync();

        //Assert
        DeepAssert.Contains(ActivitiesSeeds.ICSCviko, entities, nameof(ActivityEntity.Subject), nameof(ActivityEntity.Ratings));
    }

    // Test that a specific activity (ICS Cviko) can be retrieved by ID
    [Fact]
    public async Task GetById_Activity_ICSCvikoRetrieved()
    {
        //Act
        var entity = await ProjectDbContextSUT.Activities.SingleAsync(a => a.Id == ActivitiesSeeds.ICSCviko.Id);

        //Assert
        DeepAssert.Equal(ActivitiesSeeds.ICSCviko, entity, nameof(ActivityEntity.Subject), nameof(ActivityEntity.Ratings));
    }
}
