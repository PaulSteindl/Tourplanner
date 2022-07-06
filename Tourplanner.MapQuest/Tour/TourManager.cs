using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tourplanner.Models;
using Tourplanner.DataAccessLayer;
using Tourplanner.Exceptions;

namespace Tourplanner.BusinessLayer
{
    public class TourManager : ITourManager
    { 
        IRouteManager routeManager;
        ITourDAO tourDAO;
        ILogManager logManager;
        ICheckInput checkInput;
        ICalculateAttributes calcA;

        public TourManager(IRouteManager routeManager, ILogManager logManager, ITourDAO tourDAO, ICheckInput checkInput, ICalculateAttributes calcA)
        {
            this.routeManager = routeManager;
            this.tourDAO = tourDAO;
            this.logManager = logManager;
            this.checkInput = checkInput;
            this.calcA = calcA;
        }

        public async Task<Tour> newTour(string name, string description, string from, string to, TransportType transportType)
        {
            Tour newTour = new Tour();

            checkInput.CheckUserInputTour(name, description, from, to, transportType);

            try
            {
                var route = await routeManager.GetFullRoute(from, to, transportType);

                if (route != null)
                {
                    newTour.Id = Guid.NewGuid();
                    newTour.Name = name;
                    newTour.Description = description;
                    newTour.From = from;
                    newTour.To = to;
                    newTour.Transporttype = transportType;
                    newTour.Distance = route.Distance;
                    newTour.Time = route.Time;
                    newTour.PicPath = route.PicPath;
                    newTour.ChildFriendly = false;
                    newTour.Popularity = PopularityEnum.Bad;
                    newTour.Logs = new List<Log>();

                    if (!tourDAO.InsertTour(newTour)) throw new DataUpdateFailedException("New Tour couldn't get inserted");
                }
            }
            catch (Exception e)
            {
                throw new NullReferenceException("An error happend while creating a tour -> tour is null: " + e.Message);
            }

            return newTour;
        }

        public async Task<Tour> UpdateTour(string name, string description, string from, string to, TransportType transportType, Tour tour)
        {
            checkInput.CheckUserInputTour(name, description, from, to, transportType);

            try
            {
                var logs = logManager.GetAllLogsByTourId(tour.Id);

                var route = await routeManager.GetFullRoute(from, to, transportType);

                if (route != null)
                {
                    tour.Name = name;
                    tour.Description = description;
                    tour.From = from;
                    tour.To = to;
                    tour.Transporttype = transportType;
                    tour.Distance = route.Distance;
                    tour.Time = route.Time;
                    tour.PicPath = route.PicPath;
                    tour.ChildFriendly = calcA.CalculateChildFriendly(logs, tour.Distance);
                    tour.Popularity = calcA.CalculatePopularity(logs);

                    if(!tourDAO.UpdateTourById(tour)) throw new DataUpdateFailedException("Tour couldn't get updated");
                }
                return tour;
            }
            catch (Exception e) when (e is not DataUpdateFailedException)
            {
                throw new ArgumentException("An error happend while updating a tour: " + e.Message);
            }
        }

        public void DeleteTour(Guid tourId)
        {
            tourDAO.DeleteTourById(tourId);
        }

        public List<Tour> GetAllTours()
        {
            return tourDAO.SelectAllTours();
        }

        public Tour UpdateTourAttributes(Tour tour)
        {
            var logs = logManager.GetAllLogsByTourId(tour.Id);

            tour.ChildFriendly = calcA.CalculateChildFriendly(logs, tour.Distance);
            tour.Popularity = calcA.CalculatePopularity(logs);

            return tour;
        }
    }
}
