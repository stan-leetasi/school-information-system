using project.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.DAL.Entities;

public class StudentSubjectEntity : IEntity
{
    public required Guid Id { get; set; }
    public required Guid StudentId { get; set; }
    public required StudentEntity Student { get; set; }
    public required Guid SubjectId { get; set; }
    public required SubjectEntity Subject { get; set; }
}
