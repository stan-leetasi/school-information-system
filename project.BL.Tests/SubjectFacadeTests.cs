using project.BL.Facades;
using project.BL.Models;
using project.Common.Tests;
using project.Common.Tests.Seeds;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace project.BL.Tests;

public sealed class SubjectFacadeTests : FacadeTestsBase
{
    private readonly ISubjectFacade _subjectFacadeSUT;

    public SubjectFacadeTests(ITestOutputHelper output) : base(output)
    {
        _subjectFacadeSUT = new SubjectFacade(UnitOfWorkFactory, SubjectModelMapper);
    }

    [Fact]
    public async Task Get_SubjectListModels_For_John()
    {
        //Act
        var listModels = await _subjectFacadeSUT.GetAsync(StudentSeeds.John.Id);

        IEnumerable<SubjectListModel> subjectListModels = listModels as SubjectListModel[] ?? listModels.ToArray();
        var ICS = subjectListModels.SingleOrDefault(s => s.Id == SubjectSeeds.ICS.Id);
        var IOS = subjectListModels.SingleOrDefault(s => s.Id == SubjectSeeds.IOS.Id);

        //Assert
        Assert.True(ICS!.IsRegistered && !IOS!.IsRegistered);
    }

    [Fact]
    public async Task Register_John_IOS()
    {
        // Arrange
        var listModels = await _subjectFacadeSUT.GetAsync(StudentSeeds.John.Id);
        var IOS = listModels.SingleOrDefault(s => s.Id == SubjectSeeds.IOS.Id);

        //Act
        await _subjectFacadeSUT.RegisterStudent(IOS!, StudentSeeds.John.Id);
        
        //Assert
        listModels = await _subjectFacadeSUT.GetAsync(StudentSeeds.John.Id);
        Assert.True(listModels.SingleOrDefault(s => s.Id == SubjectSeeds.IOS.Id)!.IsRegistered);
    }

    [Fact]
    public async Task Unregister_Terry_ICS()
    {
        // Arrange
        var subjectListModels = await _subjectFacadeSUT.GetAsync(StudentSeeds.Terry.Id);
        var ICS = subjectListModels.SingleOrDefault(s => s.Id == SubjectSeeds.ICS.Id);

        //Act
        await _subjectFacadeSUT.UnregisterStudent(ICS!, StudentSeeds.Terry.Id);

        //Assert
        subjectListModels = await _subjectFacadeSUT.GetAsync(StudentSeeds.Terry.Id);
        Assert.True(!subjectListModels.SingleOrDefault(s => s.Id == SubjectSeeds.ICS.Id)!.IsRegistered);
    }
}
