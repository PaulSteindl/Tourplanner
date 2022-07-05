using Tourplanner.Models;

namespace Tourplanner.BusinessLayer
{
    public interface ICheckInput
    {
        public bool CheckUserInputTour(string name, string description, string from, string to, Transport_type transportType);
        public bool CheckUserInputLog(string comment);
    }
}
