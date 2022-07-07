﻿using Tourplanner.Models;

namespace Tourplanner.BusinessLayer
{
    public interface ITourManager
    {
        public Task<Tour> NewTour(string name, string description, string from, string to, TransportType transportType);
        public Task<Tour> UpdateTour(string name, string description, string from, string to, TransportType transportType, Tour tour);
        public void DeleteTour(Guid tourId);
        public List<Tour> GetAllTours();
        public Tour UpdateTourAttributes(Tour tour);
    }
}
