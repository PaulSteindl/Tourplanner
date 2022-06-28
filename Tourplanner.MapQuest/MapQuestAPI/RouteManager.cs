﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourplanner.DataAccessLayer;
using Tourplanner.Models;

namespace Tourplanner.BusinessLayer
{
    internal class RouteManager
    {
        private readonly IFileDAO fileDAO;
        private readonly IDirections directions;

        public RouteManager(IFileDAO fileDAO, IDirections directions)
        {
            this.fileDAO = fileDAO;
            this.directions = directions;
        }

        public async Task<Route> GetFullRoute(string from, string to, TransportType transportType)
        {

            var route = await directions.FetchRouteAsync(from, to, transportType);
            var mapArray = await directions.FetchMapAsync(route);

            if(mapArray != null && mapArray.Length > 0 && route != null && !String.IsNullOrEmpty(route.RouteId))
            {
                string fullPath = $"{Path.GetDirectoryName(Path.GetFullPath($".\\{route.RouteId}.jpeg"))}";
                if (!Directory.Exists(fullPath))
                {
                    route.picPath = fileDAO.SaveImage(mapArray, route.RouteId);
                }
            }

            return route ?? throw new NullReferenceException();
        }
    }
}
