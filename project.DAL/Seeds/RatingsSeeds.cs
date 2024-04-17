using Microsoft.EntityFrameworkCore;
using project.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.DAL.Seeds
{
    public static class RatingsSeeds
    {
        public static readonly RatingEntity ICSRating = new()
        {
            Id = Guid.Parse("5d85804a-7ab0-4d38-b449-cc3f68887c38"),
            ActivityId = ActivitiesSeeds.ICSCviko.Id,
            Activity = ActivitiesSeeds.ICSCviko,
            Notes = "Skvělé",
            Points = 91,
            StudentId = StudentSeeds.John.Id,
            Student = StudentSeeds.John

        };
        public static readonly RatingEntity IOSRating = new()
        {

            Id = Guid.Parse("f3f2cb0b-9d2e-473b-a82c-7e4dd6a56a6a"),
            ActivityId = ActivitiesSeeds.IOSPolsemka.Id,
            Activity = ActivitiesSeeds.IOSPolsemka,
            Notes = "Vypadá to že tomu fakt rozumíte",
            Points = 98,
            StudentId = StudentSeeds.Terry.Id,
            Student = StudentSeeds.Terry
        };


        public static void Seed(this ModelBuilder modelBuilder) =>
            modelBuilder.Entity<RatingEntity>().HasData(
                ICSRating with { Activity = null!, Student = null! },
                IOSRating with { Activity = null!, Student = null! });
    }
}
