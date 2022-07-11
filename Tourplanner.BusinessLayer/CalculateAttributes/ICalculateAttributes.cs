using Tourplanner.Models;

namespace Tourplanner.BusinessLayer
{
    public interface ICalculateAttributes
    {
        public bool CalculateChildFriendly(IEnumerable<Log> logs, double distance);
        public PopularityEnum CalculatePopularity(IEnumerable<Log> logs);
        public int AverageRatingCalc(List<Log> logs);
        public float AverageTimeCalc(List<Log> logs);
        public string CalcTimeFormated(int time);
        public int AverageDifficultyCalc(List<Log> logs);
        public string GetErrorString(IEnumerable<string?> errorMessages);
    }
}
