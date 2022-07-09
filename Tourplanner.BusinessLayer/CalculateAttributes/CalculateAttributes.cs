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

        public bool CalculateChildFriendly(IEnumerable<Log> logs, double distance)
        {
            float sum = 0;

            if(sum > 3 && distance < 8 && logs.Count() > 9)
                return true;
            else if(sum > 2 && distance < 5 && logs.Count() > 9)
                return true;
             
            return false;
        }

        public PopularityEnum CalculatePopularity(IEnumerable<Log> logs)
        {
            if (logs.Count() > 100)
                return PopularityEnum.Perfect;
            else if (logs.Count() > 70)
                return PopularityEnum.Excellent;
            else if (logs.Count() > 40)
                return PopularityEnum.Good;
            else if (logs.Count() > 10)
                return PopularityEnum.Okay;

            return PopularityEnum.Bad;
        }

        public float AverageRatingCalc(List<Log> logs)
        {
            float sum = 0;
            int i = 0;

            for (; i < logs.Count; i++)
            {
                sum += Convert.ToInt32(logs[i].Rating);
            }

            sum /= logs.Count;

            return sum;
        }

        public float AverageTimeCalc(List<Log> logs)
        {
            float sum = 0;
            int i = 0;

            for (; i < logs.Count; i++)
            {
                sum += logs[i].TotalTime;
            }

            sum /= logs.Count;

            return sum;
        }

        public float AverageDifficultyCalc(List<Log> logs)
        {
            float sum = 0;
            int i = 0;

            for (; i < logs.Count; i++)
            {
                sum += Convert.ToInt32(logs[i].Difficulty);
            }

            sum /= logs.Count;

            return sum;
        }

        public string CalcTimeFormated(int time)
        {
            TimeSpan t = TimeSpan.FromSeconds(time);
            string answer = "00h:00m:00s";

            //sec in einem Tag
            if (time > 86399)
            {
                answer = string.Format("{0:D1}d:{1:D2}h:{2:D2}m:{3:D2}s",
                                t.Days,
                                t.Hours,
                                t.Minutes,
                                t.Seconds);
            }
            else
            {
                answer = string.Format("{0:D2}h:{1:D2}m:{2:D2}s",
                                t.Hours,
                                t.Minutes,
                                t.Seconds);
            }

            return answer;
        }

        public string GetErrorString(IEnumerable<string?> errorMessages)
        {
            string errorMessage = String.Empty;

            if (errorMessages != null && errorMessages.Count() > 0)
            {
                foreach(string error in errorMessages)
                {
                    errorMessage += error + '\n';
                }
            }

            return string.Empty;
        }
    }
}
