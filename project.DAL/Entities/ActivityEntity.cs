using project.Common.Enums;

namespace project.DAL.Entities;

public record ActivityEntity : IEntity
{
    public required Guid Id { get; set; }

    public required DateTime BeginTime { get; set; }
    public required DateTime EndTime { get; set; }
    public required SchoolArea Area { get; set; }
    public required ActivityType Type { get; set; }
    public required string Description { get; set; }

    public required Guid SubjectId { get; set; }
    public required SubjectEntity? Subject { get; set; }

    public ICollection<RatingEntity> Ratings { get; set; } = new List<RatingEntity>();
}