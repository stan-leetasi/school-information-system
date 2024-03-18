using project.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.BL.Models;

public record SubjectListModel : ModelBase
{
    public required string Name { get; set; }
    public required string Acronym { get; set; }
    public bool IsRegistered { get; set; }

    public static SubjectListModel Empty => new()
    {
        Id = Guid.Empty, Name = string.Empty, Acronym = string.Empty, IsRegistered = false,
    };
}
