using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.DAL.Entities;

public record SubjectEntity : IEntity
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Acronym { get; set; }
    public ICollection<StudentEntity> Students { get; set; } = new List<StudentEntity>();
    public ICollection<ActivityEntity> Activities { get; set; } = new List<ActivityEntity>();
}