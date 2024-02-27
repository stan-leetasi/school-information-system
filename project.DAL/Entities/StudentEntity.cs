using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.DAL.Entities;

public record StudentEntity : IEntity
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public string? ImageUrl { get; set; }
    public ICollection<SubjectEntity> Subjects { get; set; } = new List<SubjectEntity>();
    public ICollection<RatingEntity> Ratings { get; set; } = new List<RatingEntity>();
}