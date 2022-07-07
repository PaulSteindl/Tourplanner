using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourplanner.DataAccessLayer;
using Tourplanner.Models;

namespace Tourplanner.BusinessLayer
{
    public class RouteManager : IRouteManager
    {
        private readonly IFileDAO fileDAO;
        private readonly IDirections directions;

        public RouteManager(IFileDAO fileDAO, IDirections directions)
        {
            this.fileDAO = fileDAO;
            this.directions = directions;
        }

        public async Task<Route> GetFullRoute(string from, string to, TransportType transportType, Guid tourId)
        {
            byte[]? mapArray = null;
            var route = await directions.FetchRouteAsync(from, to, transportType);
            if(route != null)
                mapArray = await directions.FetchMapAsync(route);

            if(mapArray != null && mapArray.Length > 0 && route != null && !String.IsNullOrEmpty(route.SessionId))
            {
                route.PicPath = fileDAO.SaveImage(mapArray, tourId);
            }
            else
                throw new NullReferenceException();

            return route ?? throw new NullReferenceException();
        }
    }
}
