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
    public async Task AddNew_Student_Persisted()
    {
        //Arrange
        StudentEntity entity = new()
        {
            Id = Guid.Parse("af1c534f-9e97-4c3a-a17a-f446328b460c") ,
            Name = "Michael",
            Surname = "Mouse",
            ImageUrl = "www.photots.com/micheal.jpeg"
        };

        //Act
        ProjectDbContextSUT.Students.Add(entity);
        await ProjectDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntities = await dbx.Students.SingleAsync(i => i.Id == entity.Id);

        DeepAssert.Equal(entity, actualEntities);
    }
    
    [Fact]
    public async Task GetAll_Students_ContainsJohn()
    {
        //Act
        var entities = await ProjectDbContextSUT.Students.ToArrayAsync();

        //Assert
        DeepAssert.Contains(StudentSeeds.John, entities, nameof(StudentEntity.Subjects), nameof(StudentEntity.Ratings));
    }

    [Fact]
    public async Task GetById_Student_JohnRetrieved()
    {
        //Act
        var entity = await ProjectDbContextSUT.Students.SingleAsync(i => i.Id == StudentSeeds.John.Id);

        //Assert
        DeepAssert.Equal(StudentSeeds.John, entity, nameof(StudentEntity.Subjects), nameof(StudentEntity.Ratings));
    }
}
