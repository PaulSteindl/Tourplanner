using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tourplanner.Models;
using Tourplanner.DataAccessLayer;
using Tourplanner.Exceptions;
using Tourplanner.Shared;

namespace Tourplanner.BusinessLayer
{
    public class TourManager : ITourManager
    {
        private readonly ILogger logger = LogingManager.GetLogger<TourManager>();
        IRouteManager routeManager;
        ITourDAO tourDAO;
        ILogDAO logDAO;
        IFileDAO fileDAO;
        ITourLogManager logManager;
        ICheckInput checkInput;
        ICalculateAttributes calcA;

        public TourManager(IRouteManager routeManager, ITourLogManager logManager, ITourDAO tourDAO, ILogDAO logDAO, ICheckInput checkInput, ICalculateAttributes calcA, IFileDAO fileDAO)
        {
            this.routeManager = routeManager;
            this.tourDAO = tourDAO;
            this.logDAO = logDAO;
            this.fileDAO = fileDAO;
            this.logManager = logManager;
            this.checkInput = checkInput;
            this.calcA = calcA;
        }

        public async Task<IEnumerable<Tour>> LoadTours()
        {
            var tours = tourDAO.SelectAllTours().ToList();

            if (tours != null && tours.Count() > 0)
            {
                for(int i = 0; i < tours.Count(); i++)
                {
                    tours[i].Logs = logDAO.SelectLogsByTourId(tours[i].Id);
                    //tours[i] = await DoesMapExistAsync(tours[i]);
                }
            }

            logger.Debug("Tours geladen");
            return tours;
        }

        public async Task<Tour?> DoesMapExistAsync(Tour tour)
        {
            if(tour != null && !File.Exists(tour.PicPath))
            {
                logger.Debug("Download Map again");
                var route = await routeManager.GetFullRoute(tour.From, tour.To, tour.Transporttype, tour.Id);
                if (route != null)
                    tour.PicPath = route.PicPath;
            }
            return tour;
        }

        public async Task<Tour?> NewTour(string name, string description, string from, string to, TransportType transportType)
        {
            Tour? newTour = null;   

            try
            {
                checkInput.CheckUserInputTour(name, description, from, to);

                var newId = Guid.NewGuid();
                var route = await routeManager.GetFullRoute(from, to, transportType, newId);

                if (route != null)
                {
                    newTour = new Tour
                    {
                        Id = newId,
                        Name = name,
                        Description = description,
                        From = from,
                        To = to,
                        Transporttype = transportType,
                        Distance = route.Distance,
                        Time = route.Time,
                        PicPath = route.PicPath,
                        ChildFriendly = false,
                        Popularity = PopularityEnum.Bad,
                        Logs = new List<Log>()
                    };

                    if (!tourDAO.InsertTour(newTour)) throw new DataUpdateFailedException("New Tour couldn't get inserted");

                    logger.Debug($"New Tour created with id: [{newTour.Id}]");
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Couldn't create new Tour, [{ex.Message}]");
            }

            return newTour;
        }

        public async Task<Tour?> UpdateTour(string name, string description, string from, string to, TransportType transportType, Tour tour)
        {
            try
            {
                checkInput.CheckUserInputTour(name, description, from, to);

                var logs = logManager.GetAllLogsByTourId(tour.Id);

                Route? route = null;

                if(from != tour.From || to != tour.To || transportType != tour.Transporttype)
                    route = await routeManager.GetFullRoute(from, to, transportType, tour.Id);

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

                    if (!tourDAO.UpdateTourById(tour)) throw new DataUpdateFailedException("Tour couldn't get updated");

                    logger.Debug($"Tour updated through Route with id: [{tour.Id}]");
                }
                else if(tour.Name != name || tour.Description != description)
                {
                    tour.Name = name;
                    tour.Description = description;

                    if (!tourDAO.UpdateTourById(tour)) throw new DataUpdateFailedException("Tour couldn't get updated");

                    logger.Debug($"Tour updated with id: [{tour.Id}]");
                }

                return tour;
            }
            catch (Exception ex)
            {
                logger.Error($"Tour couldn't update with id: [{tour.Id}], [{ex}]");
            }

            logger.Error($"Tour couldn't update with id: [{tour.Id}], route was null");

            return null;
        }

        public bool DeleteTour(Guid tourId)
        {
            return tourDAO.DeleteTourById(tourId);
        }

        public IEnumerable<Tour> GetAllTours()
        {
            return tourDAO.SelectAllTours();
        }

        public Tour UpdateTourAttributes(Tour tour)
        {
            var logs = logManager.GetAllLogsByTourId(tour.Id);

            tour.ChildFriendly = calcA.CalculateChildFriendly(logs, tour.Distance);
            tour.Popularity = calcA.CalculatePopularity(logs);

            logger.Debug($"Updated tour attributes from tour: [{tour.Id}]");

            return tour;
        }
    }
}
