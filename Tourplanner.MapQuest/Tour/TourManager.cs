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
        ICheckInput checkInput;
        ICalculateAttributes calcA;

        public TourManager(IRouteManager routeManager, ITourDAO tourDAO, ICheckInput checkInput, ICalculateAttributes calcA)
        {
            this.routeManager = routeManager;
            this.tourDAO = tourDAO;
            this.checkInput = checkInput;
            this.calcA = calcA;
        }

        public async Task<Tour> newTour(string name, string description, string from, string to, Transport_type transportType)
        {
            Tour newTour = new Tour();

            checkInput.CheckUserInputTour(name, description, from, to, transportType);

            try
            {
                var route = await routeManager.GetFullRoute(from, to, transportType);

                if (route != null)
                {
                    newTour.Id = new Guid();
                    newTour.Name = name;
                    newTour.Description = description;
                    newTour.From = from;
                    newTour.To = to;
                    newTour.Transporttype = transportType;
                    newTour.Distance = Convert.ToInt32(route.Distance);
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

        public async void UpdateTour(string name, string description, string from, string to, Transport_type transportType, Tour tour)
        {
            checkInput.CheckUserInputTour(name, description, from, to, transportType);

            try
            {
                var route = await routeManager.GetFullRoute(from, to, transportType);

                if (route != null)
                {
                    tour.Name = name;
                    tour.Description = description;
                    tour.From = from;
                    tour.To = to;
                    tour.Transporttype = transportType;
                    tour.Distance = Convert.ToInt32(route.Distance);
                    tour.Time = route.Time;
                    tour.PicPath = route.PicPath;
                    tour.ChildFriendly = calcA.CalculateChildFriendly(tour.Id, tour.Distance);
                    tour.Popularity = calcA.CalculatePopularity(tour.Id);

                    if(!tourDAO.UpdateTourById(tour)) throw new DataUpdateFailedException("Tour couldn't get updated");
                }
            }
            catch (Exception e)
            {
                throw new NullReferenceException("An error happend while creating a tour -> tour is null: " + e.Message);
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
    }
}
