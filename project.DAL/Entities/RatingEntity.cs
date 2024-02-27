using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.DAL.Entities;

public record RatingEntity : IEntity
{
    public required Guid Id { get; set; }
    public ushort Points { get; set; }
    public required string Notes { get; set; }
    public required Guid ActivityId { get; set; }
    public required Guid StudentId { get; set; }
    public required ActivityEntity Activity { get; set; }
    public required StudentEntity Student { get; set; }
}