using project.Common.Enums;
using project.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.BL.Models;

public record ActivityStudentListModel : ModelBase
{
    public required DateTime BeginTime { get; set; }
    public required DateTime EndTime { get; set; }
    public required SchoolArea Area { get; set; }
    public required ActivityType Type { get; set; }
    public int Points { get; set; }
    public required string Name { get; set; }
    public required Guid Id { get; set; }
    public required Guid StudentId { get; set; }
    public required StudentEntity? Student { get; init; }
    public required Guid SubjectId { get; set; }
    public required SubjectEntity? Subject { get; init; }

    public static ActivityStudentListModel Empty => new()
    {
        BeginTime = DateTime.MinValue,
        EndTime = DateTime.MinValue,
        Area = SchoolArea.None,
        Type = ActivityType.None,
        Points = 0,
        Name = string.Empty,
        Id = Guid.Empty,
        StudentId = Guid.Empty,
        Student = null,
        Subject = null,
        SubjectId = Guid.Empty,
    };
}

