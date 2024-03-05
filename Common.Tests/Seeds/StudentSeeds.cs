using Microsoft.EntityFrameworkCore;
using project.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.Common.Tests.Seeds;
public static class StudentSeeds // Class containing seed data for StudentEntity
{

    public static readonly StudentEntity John = new() // Seed data for a student named John
    {
        Id = Guid.Parse("a9267c9a-feda-4af2-8257-b389d231ecaa"),
        Name = "John",
        Surname = "Lark",
        ImageUrl = "https://static.wikia.nocookie.net/paramount/images/e/ee/MV5BYWE5YTA4ZjgtNmZmMy00MzZjLTk2ZWYtOGNiZmJkNjYwMDMwXkEyXkFqcGdeQXVyNjYxMTE1OTM%40._V1_.jpg/revision/latest?cb=20220122213214"
    };

    public static readonly StudentEntity Terry = new() // Seed data for a student named John
    {
        Id = Guid.Parse("4b28c9f6-8e4a-48a2-b7df-6e26e3e9c879"),
        Name = "Terry",
        Surname = "Davis",
        ImageUrl = "https://images.findagrave.com/photos/2018/1/192900656_66373e86-549d-4b3c-94c9-914ea3210ab0.jpeg"
    };

    static StudentSeeds() // Static constructor to initialize seed data for John and Terry
    {
        // Adding a subject seed data for John and Terry
        StudentSeeds.John.Subjects.Add(StudentSubjectSeeds.JohnICS);
        StudentSeeds.Terry.Subjects.Add(StudentSubjectSeeds.TerryIOS);
        StudentSeeds.Terry.Subjects.Add(StudentSubjectSeeds.TerryICS);
    }

    // Seed the StudentEntity data into the model builder
    public static void Seed(this ModelBuilder modelBuilder) =>
        modelBuilder.Entity<StudentEntity>().HasData(
            John with{ Subjects = [], Ratings = [] },
            Terry with { Subjects = [], Ratings = [] }
        );
}
