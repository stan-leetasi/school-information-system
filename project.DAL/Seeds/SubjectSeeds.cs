using Microsoft.EntityFrameworkCore;
using project.DAL.Entities;

namespace project.DAL.Seeds;
public static class SubjectSeeds
{
    public static readonly SubjectEntity ICS = new()
    {
        Id = Guid.Parse("2046b766-7c8b-4dd0-93c7-2ab286d73aff"),
        Name = "Seminár C#",
        Acronym = "ICS",
    };

    public static readonly SubjectEntity IOS = new()
    {
        Id = Guid.Parse("2046b766-7c8b-4dd0-93c7-2ab286d73b00"),
        Name = "Operační systémy",
        Acronym = "IOS",
    };

    public static readonly SubjectEntity IBS = new()
    {
        Id = Guid.Parse("2046b766-7c8b-4dd0-93c7-2ab286d73b01"),
        Name = "Bezpečnost a počítačové sítě",
        Acronym = "IBS",
    };

    public static readonly SubjectEntity IVS = new()
    {
        Id = Guid.Parse("2046b766-7c8b-4dd0-93c7-2ab286d73b02"),
        Name = "Praktické aspekty vývoje software",
        Acronym = "IVS",
    };

    public static readonly SubjectEntity ITS = new()
    {
        Id = Guid.Parse("2046b766-7c8b-4dd0-93c7-2ab286d73b03"),
        Name = "Testování a dynamická analýza",
        Acronym = "ITS",
    };

    static SubjectSeeds()
    {
        ICS.Students.Add(StudentSubjectSeeds.JohnICS);
        ICS.Students.Add(StudentSubjectSeeds.TerryICS);

        IOS.Students.Add(StudentSubjectSeeds.TerryIOS);
        IOS.Students.Add(StudentSubjectSeeds.ElliotIOS);

        IBS.Students.Add(StudentSubjectSeeds.ElliotIBS);
        IBS.Students.Add(StudentSubjectSeeds.TakeshiIBS);

        IVS.Students.Add(StudentSubjectSeeds.ElliotIVS);

    }

    public static void Seed(this ModelBuilder modelBuilder) =>
        modelBuilder.Entity<SubjectEntity>().HasData(
            ICS with{ Students = [], Activities = [] },
            IOS with { Students = [], Activities = [] },
            IBS with { Students = [], Activities = [] },
            IVS with { Students = [], Activities = [] },
            ITS with { Students = [], Activities = [] }
        );
}
