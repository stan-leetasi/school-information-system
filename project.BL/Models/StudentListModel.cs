using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.BL.Models;

public record StudentListModel: ModelBase
{
    public required string Surname { get; set; }
    public required string Name { get; set; }

    public static StudentListModel Empty
        => new() { Id = Guid.Empty, Name = string.Empty, Surname = string.Empty, };
}

