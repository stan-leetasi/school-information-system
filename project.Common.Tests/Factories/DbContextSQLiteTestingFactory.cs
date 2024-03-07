using project.DAL;
using Microsoft.EntityFrameworkCore;

namespace project.Common.Tests.Factories;

// Create a SQLite-based test instance of the DbContext
public class DbContextSqLiteTestingFactory(string databaseName, bool seedTestingData = false)
    : IDbContextFactory<ProjectDbContext>
{
    // Create a new instance of the DbContext for testing
    public ProjectDbContext CreateDbContext()
    {
        DbContextOptionsBuilder<ProjectDbContext> builder = new(); // Configure DbContext options for SQLite
        builder.UseSqlite($"Data Source={databaseName};Cache=Shared");

        // Return a new instance of the test DbContext with the specified options and seed data option
        return new ProjectTestingDbContext(builder.Options, seedTestingData);
    }
}
