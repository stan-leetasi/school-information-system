using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using project.Common.Tests;
using project.DAL.Entities;
using Xunit;
using Xunit.Abstractions;
using project.Common.Tests.Seeds;

namespace project.DAL.Tests;

public class DbContextStudentTests(ITestOutputHelper output) : DbContextTestsBase(output) 
{
    [Fact]
    public async Task Add_New_Student_Persisted()
    {
        // Arrange
        StudentEntity entity = new()
        {
            Id = Guid.Parse("af1c534f-9e97-4c3a-a17a-f446328b460c") ,
            Name = "Michael",
            Surname = "Mouse",
            ImageUrl = "www.photots.com/micheal.jpeg"
        };

        // Act
        ProjectDbContextSUT.Students.Add(entity);
        await ProjectDbContextSUT.SaveChangesAsync();

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntities = await dbx.Students.SingleAsync(i => i.Id == entity.Id);

        DeepAssert.Equal(entity, actualEntities);
    }

    [Fact]
    public async Task Update_Student_Info()
    {
        // Arrange
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var Student = await dbx.Students.SingleAsync(i => i.Id == StudentSeeds.John.Id);
 
        Assert.NotNull(Student);

        // Act
        Student.Name = "Jason";
        Student.Surname = "Bourne";

        ProjectDbContextSUT.Students.Update(Student);
        await ProjectDbContextSUT.SaveChangesAsync();
        
        // Assert
        var RetrievedStudent = await dbx.Students.SingleAsync(i => i.Id == Student.Id);
        DeepAssert.Equal(Student, RetrievedStudent);
    }

    [Fact]
    public async Task Delete_Student_By_Id()
    {
        // Arrange
        var StudentToDeleteId = StudentSeeds.John.Id;
        var RatingToDeleteId = RatingsSeeds.ICSRating.Id;
        var StudentSubjectToDeleteId = StudentSubjectSeeds.JohnICS.Id;

        ProjectDbContextSUT.Remove(ProjectDbContextSUT.Ratings.Single(i => i.Id == RatingToDeleteId));
        await ProjectDbContextSUT.SaveChangesAsync();

        ProjectDbContextSUT.Remove(ProjectDbContextSUT.StudentSubjects.Single(i => i.Id == StudentSubjectToDeleteId));
        await ProjectDbContextSUT.SaveChangesAsync();

        // Act
        ProjectDbContextSUT.Remove(ProjectDbContextSUT.Students.Single(i => i.Id == StudentToDeleteId));
        await ProjectDbContextSUT.SaveChangesAsync();

        // Assert
        Assert.False(await ProjectDbContextSUT.Students.AnyAsync(i => i.Id == StudentToDeleteId));
    }

    [Fact]
    public async Task Get_All_Students_Contains_John()
    {
        // Act
        var entities = await ProjectDbContextSUT.Students.ToArrayAsync();

        // Assert
        DeepAssert.Contains(StudentSeeds.John, entities, nameof(StudentEntity.Subjects), nameof(StudentEntity.Ratings));
    }


    [Fact]
    public async Task Get_ById_Student_John()
    {
        // Act
        var entity = await ProjectDbContextSUT.Students.SingleAsync(i => i.Id == StudentSeeds.John.Id);

        // Assert
        DeepAssert.Equal(StudentSeeds.John, entity, nameof(StudentEntity.Subjects), nameof(StudentEntity.Ratings));
    }

    [Fact]
    public async Task Get_ById_Non_Existing_Student()
    {
        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            var entity = await ProjectDbContextSUT.Students.SingleAsync(i => i.Id == new Guid());
        });
    }
}
