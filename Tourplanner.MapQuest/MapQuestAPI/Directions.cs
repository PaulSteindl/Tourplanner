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

namespace Tourplanner.BusinessLayer
{
    internal class Directions : MapQuestAPI
    {
        private const string MapQuestDirectionRequest = "http://www.mapquestapi.com/directions/v2/route?key={0}&from={1}&to={2}&outFormat=json&unit=k&locale=de_DE";

        public Directions(string mapQuestKey, HttpClient httpClient) : base(mapQuestKey, httpClient)
        {

        }

        public async Task<Route> GetRouteAsync(string from, string to) 
        {
            var route = await FetchRouteAsync(from, to);

            return route;
        }

        private async Task<Route> FetchRouteAsync(string from, string to)
        {
            try
            {
                var mapQuestRequestUrl = String.Format(MapQuestDirectionRequest, _mapQuestKey, from, to);

                var route = await _httpClient.GetFromJsonAsync<Route>(mapQuestRequestUrl);

                return route;
            }
            catch (Exception ex) when (ex is not FetchDataException)
            {
                throw new FetchDataException("Error while fetching route data: ", ex);
            }
        }

        // @TODO: GET images from mapquest
    }
}
