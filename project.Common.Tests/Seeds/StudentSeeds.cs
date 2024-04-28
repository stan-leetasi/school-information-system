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
        Id = Guid.Parse("4b28c9f6-8e4a-48a2-b7df-6e26e3e9c879"),
        Name = "John",
        Surname = "Lark",
        ImageUrl = "https://static.wikia.nocookie.net/paramount/images/e/ee/MV5BYWE5YTA4ZjgtNmZmMy00MzZjLTk2ZWYtOGNiZmJkNjYwMDMwXkEyXkFqcGdeQXVyNjYxMTE1OTM%40._V1_.jpg/revision/latest?cb=20220122213214"
    };

    public static readonly StudentEntity Terry = new()
    {
        Id = Guid.Parse("4b28c9f6-8e4a-48a2-b7df-6e26e3e9c880"),
        Name = "Terry",
        Surname = "Davis",
        ImageUrl = "https://images.findagrave.com/photos/2018/1/192900656_66373e86-549d-4b3c-94c9-914ea3210ab0.jpeg"
    };
    
    public static readonly StudentEntity Elliot = new()
    {
        Id = Guid.Parse("4b28c9f6-8e4a-48a2-b7df-6e26e3e9c881"),
        Name = "Elliot",
        Surname = "Alderson",
        ImageUrl = "https://www.fayobserver.com/gcdn/authoring/2016/07/10/NTFO/ghows-NC-c889c372-7612-4a09-83c8-fd8e79ad419f-76f7c825.jpeg?width=1200&disable=upscale&format=pjpg&auto=webp"
    };

    public static readonly StudentEntity Takeshi = new()
    {
        Id = Guid.Parse("4b28c9f6-8e4a-48a2-b7df-6e26e3e9c882"),
        Name = "Takeshi",
        Surname = "Kovacs",
        ImageUrl = "https://i.insider.com/5a7478f27101ad48f7334ecc?width=700"
    };

    public static readonly StudentEntity Hawkeye = new()
    {
        Id = Guid.Parse("4b28c9f6-8e4a-48a2-b7df-6e26e3e9c883"),
        Name = "Hawkeye",
        Surname = "Pierce",
        ImageUrl = "https://static.wikia.nocookie.net/mash/images/2/21/Hawk_hd_ready.jpg/revision/latest?cb=20220118001314"
    };

    static StudentSeeds()
    {
        StudentSeeds.John.Subjects.Add(StudentSubjectSeeds.JohnICS);
        StudentSeeds.Terry.Subjects.Add(StudentSubjectSeeds.TerryIOS);
        StudentSeeds.Terry.Subjects.Add(StudentSubjectSeeds.TerryICS);
        StudentSeeds.Elliot.Subjects.Add(StudentSubjectSeeds.ElliotIBS);
        StudentSeeds.Elliot.Subjects.Add(StudentSubjectSeeds.ElliotIOS);
        StudentSeeds.Elliot.Subjects.Add(StudentSubjectSeeds.ElliotIVS);
        StudentSeeds.Takeshi.Subjects.Add(StudentSubjectSeeds.TakeshiIBS);
    }

    public static void Seed(this ModelBuilder modelBuilder) =>
        modelBuilder.Entity<StudentEntity>().HasData(
            John with{ Subjects = [], Ratings = [] },
            Terry with { Subjects = [], Ratings = [] },
            Elliot with { Subjects = [], Ratings = [] },
            Takeshi with { Subjects = [], Ratings = [] },
            Hawkeye with { Subjects = [], Ratings = [] }
        );
}
