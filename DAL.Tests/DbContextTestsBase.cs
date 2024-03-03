using project.Common.Tests;
using project.Common.Tests.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace project.DAL.Tests;
public class DbContextTestsBase : IAsyncLifetime
{
    protected DbContextTestsBase(ITestOutputHelper output)
    {
        XUnitTestOutputConverter converter = new(output);
        Console.SetOut(converter);

        DbContextFactory = new DbContextSqLiteTestingFactory(GetType().FullName!, seedTestingData: true);
        ProjectDbContextSUT = DbContextFactory.CreateDbContext();
    }

    protected IDbContextFactory<ProjectDbContext> DbContextFactory { get; }
    protected ProjectDbContext ProjectDbContextSUT { get; }


    public async Task InitializeAsync()
    {
        await ProjectDbContextSUT.Database.EnsureDeletedAsync();
        await ProjectDbContextSUT.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await ProjectDbContextSUT.Database.EnsureDeletedAsync();
        await ProjectDbContextSUT.DisposeAsync();
    }
}
