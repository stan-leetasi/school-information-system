using Microsoft.EntityFrameworkCore.Design;
using System.Data.Common;

namespace project.DAL.Factories
{
    public class DesignTimeDbFactory : IDesignTimeDbContextFactory<ProjectDbContext>
    {
        private readonly DbContextSqLiteFactory _dbContextSqLiteFactory = new($"schoolIS.db");
        public ProjectDbContext CreateDbContext(string[] args) => _dbContextSqLiteFactory.CreateDbContext();
    }
}