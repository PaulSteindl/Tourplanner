using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourplanner.Models;

namespace Tourplanner.BusinessLayer
{
    public interface IRouteManager
    {
        public Task<Route> GetFullRoute(string from, string to, Transport_type transportType);
    }
}
