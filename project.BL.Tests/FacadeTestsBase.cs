using project.BL.Mappers;
using project.Common.Tests;
using project.Common.Tests.Factories;
using project.DAL;
using project.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace project.BL.Tests;

public class FacadeTestsBase : IAsyncLifetime
{
    protected FacadeTestsBase(ITestOutputHelper output)
    {
        XUnitTestOutputConverter converter = new(output);
        Console.SetOut(converter);

        DbContextFactory = new DbContextSqLiteTestingFactory(GetType().FullName!, seedTestingData: true);

        StudentModelMapper = new StudentModelMapper();
        RatingModelMapper = new RatingModelMapper();
        ActivityModelMapper = new ActivityModelMapper(RatingModelMapper);
        SubjectModelMapper = new SubjectModelMapper(ActivityModelMapper, StudentModelMapper);

        UnitOfWorkFactory = new UnitOfWorkFactory(DbContextFactory);
    }

    protected IDbContextFactory<ProjectDbContext> DbContextFactory { get; }

    protected StudentModelMapper StudentModelMapper { get; }
    protected SubjectModelMapper SubjectModelMapper { get; }
    protected RatingModelMapper RatingModelMapper { get; }
    protected ActivityModelMapper ActivityModelMapper { get; }

    protected UnitOfWorkFactory UnitOfWorkFactory { get; }

    public async Task InitializeAsync()
    {
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        await dbx.Database.EnsureDeletedAsync();
        await dbx.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        await dbx.Database.EnsureDeletedAsync();
    }
}
