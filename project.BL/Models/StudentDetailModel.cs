namespace project.BL.Models;

public record StudentDetailModel : ModelBase
{
    public required string Surname { get; set; }
    public required string Name { get; set; }
    public Uri? ImageUrl { get; set; }

    public static StudentDetailModel Empty =>
        new() { Id = Guid.Empty, Name = string.Empty, Surname = string.Empty, ImageUrl = new Uri("about:blank") };
}


