using System;
using Tourplanner.Models;

namespace Tourplanner.DataAccessLayer
{
    public interface ITourDAO
    {
        Task<Tour> AddTour(Tour tour);
        Task<Tour> ModifyTour(Tour tour);
        Task<Tour> DeleteTour(Tour tour);
        Task<string> SaveImage(byte[] mapArray, , string routeId);
    }
}