using Microsoft.EntityFrameworkCore;
using project.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.Common.Tests.Seeds;

// Class containing seed data for the relationship between students and subjects
public static class StudentSubjectSeeds
{
    // Seed data for the relationship between John and the ICS subject
    public static readonly StudentSubjectEntity JohnICS = new()
    {
        Id = Guid.Parse("f69514ff-4fea-43e8-bf91-ce62a88540a7"),
        StudentId = StudentSeeds.John.Id,
        SubjectId = SubjectSeeds.ICS.Id,

        Student = StudentSeeds.John,
        Subject = SubjectSeeds.ICS
    };

    // Seed data for the relationship between Terry and the IOS subject
    public static readonly StudentSubjectEntity TerryIOS = new()
    {
        Id = Guid.Parse("e5fd1a8e-8b2a-4e6b-a0a1-9d5d76c5a514"),
        StudentId = StudentSeeds.Terry.Id,
        SubjectId = SubjectSeeds.IOS.Id,

        Student = StudentSeeds.Terry,
        Subject = SubjectSeeds.IOS
    };

    // Method to seed the relationship data into the model builder
    public static void Seed(this ModelBuilder modelBuilder) =>
        modelBuilder.Entity<StudentSubjectEntity>().HasData(
            JohnICS with {Student = null!, Subject = null!},
            TerryIOS with { Student = null!, Subject = null! }
        ); // Seed data for the relationship between John and the ICS subject
           // and Terry and the IOS subject with null references
}
