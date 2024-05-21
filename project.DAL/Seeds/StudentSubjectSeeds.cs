using Microsoft.EntityFrameworkCore;
using project.DAL.Entities;

namespace project.DAL.Seeds;

public static class StudentSubjectSeeds
{
    public static readonly StudentSubjectEntity JohnICS = new()
    {
        Id = Guid.Parse("f69514ff-4fea-43e8-bf91-ce62a88540a7"),
        StudentId = StudentSeeds.JohnL.Id,
        SubjectId = SubjectSeeds.ICS.Id,

        Student = StudentSeeds.JohnL,
        Subject = SubjectSeeds.ICS
    };

    public static readonly StudentSubjectEntity TerryIOS = new()
    {
        Id = Guid.Parse("f69514ff-4fea-43e8-bf91-ce62a88540a8"),
        StudentId = StudentSeeds.Terry.Id,
        SubjectId = SubjectSeeds.IOS.Id,

        Student = StudentSeeds.Terry,
        Subject = SubjectSeeds.IOS
    };

    public static readonly StudentSubjectEntity TerryICS = new()
    {
        Id = Guid.Parse("f69514ff-4fea-43e8-bf91-ce62a88540a9"),
        StudentId = StudentSeeds.Terry.Id,
        SubjectId = SubjectSeeds.ICS.Id,

        Student = StudentSeeds.Terry,
        Subject = SubjectSeeds.ICS
    };

    public static readonly StudentSubjectEntity ElliotIOS = new()
    {
        Id = Guid.Parse("f69514ff-4fea-43e8-bf91-ce62a88540aa"),
        StudentId = StudentSeeds.Elliot.Id,
        SubjectId = SubjectSeeds.IOS.Id,

        Student = StudentSeeds.Elliot,
        Subject = SubjectSeeds.IOS
    };

    public static readonly StudentSubjectEntity ElliotIBS = new()
    {
        Id = Guid.Parse("f69514ff-4fea-43e8-bf91-ce62a88540ab"),
        StudentId = StudentSeeds.Elliot.Id,
        SubjectId = SubjectSeeds.IBS.Id,

        Student = StudentSeeds.Elliot,
        Subject = SubjectSeeds.IBS
    };

    public static readonly StudentSubjectEntity ElliotIVS = new()
    {
        Id = Guid.Parse("f69514ff-4fea-43e8-bf91-ce62a88540ac"),
        StudentId = StudentSeeds.Elliot.Id,
        SubjectId = SubjectSeeds.IVS.Id,

        Student = StudentSeeds.Elliot,
        Subject = SubjectSeeds.IVS
    };

    public static readonly StudentSubjectEntity TakeshiIBS = new()
    {
        Id = Guid.Parse("f69514ff-4fea-43e8-bf91-ce62a88540ad"),
        StudentId = StudentSeeds.Takeshi.Id,
        SubjectId = SubjectSeeds.IBS.Id,

        Student = StudentSeeds.Takeshi,
        Subject = SubjectSeeds.IBS
    };

    public static void Seed(this ModelBuilder modelBuilder) =>
        modelBuilder.Entity<StudentSubjectEntity>().HasData(
            JohnICS with {Student = null!, Subject = null!},
            TerryIOS with { Student = null!, Subject = null!},
            TerryICS with { Student = null!, Subject = null! },
            ElliotIOS with { Student = null!, Subject = null! },
            ElliotIBS with { Student = null!, Subject = null! },
            ElliotIVS with { Student = null!, Subject = null! },
            TakeshiIBS with { Student = null!, Subject = null! }
        ); 
}
