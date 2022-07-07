using Tourplanner.Models;

namespace Tourplanner.BusinessLayer
{
    public interface ICalculateAttributes
    {
        public bool CalculateChildFriendly(List<Log> logs, double distance);
        public PopularityEnum CalculatePopularity(List<Log> logs);
        public float AverageRatingCalc(List<Log> logs);
        public float AverageTimeCalc(List<Log> logs);
        public string CalcTimeFormated(int time);
    }
}
