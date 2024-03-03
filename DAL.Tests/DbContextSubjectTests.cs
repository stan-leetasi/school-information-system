using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using project.Common.Tests;
using project.DAL.Entities;
using Xunit;
using Xunit.Abstractions;
using project.Common.Tests.Seeds;

namespace project.DAL.Tests;

// Test class for testing operations related to students in the DbContext
public class DbContextSubjectTests(ITestOutputHelper output) : DbContextTestsBase(output)
{
    // Test that a new subject can be added and will persist in the database
    [Fact]
    public async Task AddNew_Subject_Persisted()
    {
        //Arrange
        SubjectEntity entity = new()
        {
            // Generate new GUID for the subject
            Id = Guid.NewGuid() /*Parse("38871102 - c06f - 4f68 - 99eb - c0f6c0b2d33b")*/ ,
            Name = "C++",
            Acronym = "ICP"
        };

        //Act
        ProjectDbContextSUT.Subjects.Add(entity);
        await ProjectDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntities = await dbx.Subjects.SingleAsync(i => i.Id == entity.Id);

        DeepAssert.Equal(entity, actualEntities);
    }

    // Test that all subjects retrieved from the database contain ICS
    [Fact]
    public async Task GetAll_Subjects_ContainsICS()
    {
        //Act
        var entities = await ProjectDbContextSUT.Subjects.ToListAsync();

        //Assert
        DeepAssert.Contains(SubjectSeeds.ICS, entities, nameof(SubjectEntity.Students), nameof(SubjectEntity.Activities));
    }

    // Test that a specific subject (ICS) can be retrieved by ID
    [Fact]
    public async Task GetById_Subject_JohnRetrieved()
    {
        //Act
        var entity = await ProjectDbContextSUT.Subjects.SingleAsync(i => i.Id == SubjectSeeds.ICS.Id);

        //Assert
        DeepAssert.Equal(SubjectSeeds.ICS, entity, nameof(SubjectEntity.Students));
    }

    // Test that a specific subject (ICS) can be retrieved by ID and includes its students
    [Fact]
    public async Task GetById_IncludingStudents_Subject()
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
}
