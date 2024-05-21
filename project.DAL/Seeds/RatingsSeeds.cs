using Microsoft.EntityFrameworkCore;
using project.DAL.Entities;

namespace project.DAL.Seeds
{
    public static class RatingsSeeds
    {
        public static readonly RatingEntity ICSObhajobaRatingJohnL = new()
        {
            Id = Guid.Parse("5d85804a-7ab0-4d38-b449-cc3f68887c36"),
            ActivityId = ActivitiesSeeds.ICSObhajoba.Id,
            Activity = ActivitiesSeeds.ICSObhajoba,
            Notes = "Máte štěstí že se vám to vůbec spustilo",
            Points = 15,
            StudentId = StudentSeeds.JohnL.Id,
            Student = StudentSeeds.JohnL
        };

        public static readonly RatingEntity ICSCvikoRatingJohnL = new()
        {
            Id = Guid.Parse("5d85804a-7ab0-4d38-b449-cc3f68887c37"),
            ActivityId = ActivitiesSeeds.ICSCviko.Id,
            Activity = ActivitiesSeeds.ICSCviko,
            Notes = "Skvělé",
            Points = 8,
            StudentId = StudentSeeds.JohnL.Id,
            Student = StudentSeeds.JohnL
        };

        public static readonly RatingEntity IOSPolsemkaRatingTerry = new()
        {

            Id = Guid.Parse("5d85804a-7ab0-4d38-b449-cc3f68887c38"),
            ActivityId = ActivitiesSeeds.IOSPolsemka.Id,
            Activity = ActivitiesSeeds.IOSPolsemka,
            Notes = "Vypadá to že tomu fakt rozumíte",
            Points = 15,
            StudentId = StudentSeeds.Terry.Id,
            Student = StudentSeeds.Terry
        };
        
        public static readonly RatingEntity ICSObhajobaRatingTerry = new()
        {
            Id = Guid.Parse("5d85804a-7ab0-4d38-b449-cc3f68887c39"),
            ActivityId = ActivitiesSeeds.ICSObhajoba.Id,
            Activity = ActivitiesSeeds.ICSObhajoba,
            Notes = "Můžete jít ďelat do Microsoftu",
            Points = 52,
            StudentId = StudentSeeds.Terry.Id,
            Student = StudentSeeds.Terry
        };
        
        public static readonly RatingEntity ICSCvikoRatingTerry = new()
        {
            Id = Guid.Parse("5d85804a-7ab0-4d38-b449-cc3f68887c40"),
            ActivityId = ActivitiesSeeds.ICSCviko.Id,
            Activity = ActivitiesSeeds.ICSCviko,
            Notes = "Dalo by se zlepšit",
            Points = 5,
            StudentId = StudentSeeds.Terry.Id,
            Student = StudentSeeds.Terry
        };

        public static readonly RatingEntity IOSPolsemkaRatingElliot = new()
        {
            Id = Guid.Parse("5d85804a-7ab0-4d38-b449-cc3f68887c41"),
            ActivityId = ActivitiesSeeds.IOSPolsemka.Id,
            Activity = ActivitiesSeeds.IOSPolsemka,
            Notes = "Velmi dobré",
            Points = 14,
            StudentId = StudentSeeds.Elliot.Id,
            Student = StudentSeeds.Elliot
        };

        public static readonly RatingEntity IBSRatingElliot = new()
        {
            Id = Guid.Parse("5d85804a-7ab0-4d38-b449-cc3f68887c42"),
            ActivityId = ActivitiesSeeds.IBSLabak.Id,
            Activity = ActivitiesSeeds.IBSLabak,
            Notes = "Skvělá implementace",
            Points = 5,
            StudentId = StudentSeeds.Elliot.Id,
            Student = StudentSeeds.Elliot
        };

        public static readonly RatingEntity IVSRatingElliot = new()
        {
            Id = Guid.Parse("5d85804a-7ab0-4d38-b449-cc3f68887c43"),
            ActivityId = ActivitiesSeeds.IVSObhajoba.Id,
            Activity = ActivitiesSeeds.IVSObhajoba,
            Notes = "Skvělá implementace kalkulačky",
            Points = 40,
            StudentId = StudentSeeds.Elliot.Id,
            Student = StudentSeeds.Elliot
        };

        public static readonly RatingEntity IBSRatingTakeshi = new()
        {
            Id = Guid.Parse("5d85804a-7ab0-4d38-b449-cc3f68887c44"),
            ActivityId = ActivitiesSeeds.IBSLabak.Id,
            Activity = ActivitiesSeeds.IBSLabak,
            Notes = "Hrůza",
            Points = 2,
            StudentId = StudentSeeds.Takeshi.Id,
            Student = StudentSeeds.Takeshi
        };

        public static readonly RatingEntity EmptyRatingEntity = new()
        {
            Id = default,
            Points = default,
            Notes = default,
            ActivityId = default,
            StudentId = default,
            Activity = default,
            Student = default,
        };

        public static void Seed(this ModelBuilder modelBuilder) =>
            modelBuilder.Entity<RatingEntity>().HasData(
                ICSCvikoRatingJohnL with { Activity = null!, Student = null! },
                ICSObhajobaRatingJohnL with { Activity = null!, Student = null! },
                IOSPolsemkaRatingTerry with { Activity = null!, Student = null! },
                ICSCvikoRatingTerry with { Activity = null!, Student = null! },
                ICSObhajobaRatingTerry with { Activity = null!, Student = null! },
                IOSPolsemkaRatingElliot with { Activity = null!, Student = null! },
                IVSRatingElliot with { Activity = null!, Student = null! },
                IBSRatingElliot with { Activity = null!, Student = null! },
                IBSRatingTakeshi with { Activity = null!, Student = null! }
                );
    }
}
