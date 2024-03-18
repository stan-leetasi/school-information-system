using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.BL.Models;

public record StudentDetailModel : ModelBase
{
        public required string Surname { get; set; }
        public required string Name { get; set; }
        public string? ImageUrl { get; set; }
        public required Guid Id { get; set; }

        public static StudentListModel Empty => new()
        {
            Id = Guid.NewGuid(),
            Surname = string.Empty,
            Name = string.Empty,
            //ImageUrl = string.Empty,
        };
}


