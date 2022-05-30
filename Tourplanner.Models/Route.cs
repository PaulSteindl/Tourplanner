using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tourplanner.Models
{
    public record Route
    {
        public string? RouteId { get; init; }

        private double? TotalDistance { get; init; }

        public string? StartLocation {  get; init; }

        public string? EndLocation { get; init; }

    }
}
