using Microsoft.EntityFrameworkCore;
using project.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.Common.Tests.Seeds
{
    public static class ActivitiesSeeds
    {
        public static readonly ActivityEntity ICSCviko = new()
        {

            Id = Guid.Parse("c7435c8b-30a1-4fc9-8e0b-36b3ff2ee8c5"),
            Area = Enums.SchoolArea.ComputerLab,
            BeginTime = new DateTime(2024, 3, 4, 9, 0, 0),
            EndTime = new DateTime(2024, 4, 4, 12, 0, 0),
            Description = "ICS Cvičenie",
            Subject = SubjectSeeds.ICS,
            SubjectId = SubjectSeeds.ICS.Id,
            Type = Enums.ActivityType.Exercise
        };
        public static readonly ActivityEntity IOSPolsemka = new()
        {
            Id = Guid.Parse("d0de8b41-9028-4f5f-82d8-7a3ae1bf8b18"),
            Area = Enums.SchoolArea.MainLectureHall,
            BeginTime = new DateTime(2024, 7, 1, 5, 0, 0),
            EndTime = new DateTime(2024, 7, 1, 8, 0, 0),
            Description = "IOS Polsemestralka",
            Subject = SubjectSeeds.IOS,
            SubjectId = SubjectSeeds.IOS.Id,
            Type = Enums.ActivityType.MidtermExam
        };

        public static readonly ActivityEntity EmptyActivityEntity = new()
        {
            Id = default,
            Area = default,
            BeginTime = default,
            Description = default,
            EndTime = default,
            Subject = default,
            SubjectId = default,
            Type = default
        };

        static ActivitiesSeeds()
        {
            ActivitiesSeeds.ICSCviko.Ratings.Add(RatingsSeeds.ICSRating);
            ActivitiesSeeds.IOSPolsemka.Ratings.Add(RatingsSeeds.IOSRating);
        }

        public static void Seed(this ModelBuilder modelBuilder) =>
            modelBuilder.Entity<ActivityEntity>().HasData(
                ICSCviko with { Subject = null!, Ratings = [] },
                IOSPolsemka with { Subject = null!, Ratings = [] });
    }
}
