using project.DAL;
using Microsoft.EntityFrameworkCore;

namespace project.Common.Tests.Factories;

public class DbContextSqLiteTestingFactory(string databaseName, bool seedTestingData = false)
    : IDbContextFactory<ProjectDbContext>
{
    public ProjectDbContext CreateDbContext()
    {
        DbContextOptionsBuilder<ProjectDbContext> builder = new();
        builder.UseSqlite($"Data Source={databaseName};Cache=Shared");

        return new ProjectTestingDbContext(builder.Options, seedTestingData);
    }
}
