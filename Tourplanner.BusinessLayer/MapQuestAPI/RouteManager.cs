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

        public async Task<RouteInfo?> GetFullRoute(string from, string to, TransportType transportType, Guid tourId)
        {
            byte[]? mapArray = null;
            var routeInfo = await directions.FetchRouteAsync(from, to, transportType);
            if(routeInfo.Route != null && !String.IsNullOrEmpty(routeInfo.Route.SessionId) && routeInfo.Route.Time != 0)
                mapArray = await directions.FetchMapAsync(routeInfo.Route);

            if(mapArray != null && routeInfo.Route != null && !String.IsNullOrEmpty(routeInfo.Route.SessionId))
            {
                //fileDAO.DeleteImage(tourId);
                routeInfo.Route.PicPath = fileDAO.SaveImage(mapArray, tourId);
            }

            return routeInfo;
        }
    }
}
