using Microsoft.EntityFrameworkCore;
using project.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.Common.Tests.Seeds
{
    // Class containing seed data for ratings
    public static class RatingsSeeds
    {
        // Seed data for ratings
        public static readonly List<RatingEntity> ICSRatings = new()
        {
            new RatingEntity
            {
                Id = Guid.Parse("5d85804a-7ab0-4d38-b449-cc3f68887c38"),
                ActivityId = ActivitiesSeeds.Activities[0].Id,
                Activity = ActivitiesSeeds.Activities[0],
                Notes = "Skvělé",
                Points = 91,
                StudentId = StudentSeeds.John.Id,
                Student = StudentSeeds.John
            },
            new RatingEntity
            {
                Id = Guid.Parse("76c69c3e-3ed9-49b0-9671-5ee19f5b42ac"),
                ActivityId = ActivitiesSeeds.Activities[0].Id,
                Activity = ActivitiesSeeds.Activities[0],
                Notes = "Mohlo by být lepší",
                Points = 70,
                StudentId = StudentSeeds.Terry.Id,
                Student = StudentSeeds.Terry
            }
        };
        public static readonly List<RatingEntity> IOSRatings = new()
        {
            new RatingEntity
            {
                Id = Guid.Parse("b716a18d-3d8d-4b56-9e3a-2d4b92b63d5e"),
                ActivityId = ActivitiesSeeds.Activities[1].Id,
                Activity = ActivitiesSeeds.Activities[1],
                Notes = "Hrůza",
                Points = 20,
                StudentId = StudentSeeds.John.Id,
                Student = StudentSeeds.John
            },
            new RatingEntity
            {
                Id = Guid.Parse("f3f2cb0b-9d2e-473b-a82c-7e4dd6a56a6a"),
                ActivityId = ActivitiesSeeds.Activities[1].Id,
                Activity = ActivitiesSeeds.Activities[1],
                Notes = "Vypadá to že tomu fakt rozumíte",
                Points = 98,
                StudentId = StudentSeeds.Terry.Id,
                Student = StudentSeeds.Terry
            }
        };

        // Method to seed rating data into the model builder
        public static void Seed(this ModelBuilder modelBuilder) =>
            modelBuilder.Entity<RatingEntity>().HasData(ICSRatings, IOSRatings);
    }
}
