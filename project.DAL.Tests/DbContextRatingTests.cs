using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using project.Common.Tests;
using project.DAL.Entities;
using Xunit;
using Xunit.Abstractions;
using project.Common.Tests.Seeds;

namespace project.DAL.Tests;

public class DbContextRatingTests(ITestOutputHelper output) : DbContextTestsBase(output)
{
    [Fact]
    public void RatingEntity_PropertiesAreSetCorrectly()
    {
        // Arrange
        var rating = RatingsSeeds.ICSCvikoRatingJohnL;

        // Act
        var id = rating.Id;
        var points = rating.Points;
        var notes = rating.Notes;
        var activityId = rating.ActivityId;
        var studentId = rating.StudentId;

        // Assert
        Assert.Equal("5d85804a-7ab0-4d38-b449-cc3f68887c37", id.ToString());
        Assert.Equal(8, points);
        Assert.Equal("Skvělé", notes);
    }

    [Fact]
    public async Task Delete_Rating_ById()
    {
        // Arrange
        var RatingToDeleteId = RatingsSeeds.ICSCvikoRatingJohnL.Id;

        // Act
        ProjectDbContextSUT.Remove(ProjectDbContextSUT.Ratings.Single(i => i.Id == RatingToDeleteId));
        await ProjectDbContextSUT.SaveChangesAsync();

        // Assert
        Assert.False(await ProjectDbContextSUT.Ratings.AnyAsync(i => i.Id == RatingToDeleteId));
    }

    [Fact]
    public void Delete_Non_Existent_Rating_ById()
    {
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
        {
            ProjectDbContextSUT.Remove(ProjectDbContextSUT.Ratings.Single(i => i.Id == new Guid()));
        });
    }

    [Fact]
    public async Task Update_Rating_Persisted()
    {
        // Arrange
        var baseEntity = RatingsSeeds.ICSCvikoRatingJohnL;
        var entity =
            baseEntity with
            {
                Points = 49,
                Notes = baseEntity + "Updated",
                Student = default,
                Activity = default,
            };

        // Act
        ProjectDbContextSUT.Ratings.Update(entity);
        await ProjectDbContextSUT.SaveChangesAsync();

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Ratings.SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity, nameof(RatingEntity.Student), nameof(RatingEntity.Activity));
    }

    [Fact]
    public async Task Add_New_Rating_Persisted()
    {
        // Arrange
        RatingEntity entity = RatingsSeeds.EmptyRatingEntity with
        {
            Id = Guid.NewGuid(),
            ActivityId = ActivitiesSeeds.IOSPolsemka.Id,
            Notes = "Měl byste se nad sebou zamyslet",
            Points = 15,
            StudentId = StudentSeeds.JohnL.Id,
        };

        // Act
        ProjectDbContextSUT.Ratings.Add(entity);
        await ProjectDbContextSUT.SaveChangesAsync();

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntities = await dbx.Ratings.SingleAsync(i => i.Id == entity.Id);

        DeepAssert.Equal(entity, actualEntities, nameof(SubjectEntity.Students), nameof(SubjectEntity.Activities));
    }
}
