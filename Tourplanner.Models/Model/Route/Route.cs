using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tourplanner.Models
{
    public record Route
    {
        public BoundingBox? BoundingBox { get; set; }
        public double Distance { get; set; }
        public string? SessionId { get; set; }
        public int Time { get; set; }
        public string? PicPath { get; set; }
    }
}
