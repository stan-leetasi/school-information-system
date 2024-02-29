using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using project.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.DAL
{
    public class ProjectDbContext : DbContext
    {
        public ProjectDbContext(DbContextOptions contextOptions, bool seedDemoData = false) : base(contextOptions)
        {
        }

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

            modelBuilder.Entity<SubjectEntity>()
                .HasMany(x => x.Activities)
                .WithOne(x => x.Subject)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<StudentEntity>()
                .HasMany<StudentSubjectEntity>()
                .WithOne(x => x.Student)
                .OnDelete(DeleteBehavior.Restrict);

            // modelBuilder.Entity<RatingEntity>()
            //     .HasOne(x => x.Activity)
            //     .WithMany(x => x.Ratings)
            //     .OnDelete(DeleteBehavior.Restrict);

            // modelBuilder.Entity<SubjectEntity>()
            //     .HasMany<StudentSubjectEntity>()
            //     .WithOne(x => x.Subject)
            //     .OnDelete(DeleteBehavior.Restrict);
        }
    }
}