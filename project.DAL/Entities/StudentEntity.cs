namespace project.DAL.Entities;

public record StudentEntity : IEntity
{
    public required Guid Id { get; set; }

    public required string Name { get; set; }
    public required string Surname { get; set; }
    public Uri? ImageUrl { get; set; }

    public ICollection<StudentSubjectEntity> Subjects { get; init; } = new List<StudentSubjectEntity>();

    public ICollection<RatingEntity> Ratings { get; init; } = new List<RatingEntity>();
}