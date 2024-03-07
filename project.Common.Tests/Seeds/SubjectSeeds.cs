using Microsoft.EntityFrameworkCore;
using project.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.Common.Tests.Seeds;
public static class SubjectSeeds
{
    public static readonly SubjectEntity ICS = new()
    {
        Id = Guid.Parse("2046b766-7c8b-4dd0-93c7-2ab286d73aff"),
        Name = "C Sharp",
        Acronym = "ICS",
    };

    public static readonly SubjectEntity IOS = new()
    {
        Id = Guid.Parse("7b248a9c-30e1-4b2d-b0f3-0c0c8b76a2f7"),
        Name = "Operacne Systemy",
        Acronym = "IOS",
    };

    static SubjectSeeds()
    {
        ICS.Students.Add(StudentSubjectSeeds.JohnICS);
        ICS.Students.Add(StudentSubjectSeeds.TerryICS);
        IOS.Students.Add(StudentSubjectSeeds.TerryIOS);
    }

    public static void Seed(this ModelBuilder modelBuilder) =>
        modelBuilder.Entity<SubjectEntity>().HasData(
            ICS with{ Students = [], Activities = [] },
            IOS with { Students = [], Activities = [] }
        );
}
