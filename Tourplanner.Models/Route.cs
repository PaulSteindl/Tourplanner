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

        public double? TotalDistance { get; init; }

        public int TotalTime { get; init; }

        public string? StartLocation {  get; init; }

        public string? EndLocation { get; init; }

        public double? ul_lng { get; init; }

        public double? ul_lat { get; init; }

        public double? lr_lng { get; init; }

        public double? lr_lat { get; init; }

        public string? picPath { get; set; }
    }
}
