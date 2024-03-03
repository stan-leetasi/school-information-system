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

    public static void Seed(this ModelBuilder modelBuilder) =>
        modelBuilder.Entity<StudentSubjectEntity>().HasData(
            JohnICS with {Student = null!, Subject = null!}
        );
}
