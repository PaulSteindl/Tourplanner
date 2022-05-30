using System;
using System.IO;
using System.Net.Http;

namespace Tourplanner.BusinessLayer
{
    internal abstract class MapQuestAPI
    {
        protected readonly string _mapQuestKey;
        protected readonly HttpClient _httpClient;

        internal MapQuestAPI(string mapQuestKey, HttpClient httpClient)
        {
            _mapQuestKey = mapQuestKey;
            _httpClient = httpClient;
        }
    }
}