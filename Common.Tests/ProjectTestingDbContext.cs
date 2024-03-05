using Microsoft.EntityFrameworkCore;
using project.DAL;
using project.Common.Tests.Seeds;

namespace project.Common.Tests;

public class ProjectTestingDbContext(DbContextOptions contextOptions, bool seedTestingData = false)
    : ProjectDbContext(contextOptions, seedDemoData: false)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        if (seedTestingData)
        {
            StudentSeeds.Seed(modelBuilder);
            SubjectSeeds.Seed(modelBuilder);
            StudentSubjectSeeds.Seed(modelBuilder);
            ActivitiesSeeds.Seed(modelBuilder);
            RatingsSeeds.Seed(modelBuilder);
        }
    }
}
