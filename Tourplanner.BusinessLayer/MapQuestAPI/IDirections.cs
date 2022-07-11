using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourplanner.Models;

namespace Tourplanner.BusinessLayer
{
    public interface IDirections
    {
        public Task<byte[]> FetchMapAsync(Route route);
        public Task<RouteInfo> FetchRouteAsync(string from, string to, TransportType transportType);
    }
}
