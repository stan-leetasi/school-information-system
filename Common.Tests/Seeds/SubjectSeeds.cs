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

    static SubjectSeeds()
    {
        ICS.Students.Add(StudentSubjectSeeds.JohnICS);
    }

    public static void Seed(this ModelBuilder modelBuilder) =>
        modelBuilder.Entity<SubjectEntity>().HasData(
            ICS with{ Students = [], Activities = [] }
        );
}
