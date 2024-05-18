using Microsoft.EntityFrameworkCore;
using project.DAL.Entities;

namespace project.DAL.Seeds;
public static class StudentSeeds
{
    public static readonly StudentEntity JohnL = new()
    {
        Id = Guid.Parse("4b28c9f6-8e4a-48a2-b7df-6e26e3e9c879"),
        Name = "John",
        Surname = "Lark",
        ImageUrl = new Uri("https://i.imgur.com/tE5E66T.png")
    };

    public static readonly StudentEntity Terry = new()
    {
        Id = Guid.Parse("4b28c9f6-8e4a-48a2-b7df-6e26e3e9c880"),
        Name = "Terry",
        Surname = "Davis",
        ImageUrl = new Uri("https://i.imgur.com/Qume1La.png")
    };
    
    public static readonly StudentEntity Elliot = new()
    {
        Id = Guid.Parse("4b28c9f6-8e4a-48a2-b7df-6e26e3e9c881"),
        Name = "Elliot",
        Surname = "Alderson",
        ImageUrl = new Uri("https://i.imgur.com/cfb5avx.png")
    };

    public static readonly StudentEntity Takeshi = new()
    {
        Id = Guid.Parse("4b28c9f6-8e4a-48a2-b7df-6e26e3e9c882"),
        Name = "Takeshi",
        Surname = "Kovacs",
        ImageUrl = new Uri("https://i.imgur.com/lu9MTXa.png")
    };

    public static readonly StudentEntity Hawkeye = new()
    {
        Id = Guid.Parse("4b28c9f6-8e4a-48a2-b7df-6e26e3e9c883"),
        Name = "Hawkeye",
        Surname = "Pierce",
        ImageUrl = new Uri("https://imgur.com/50N3h3w")
    };

    public static readonly StudentEntity JohnM = new()
    {
        Id = Guid.Parse("4b28c9f6-8e4a-48a2-b7df-6e26e3e9c884"),
        Name = "John",
        Surname = "McIntyre",
        ImageUrl = new Uri("https://imgur.com/lW0VYrG")
    };

    static StudentSeeds()
    {
        StudentSeeds.JohnL.Subjects.Add(StudentSubjectSeeds.JohnICS);
        StudentSeeds.Terry.Subjects.Add(StudentSubjectSeeds.TerryIOS);
        StudentSeeds.Terry.Subjects.Add(StudentSubjectSeeds.TerryICS);
        StudentSeeds.Elliot.Subjects.Add(StudentSubjectSeeds.ElliotIBS);
        StudentSeeds.Elliot.Subjects.Add(StudentSubjectSeeds.ElliotIOS);
        StudentSeeds.Elliot.Subjects.Add(StudentSubjectSeeds.ElliotIVS);
        StudentSeeds.Takeshi.Subjects.Add(StudentSubjectSeeds.TakeshiIBS);
    }

    public static void Seed(this ModelBuilder modelBuilder) =>
        modelBuilder.Entity<StudentEntity>().HasData(
            JohnL with{ Subjects = [], Ratings = [] },
            Terry with { Subjects = [], Ratings = [] },
            Elliot with { Subjects = [], Ratings = [] },
            Takeshi with { Subjects = [], Ratings = [] },
            Hawkeye with { Subjects = [], Ratings = [] },
            JohnM with { Subjects = [], Ratings = [] }
        );
}
