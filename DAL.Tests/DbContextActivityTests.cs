using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using project.Common.Tests;
using project.DAL.Entities;
using Xunit;
using Xunit.Abstractions;
using project.Common.Tests.Seeds;
using System;

namespace project.DAL.Tests;

public class DbContextActivityTests(ITestOutputHelper output) : DbContextTestsBase(output)
{
    [Fact]
    public async Task Add_New_Activity_Persisted()
    {
        // Arrange
        ActivityEntity entity = ActivitiesSeeds.EmptyActivityEntity with
        {
            BeginTime = new DateTime(2024, 3, 4, 9, 0, 0),
            EndTime = new DateTime(2024, 3, 4, 11, 0, 0),
            Description = "ICS Polsemka",
            SubjectId = SubjectSeeds.ICS.Id,
            Area = Common.Enums.SchoolArea.MainLectureHall,
            Type = Common.Enums.ActivityType.MidtermExam,
        };


        // Act
        ProjectDbContextSUT.Activities.Add(entity);
        await ProjectDbContextSUT.SaveChangesAsync();


        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var RetrievedEntity = await dbx.Activities.SingleAsync(i => i.Id == entity.Id);

        DeepAssert.Equal(entity, RetrievedEntity);
    }

    [Fact]
    public async Task Update_Activity_Info()
    {
        // Arrange
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var Activity = await dbx.Activities.SingleAsync(i => i.Id == ActivitiesSeeds.ICSCviko.Id);

        Assert.NotNull(Activity);

        // Act
        Activity.Description = "ICS Democviko";
        Activity.Area = Common.Enums.SchoolArea.MainLectureHall;

        ProjectDbContextSUT.Activities.Update(Activity);
        await ProjectDbContextSUT.SaveChangesAsync();

        // Assert
        var RetrievedActivity = await dbx.Activities.SingleAsync(i => i.Id == Activity.Id);
        DeepAssert.Equal(Activity, RetrievedActivity);
    }

    [Fact]
    public async Task Delete_Activity_ById()
    {
        // Arrange
        var ActivityToDeleteId = ActivitiesSeeds.IOSPolsemka.Id;
        var RatingId = RatingsSeeds.IOSRating.Id;
        
        ProjectDbContextSUT.Remove(ProjectDbContextSUT.Ratings.Single(i => i.Id == RatingId));
        await ProjectDbContextSUT.SaveChangesAsync();

        // Act
        ProjectDbContextSUT.Remove(ProjectDbContextSUT.Activities.Single(i => i.Id == ActivityToDeleteId));
        await ProjectDbContextSUT.SaveChangesAsync();

        // Assert
        Assert.False(await ProjectDbContextSUT.Activities.AnyAsync(i => i.Id == ActivityToDeleteId));
    }

    [Fact]
    public async Task Get_All_Activities_Contains_ICSCviko()
    {
        // Act
        var entities = await ProjectDbContextSUT.Activities.ToArrayAsync();

        // Assert
        DeepAssert.Contains(ActivitiesSeeds.ICSCviko, entities, nameof(ActivityEntity.Subject), nameof(ActivityEntity.Ratings));
    }

    [Fact]
    public async Task Retrieve_ById_Activity_ICSCviko()
    {
        // Act
        var entity = await ProjectDbContextSUT.Activities.SingleAsync(a => a.Id == ActivitiesSeeds.ICSCviko.Id);

        // Assert
        DeepAssert.Equal(ActivitiesSeeds.ICSCviko, entity, nameof(ActivityEntity.Subject), nameof(ActivityEntity.Ratings));
    }

    [Fact]
    public async Task Retrieve_ById_Non_Existent_Activity()
    {
        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await ProjectDbContextSUT.Activities.SingleAsync(a => a.Id == new Guid());
        });
    }
}
