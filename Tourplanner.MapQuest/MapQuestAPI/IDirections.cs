using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourplanner.Models;

public interface IDirections
{
    public Task<byte[]> FetchMapAsync(Route route);
    public Task<Route> FetchRouteAsync(string from, string to, TransportType transportType);
}
