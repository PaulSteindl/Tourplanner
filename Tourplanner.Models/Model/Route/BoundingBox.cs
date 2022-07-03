using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tourplanner.Models
{
    public record BoundingBox
    {
        public LrUl Lr { get; set; }
        public LrUl Ul { get; set; }
    }
}
