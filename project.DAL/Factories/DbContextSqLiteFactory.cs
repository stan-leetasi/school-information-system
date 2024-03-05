using Microsoft.EntityFrameworkCore;

namespace project.DAL.Factories;

public class DbContextSqLiteFactory : IDbContextFactory<ProjectDbContext>
{
    private readonly bool _seedTestingData;
    private readonly DbContextOptionsBuilder<ProjectDbContext> _contextOptionsBuilder = new();

    public DbContextSqLiteFactory(string databaseName, bool seedTestingData = false)
    {
        _seedTestingData = seedTestingData;

        _contextOptionsBuilder.UseSqlite($"Data Source={databaseName};Cache=Shared");
    }

    public ProjectDbContext CreateDbContext() => new(_contextOptionsBuilder.Options, _seedTestingData);
}
