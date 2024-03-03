using Microsoft.EntityFrameworkCore;
using project.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.Common.Tests.Seeds;
public static class SubjectSeeds // Class containing seed data for subjects
{

    public static readonly SubjectEntity ICS = new() // Seed data for the ICS subject
    {
        Id = Guid.Parse("2046b766-7c8b-4dd0-93c7-2ab286d73aff"),
        Name = "C Sharp",
        Acronym = "ICS",
    };

    public static readonly SubjectEntity IOS = new() // Seed data for the IZG subject
    {
        Id = Guid.Parse("7b248a9c-30e1-4b2d-b0f3-0c0c8b76a2f7"), // Replace "your-guid-here" with the desired GUID
        Name = "Operacne Systemy",
        Acronym = "IOS",
    };

    static SubjectSeeds() // Constructor to initialize seed data for the subjects
    {
        ICS.Students.Add(StudentSubjectSeeds.JohnICS);
        ICS.Students.Add(StudentSubjectSeeds.TerryIOS);
    }

    // Method to seed subject data into the model builder
    public static void Seed(this ModelBuilder modelBuilder) =>
        modelBuilder.Entity<SubjectEntity>().HasData(
            ICS with{ Students = [], Activities = [] },
            IOS with { Students = [], Activities = [] }
        ); // Seed data for the ICS and IOS subject with empty student and activity lists
}
