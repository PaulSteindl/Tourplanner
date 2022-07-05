using Tourplanner.Models;

namespace Tourplanner.BusinessLayer
{
    public interface ITourManager
    {
        public Task<Tour> newTour(string name, string description, string from, string to, Transport_type transportType);
        public void UpdateTour(string name, string description, string from, string to, Transport_type transportType, Tour tour);
        public void DeleteTour(Guid tourId);
        public List<Tour> GetAllTours();
    }
}
