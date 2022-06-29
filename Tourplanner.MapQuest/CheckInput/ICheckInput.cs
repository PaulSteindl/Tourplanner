using Tourplanner.Models;

namespace Tourplanner.BusinessLayer
{
    public interface ICheckInput
    {
        public bool CheckUserInputTour(string name, string description, string from, string to, TransportType transportType);
        public bool CheckUserInputLog(string comment);
    }
}
