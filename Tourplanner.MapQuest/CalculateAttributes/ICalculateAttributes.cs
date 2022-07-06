using Tourplanner.Models;

namespace Tourplanner.BusinessLayer
{
    public interface ICalculateAttributes
    {
        public bool CalculateChildFriendly(List<Log> logs, double distance);
        public PopularityEnum CalculatePopularity(List<Log> logs);
    }
}
