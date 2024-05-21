using Microsoft.EntityFrameworkCore;
using project.DAL.Entities;
using project.Common.Enums;

namespace project.DAL.Seeds
{
    public static class ActivitiesSeeds
    {
        public static readonly ActivityEntity ICSObhajoba = new()
        {
            Id = Guid.Parse("c7435c8b-30a1-4fc9-8e0b-36b3ff2ee8c4"),
            Area = SchoolArea.ComputerLab,
            BeginTime = new DateTime(2024, 5, 22, 9, 45, 0),
            EndTime = new DateTime(2024, 5, 22, 10, 0, 0),
            Description = "ICS Obhajoba Projektu",
            Subject = SubjectSeeds.ICS,
            SubjectId = SubjectSeeds.ICS.Id,
            Type = ActivityType.ProjectDefense
        };
        public static readonly ActivityEntity ICSCviko = new()
        {
            Id = Guid.Parse("c7435c8b-30a1-4fc9-8e0b-36b3ff2ee8c5"),
            Area = SchoolArea.ComputerLab,
            BeginTime = new DateTime(2024, 3, 4, 9, 0, 0),
            EndTime = new DateTime(2024, 3, 4, 12, 0, 0),
            Description = "ICS Cvičení",
            Subject = SubjectSeeds.ICS,
            SubjectId = SubjectSeeds.ICS.Id,
            Type = ActivityType.Exercise
        };
        public static readonly ActivityEntity IOSPolsemka = new()
        {
            Id = Guid.Parse("c7435c8b-30a1-4fc9-8e0b-36b3ff2ee8c6"),
            Area = SchoolArea.MainLectureHall,
            BeginTime = new DateTime(2024, 4, 5, 9, 0, 0),
            EndTime = new DateTime(2024, 4, 5, 10, 0, 0),
            Description = "IOS Polsemestrálka",
            Subject = SubjectSeeds.IOS,
            SubjectId = SubjectSeeds.IOS.Id,
            Type = ActivityType.MidtermExam
        };

        public static readonly ActivityEntity IOSSemka = new()
        {
            Id = Guid.Parse("c7435c8b-30a1-4fc9-8e0b-36b3ff2ee8c7"),
            Area = SchoolArea.MainLectureHall,
            BeginTime = new DateTime(2024, 5, 28, 14, 0, 0),
            EndTime = new DateTime(2024, 5, 28, 15, 50, 0),
            Description = "IOS Semestrálka",
            Subject = SubjectSeeds.IOS,
            SubjectId = SubjectSeeds.IOS.Id,
            Type = ActivityType.FinalExam
        };

        public static readonly ActivityEntity IBSLabak = new()
        {
            Id = Guid.Parse("c7435c8b-30a1-4fc9-8e0b-36b3ff2ee8c8"),
            Area = SchoolArea.ElectronicsLab,
            BeginTime = new DateTime(2025, 4, 5, 8, 0, 0),
            EndTime = new DateTime(2025, 4, 5, 10, 0, 0),
            Description = "IBS Laboratoř",
            Subject = SubjectSeeds.IBS,
            SubjectId = SubjectSeeds.IBS.Id,
            Type = ActivityType.Exercise
        };

        public static readonly ActivityEntity IVSObhajoba = new()
        {
            Id = Guid.Parse("c7435c8b-30a1-4fc9-8e0b-36b3ff2ee8c9"),
            Area = SchoolArea.ComputerLab,
            BeginTime = new DateTime(2024, 5, 1, 10, 0, 0),
            EndTime = new DateTime(2024, 5, 1, 12, 0, 0),
            Description = "IVS Obhajoby projektů",
            Subject = SubjectSeeds.IVS,
            SubjectId = SubjectSeeds.IVS.Id,
            Type = ActivityType.ProjectDefense
        };

        public static readonly ActivityEntity ITSSkuska = new()
        {
            Id = Guid.Parse("c7435c8b-30a1-4fc9-8e0b-36b3ff2ee8d0"),
            Area = SchoolArea.MainLectureHall,
            BeginTime = new DateTime(2024, 5, 22, 13, 0, 0),
            EndTime = new DateTime(2024, 5, 22, 14, 50, 0),
            Description = "ITS Závěrečná zkouška",
            Subject = SubjectSeeds.ITS,
            SubjectId = SubjectSeeds.ITS.Id,
            Type = ActivityType.FinalExam
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
            ActivitiesSeeds.ICSCviko.Ratings.Add(RatingsSeeds.ICSCvikoRatingJohnL);
            ActivitiesSeeds.ICSCviko.Ratings.Add(RatingsSeeds.ICSObhajobaRatingJohnL);
            ActivitiesSeeds.ICSCviko.Ratings.Add(RatingsSeeds.ICSCvikoRatingTerry);
            ActivitiesSeeds.ICSCviko.Ratings.Add(RatingsSeeds.ICSObhajobaRatingTerry);
            ActivitiesSeeds.IOSPolsemka.Ratings.Add(RatingsSeeds.IOSPolsemkaRatingTerry);
            ActivitiesSeeds.IOSPolsemka.Ratings.Add(RatingsSeeds.IOSPolsemkaRatingElliot);
            ActivitiesSeeds.IBSLabak.Ratings.Add(RatingsSeeds.IBSRatingElliot);
            ActivitiesSeeds.IBSLabak.Ratings.Add(RatingsSeeds.IBSRatingTakeshi);
            ActivitiesSeeds.IVSObhajoba.Ratings.Add(RatingsSeeds.IVSRatingElliot);

        }

        public static void Seed(this ModelBuilder modelBuilder) =>
            modelBuilder.Entity<ActivityEntity>().HasData(
                ICSCviko with { Subject = null!, Ratings = [] },
                ICSObhajoba with { Subject = null!, Ratings = [] },
                IOSPolsemka with { Subject = null!, Ratings = [] },
                IOSSemka with { Subject = null!, Ratings = [] },
                IBSLabak with { Subject = null!, Ratings = [] },
                IVSObhajoba with { Subject = null!, Ratings = [] },
                ITSSkuska with { Subject = null!, Ratings = [] }
                );
    }
}
