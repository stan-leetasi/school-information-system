using Microsoft.EntityFrameworkCore;
using project.DAL.Entities;
using project.DAL.Seeds;

namespace project.DAL
{
    public class ProjectDbContext(DbContextOptions contextOptions, bool seedDemoData = false) : DbContext(contextOptions)
    {

        public DbSet<ActivityEntity> Activities => Set<ActivityEntity>();
        public DbSet<RatingEntity> Ratings => Set<RatingEntity>();
        public DbSet<StudentEntity> Students => Set<StudentEntity>();
        public DbSet<SubjectEntity> Subjects => Set<SubjectEntity>();
        public DbSet<StudentSubjectEntity> StudentSubjects => Set<StudentSubjectEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActivityEntity>()
                .HasMany(x => x.Ratings)
                .WithOne(x => x.Activity)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StudentEntity>()
                .HasMany(x => x.Ratings)
                .WithOne(x => x.Student)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<StudentEntity>()
                .HasMany(x => x.Subjects)
                .WithOne(x => x.Student)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SubjectEntity>()
                .HasMany(x => x.Activities)
                .WithOne(x => x.Subject)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<SubjectEntity>()
                .HasMany(x => x.Students)
                .WithOne(x => x.Subject)
                .OnDelete(DeleteBehavior.Cascade);

            if (seedDemoData)
            {
                StudentSeeds.Seed(modelBuilder);
                SubjectSeeds.Seed(modelBuilder);
                StudentSubjectSeeds.Seed(modelBuilder);
                ActivitiesSeeds.Seed(modelBuilder);
                RatingsSeeds.Seed(modelBuilder);
            }
        }
    }
}