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

namespace Tourplanner.BusinessLayer
{
    public class Directions : MapQuestAPI, IDirections
    {
        private const string MapQuestDirectionRequest = "http://www.mapquestapi.com/directions/v2/route?key={0}&from={1}&to={2}&routeType={3}&outFormat=json&unit=k&locale=de_DE";
        private const string MapQuestMapRequest = "https://www.mapquestapi.com/staticmap/v5/map?key={0}&session={1}&size=640,480&zoom=11&boundingBox={2},{3},{4},{5}";

        public Directions(string mapQuestKey, HttpClient httpClient) : base(mapQuestKey, httpClient)
        {

        }

        public async Task<Route> FetchRouteAsync(string from, string to, Models.TransportType transportType)
        {
            try
            {
                var mapQuestRequestUrl = String.Format(MapQuestDirectionRequest, _mapQuestKey, from, to, transportType);

                var routeInfo = await _httpClient.GetFromJsonAsync<RouteInfo>(mapQuestRequestUrl);

                if(routeInfo != null)
                    return routeInfo.Route ?? throw new FetchDataException("Route is null");
                else
                    throw new FetchDataException("RouteInfo is null");
            }
            catch (Exception ex) when (ex is not FetchDataException)
            {
                throw new FetchDataException("Error while fetching route data: ", ex);
            }
        }

        public async Task<byte[]> FetchMapAsync(Route route)
        {
            try
            {
                var mapQuestRequestUrl = String.Format(MapQuestMapRequest, _mapQuestKey, route.SessionId, route.BoundingBox.Ul.Lng, route.BoundingBox.Ul.Lat, route.BoundingBox.Lr.Lng, route.BoundingBox.Lr.Lat);

                var mapArray = await _httpClient.GetByteArrayAsync(mapQuestRequestUrl);

                return mapArray ?? throw new FetchDataException("Route is null");
            }
            catch (Exception ex) when (ex is not FetchDataException)
            {
                throw new FetchDataException("Error while fetching map data: ", ex);
            }
        }
    }
}
