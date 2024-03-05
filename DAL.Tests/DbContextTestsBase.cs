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
public class DbContextTestsBase : IAsyncLifetime // Base class for setting up DbContext-related tests
{
    protected DbContextTestsBase(ITestOutputHelper output) // Constructor for initializing test output and DbContext-related components
    {
        XUnitTestOutputConverter converter = new(output); // Redirect console output to XUnit test output
        Console.SetOut(converter);

        // Initialize DbContext factory with SQLite for testing
        DbContextFactory = new DbContextSqLiteTestingFactory(GetType().FullName!, seedTestingData: true);

        // Create a new instance of the DbContext for testing
        ProjectDbContextSUT = DbContextFactory.CreateDbContext(); 
    }

    protected IDbContextFactory<ProjectDbContext> DbContextFactory { get; } // Property for accessing the DbContext factory
    protected ProjectDbContext ProjectDbContextSUT { get; } // Property for accessing the instance of the DbContext under test

    // Initialize the database before each test
    public async Task InitializeAsync()
    {
        // Delete and create the database before each test
        await ProjectDbContextSUT.Database.EnsureDeletedAsync();
        await ProjectDbContextSUT.Database.EnsureCreatedAsync();
    }

    // Dispose of resources after each test
    public async Task DisposeAsync()
    {
        // Delete the database and dispose DbContet after each test
        await ProjectDbContextSUT.Database.EnsureDeletedAsync();
        await ProjectDbContextSUT.DisposeAsync();
    }
}
