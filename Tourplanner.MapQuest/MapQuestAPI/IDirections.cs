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
        public Task<Route> FetchRouteAsync(string from, string to, Transport_type transportType);
    }
}
