﻿using System;
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

namespace Tourplanner.BusinessLayer
{
    internal class Directions : MapQuestAPI
    {
        private const string MapQuestDirectionRequest = "http://www.mapquestapi.com/directions/v2/route?key={0}&from={1}&to={2}&outFormat=json&unit=k&locale=de_DE";
        private const string MapQuestMapRequest = "https://www.mapquestapi.com/staticmap/v5/map?key={0}&session={1}&size=640,480&zoom=11&boundingBox={2},{3},{4},{5}";

        public Directions(string mapQuestKey, HttpClient httpClient) : base(mapQuestKey, httpClient)
        {

        }

        public async Task<Route> GetRouteAsync(string from, string to) 
        {
            var route = await FetchRouteAsync(from, to);
            var map = await FetchMapAsync(route);

            return route;
        }

        private async Task<Route> FetchRouteAsync(string from, string to)
        {
            try
            {
                var mapQuestRequestUrl = String.Format(MapQuestDirectionRequest, _mapQuestKey, from, to);

                var route = await _httpClient.GetFromJsonAsync<Route>(mapQuestRequestUrl);

                return route ?? throw new FetchDataException("Route is null");
            }
            catch (Exception ex) when (ex is not FetchDataException)
            {
                throw new FetchDataException("Error while fetching route data: ", ex);
            }
        }

        private async Task<byte[]> FetchMapAsync(Route route)
        {
            try
            {
                var mapQuestRequestUrl = String.Format(MapQuestMapRequest, _mapQuestKey, route.RouteId, route.ul_lng, route.ul_lat, route.lr_lng, route.lr_lat);

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
