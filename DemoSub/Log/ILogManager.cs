﻿using System;
using System.Collections.Generic;
using Tourplanner.Models;

namespace Tourplanner.LogManager
{
    internal interface ILogManager
    {
        public string CreateLog(Guid tourguid, string comment, DifficultyEnum difficulty, int totalTime, PopularityEnum rating);
        public string UpdateLog(Guid id, string comment, DateTime date, DifficultyEnum difficulty, int totalTime, PopularityEnum rating);
        public List<Log>? GetLogsFromTour(Guid tourId);
    }
}