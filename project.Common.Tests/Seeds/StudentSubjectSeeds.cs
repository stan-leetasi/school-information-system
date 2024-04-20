using Microsoft.EntityFrameworkCore;
using project.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.Common.Tests.Seeds;

public static class StudentSubjectSeeds
{
    public static readonly StudentSubjectEntity JohnICS = new()
    {
        Id = Guid.Parse("f69514ff-4fea-43e8-bf91-ce62a88540a7"),
        StudentId = StudentSeeds.John.Id,
        SubjectId = SubjectSeeds.ICS.Id,

        Student = StudentSeeds.John,
        Subject = SubjectSeeds.ICS
    };

    public static readonly StudentSubjectEntity TerryIOS = new()
    {
        Id = Guid.Parse("e5fd1a8e-8b2a-4e6b-a0a1-9d5d76c5a514"),
        StudentId = StudentSeeds.Terry.Id,
        SubjectId = SubjectSeeds.IOS.Id,

        Student = StudentSeeds.Terry,
        Subject = SubjectSeeds.IOS
    };

    public static readonly StudentSubjectEntity TerryICS = new()
    {
        Id = Guid.Parse("2f7a3c91-9c7e-47f4-b729-6d9dcf8e3a20"),
        StudentId = StudentSeeds.Terry.Id,
        SubjectId = SubjectSeeds.ICS.Id,

        Student = StudentSeeds.Terry,
        Subject = SubjectSeeds.ICS
    };

    public static void Seed(this ModelBuilder modelBuilder) =>
        modelBuilder.Entity<StudentSubjectEntity>().HasData(
            JohnICS with {Student = null!, Subject = null!},
            TerryIOS with { Student = null!, Subject = null!},
            TerryICS with { Student = null!, Subject = null! }
        ); 
}
