using Microsoft.EntityFrameworkCore;
using project.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.Common.Tests.Seeds;
public static class StudentSeeds
{

    public static readonly StudentEntity John = new()
    {
        Id = Guid.Parse("a9267c9a-feda-4af2-8257-b389d231ecaa"),
        Name = "John",
        Surname = "Lark",
        ImageUrl = "www.photos.com/john.jpeg"
    };

    static StudentSeeds()
    {
        StudentSeeds.John.Subjects.Add(StudentSubjectSeeds.JohnICS);
    }

    public static void Seed(this ModelBuilder modelBuilder) =>
        modelBuilder.Entity<StudentEntity>().HasData(
            John with{ Subjects = [], Ratings = [] }
        );
}
