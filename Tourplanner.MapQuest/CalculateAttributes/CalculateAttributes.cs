using Tourplanner.Models;
using Tourplanner.DataAccessLayer;

namespace Tourplanner.BusinessLayer
{
    public class CalculateAttributes : ICalculateAttributes
    {
        ILogDAO logDAO;

        public CalculateAttributes(ILogDAO logDAO)
        {
            this.logDAO = logDAO;
        }

        public bool CalculateChildFriendly(Guid tourId, double distance)
        {
            float sum = 0;
            int i = 0;
            var logs = logDAO.SelectLogsByTourId(tourId);

            for(; i < logs.Count; i++)
            {
                sum += Convert.ToInt32(logs[i].Difficulty);
            }

            sum /= logs.Count;

            if(sum > 3 && distance < 8 && logs.Count > 9)
                return true;
            else if(sum > 2 && distance < 5 && logs.Count > 9)
                return true;
             
            return false;
        }

        public PopularityEnum CalculatePopularity(Guid tourId)
        {
            var logs = logDAO.SelectLogsByTourId(tourId);

            if (logs.Count > 100)
                return PopularityEnum.Perfect;
            else if (logs.Count > 70)
                return PopularityEnum.Excellent;
            else if (logs.Count > 40)
                return PopularityEnum.Good;
            else if (logs.Count > 10)
                return PopularityEnum.Okay;

            return PopularityEnum.Bad;
        }
    }
}
