namespace project.BL.Models;

public record StudentDetailModel : ModelBase
{
    public required string Surname { get; set; }
    public required string Name { get; set; }
    public string? ImageUrl { get; set; }

    public static StudentListModel Empty =>
        new() { Id = Guid.Empty, Name = string.Empty, Surname = string.Empty, };
}


