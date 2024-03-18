using Microsoft.VisualBasic;
using project.Common.Enums;
using project.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.BL.Models;

public record ActivityListModel : ModelBase
{
    public required DateTime BeginTime { get; set; }
    public required DateTime EndTime { get; set; }
    public required SchoolArea Area { get; set; }
    public required ActivityType Type { get; set; }
    public int NumberOfStudent { get; set; }
    public int Points { get; set; }
    public bool IsRegistered { get; set; }
    public required SubjectEntity? Subject { get; set; }


    public static ActivityListModel Empty => new()
    {
        Id = Guid.NewGuid(),
        BeginTime = DateTime.MinValue,
        EndTime = DateTime.MinValue,
        Area = SchoolArea.None,
        Type = ActivityType.None,
        NumberOfStudent = 0,
        Points = 0,
        IsRegistered = false,
        Subject = null,
    };
}
