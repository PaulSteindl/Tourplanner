using System;
using System.Collections.Generic;
using Tourplanner.Models;

namespace Tourplanner
{
    internal interface ILogManager
    {
        public string CreateLog(Guid tourguid, string comment, DifficultyEnum difficulty, int totalTime, PopularityEnum rating);
        public string UpdateLog(Guid id, string comment, DateTime date, DifficultyEnum difficulty, int totalTime, PopularityEnum rating);
        public IEnumerable<Log> GetLogsFromTour(Guid tourId);
    }
}
