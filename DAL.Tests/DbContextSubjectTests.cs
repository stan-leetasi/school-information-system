using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using project.Common.Tests;
using project.DAL.Entities;
using Xunit;
using Xunit.Abstractions;
using project.Common.Tests.Seeds;

namespace project.DAL.Tests;

public class DbContextSubjectTests(ITestOutputHelper output) : DbContextTestsBase(output)
{
    [Fact]
    public async Task Add_New_Subject_Persisted()
    {
        // Arrange
        SubjectEntity entity = new()
        {
            // Generate new GUID for the subject
            Id = Guid.NewGuid() /*Parse("38871102 - c06f - 4f68 - 99eb - c0f6c0b2d33b")*/ ,
            Name = "C++",
            Acronym = "ICP"
        };

        // Act
        ProjectDbContextSUT.Subjects.Add(entity);
        await ProjectDbContextSUT.SaveChangesAsync();

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntities = await dbx.Subjects.SingleAsync(i => i.Id == entity.Id);

        DeepAssert.Equal(entity, actualEntities);
    }

    [Fact]
    public async Task Update_Subject_Info()
    {
        // Arrange
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var Subject = await dbx.Subjects.SingleAsync(i => i.Id == SubjectSeeds.IOS.Id);

        Assert.NotNull(Subject);

        // Act
        Subject.Name = "Základy Programovania";
        Subject.Acronym = "IZP";

        ProjectDbContextSUT.Subjects.Update(Subject);
        await ProjectDbContextSUT.SaveChangesAsync();

        // Assert
        var RetrievedSubject = await dbx.Subjects.SingleAsync(i => i.Id == Subject.Id);
        DeepAssert.Equal(Subject, RetrievedSubject);
    }

    [Fact]
    public async Task Delete_Subject_By_Id()
    {
        // Arrange
        var SubjectToDeleteId = SubjectSeeds.ICS.Id;
        var SubjectRatingId = RatingsSeeds.ICSRating.Id;

        ProjectDbContextSUT.Remove(ProjectDbContextSUT.Ratings.Single(i => i.Id == SubjectRatingId));
        await ProjectDbContextSUT.SaveChangesAsync();

        // Act
        ProjectDbContextSUT.Remove(ProjectDbContextSUT.Subjects.Single(i => i.Id == SubjectToDeleteId));
        await ProjectDbContextSUT.SaveChangesAsync();

        // Assert
        Assert.False(await ProjectDbContextSUT.Subjects.AnyAsync(i => i.Id == SubjectToDeleteId));
    }

    [Fact]
    public async Task Get_All_Subjects_Contains_ICS()
    {
        //Act
        var entities = await ProjectDbContextSUT.Subjects.ToListAsync();

        //Assert
        DeepAssert.Contains(SubjectSeeds.ICS, entities, nameof(SubjectEntity.Students), nameof(SubjectEntity.Activities));
    }

    [Fact]
    public async Task Get_ById_Subject_Contains_John()
    {
        //Act
        var entity = await ProjectDbContextSUT.Subjects.SingleAsync(i => i.Id == SubjectSeeds.ICS.Id);

        //Assert
        DeepAssert.Equal(SubjectSeeds.ICS, entity, nameof(SubjectEntity.Students));
    }

    [Fact]
    public async Task Get_ById_Subject_Includes_Students()
    {
        //Act
        var entity = await ProjectDbContextSUT.Subjects
            .Include(i => i.Students)
            .ThenInclude(i => i.Student)
            .SingleAsync(i => i.Id == SubjectSeeds.ICS.Id);

        //Assert
        DeepAssert.Equal(SubjectSeeds.ICS, entity, nameof(SubjectEntity.Students));
        DeepAssert.Contains(StudentSubjectSeeds.JohnICS, entity.Students, nameof(StudentSubjectEntity.Student), nameof(StudentSubjectEntity.Subject));
    }

    [Fact]
    public void Get_ById_Non_Existing_Subject()
    {

        // Act & Assert
        var exception = Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await ProjectDbContextSUT.Subjects.SingleAsync(i => i.Id == new Guid());
        });
    }
}
