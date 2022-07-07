using System;
using System.Net;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Tourplanner.Exceptions;
using System.Net.Http.Json;
using Tourplanner.Models;
using Tourplanner.DataAccessLayer;
using System.Globalization;
using Tourplanner.Shared;

namespace Tourplanner.BusinessLayer
{
    public class Directions : IDirections
    {

        IMapQuestConfiguration configuration;
        private readonly HttpClient httpClient;
        private readonly ILogger logger = Shared.LogManager.GetLogger<Directions>();

        public Directions(IMapQuestConfiguration mapQuest)
        {
            httpClient = new HttpClient();
            this.configuration = mapQuest;
        }

        public async Task<Route> FetchRouteAsync(string from, string to, Models.TransportType transportType)
        {
            try
            {
                var mapQuestRequestUrl = String.Format(configuration.DirectionUrl, configuration.MapQuestKey, from, to, transportType);

                var routeInfo = await httpClient.GetFromJsonAsync<RouteInfo>(mapQuestRequestUrl);

                logger.Debug($"Fetched Routeinfo with session id{routeInfo.Route.SessionId}");

                return routeInfo.Route ?? throw new FetchDataException("Route is null");
            }
            catch (Exception ex) when (ex is not FetchDataException)
            {
                logger.Warn($"Couldn't fetch Routeinfo, [{ex}]");
                throw new FetchDataException("Error while fetching route data: ", ex);
            }
        }

        public async Task<byte[]> FetchMapAsync(Route route)
        {
            try
            {
                var mapQuestRequestUrl = String.Format(configuration.MapUrl, configuration.MapQuestKey, route.SessionId, 
                    route.BoundingBox.Ul.Lat.ToString(CultureInfo.CreateSpecificCulture("en-GB")), 
                    route.BoundingBox.Ul.Lng.ToString(CultureInfo.CreateSpecificCulture("en-GB")), 
                    route.BoundingBox.Lr.Lat.ToString(CultureInfo.CreateSpecificCulture("en-GB")), 
                    route.BoundingBox.Lr.Lng.ToString(CultureInfo.CreateSpecificCulture("en-GB"))
                );

                var mapArray = await httpClient.GetByteArrayAsync(mapQuestRequestUrl);

                logger.Debug($"Fetched Map for Route with session id{route.SessionId}");

                return mapArray ?? throw new FetchDataException("Route is null");
            }
            catch (Exception ex) when (ex is not FetchDataException)
            {
                logger.Warn($"Couldn't fetch Map, [{ex}]");
                throw new FetchDataException("Error while fetching map data: ", ex);
            }
        }
    }
}
