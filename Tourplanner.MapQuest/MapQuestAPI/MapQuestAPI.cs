using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;

namespace Tourplanner.BusinessLayer
{
    internal abstract class MapQuestAPI
    {
        internal const string ImagePath = @".\TourPlanner_CachedImages\";
        internal const string MapImagePath = ImagePath + @"Maps\";
        internal const string IconImagePath = ImagePath + @"Icons\";

        protected readonly string _mapQuestKey;
        protected readonly HttpClient _httpClient;

        internal MapQuestAPI(string mapQuestKey, HttpClient httpClient)
        {
            _mapQuestKey = mapQuestKey;
            _httpClient = httpClient;
        }
    }
}