﻿using Tourplanner.Models;

namespace Tourplanner.BusinessLayer
{
    public interface ICalculateAttributes
    {
        public bool CalculateChildFriendly(IEnumerable<Log> logs, double distance);
        public PopularityEnum CalculatePopularity(IEnumerable<Log> logs);
        public float AverageRatingCalc(List<Log> logs);
        public float AverageTimeCalc(List<Log> logs);
        public string CalcTimeFormated(int time);
        public float AverageDifficultyCalc(List<Log> logs);
    }
}
