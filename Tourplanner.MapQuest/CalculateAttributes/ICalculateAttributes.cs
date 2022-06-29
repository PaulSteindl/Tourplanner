using Tourplanner.Models;

namespace Tourplanner.BusinessLayer
{
    public interface ICalculateAttributes
    {
        public bool CalculateChildFriendly(Guid tourId, double distance);
        public PopularityEnum CalculatePopularity(Guid tourId);
    }
}
