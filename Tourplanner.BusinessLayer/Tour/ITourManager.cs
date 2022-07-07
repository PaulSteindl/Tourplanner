using Tourplanner.Models;

namespace Tourplanner.BusinessLayer
{
    public interface ITourManager
    {
        public List<Tour> LoadTours();
        public Task<Tour?> NewTour(string name, string description, string from, string to, TransportType transportType);
        public Task<Tour?> UpdateTour(string name, string description, string from, string to, TransportType transportType, Tour tour);
        public bool DeleteTour(Guid tourId);
        public List<Tour> GetAllTours();
        public Tour UpdateTourAttributes(Tour tour);
        public Task<Tour?> DoesMapExistAsync(string path, Tour tour)
    }
}
