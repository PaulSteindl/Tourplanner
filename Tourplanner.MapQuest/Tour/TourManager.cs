using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tourplanner.Models;
using Tourplanner.DataAccessLayer;

namespace Tourplanner.BusinessLayer
{
    public class TourManager : ITourManager
    { 
        IRouteManager routeManager;
        ITourDAO tourDAO;

        //TODO Update Tour? Vlt doch nicht RouteId benutzen

        public async Task<Tour> newTour(string name, string description, string from, string to, TransportType transportType)
        {
            Tour newTour = new Tour();

            if (name == null || !CheckUserInput(name)) throw new ArgumentException("Name is invalid");
            if (name == null || !CheckUserInputWithSymbols(description)) throw new ArgumentException("Description is invalid");
            if (name == null || !CheckUserInput(from)) throw new ArgumentException("Starting location is invalid");
            if (name == null || !CheckUserInput(to)) throw new ArgumentException("Ending location is invalid");

            try
            {
                var route = await routeManager.GetFullRoute(from, to, transportType);

                if (route != null)
                {
                    newTour.Id = route.RouteId;
                    newTour.Name = name;
                    newTour.Description = description;
                    newTour.From = from;
                    newTour.To = to;
                    newTour.Transporttype = transportType;
                    newTour.Distance = route.TotalDistance.Value;
                    newTour.Time = route.TotalTime;
                    newTour.PicPath = route.picPath;
                    newTour.ChildFriendly = CalculateChildFriendly();
                    newTour.Popularity = PopularityEnum.Bad;
                    newTour.Logs = new List<Log>();

                    tourDAO.InsertTour(newTour);
                }
            }
            catch (Exception e)
            {
                throw new NullReferenceException("An error happend while creating a tour -> tour is null: " + e.Message);
            }

            return newTour;
        }

        public void DeleteTour(string tourId)
        {
            tourDAO.DeleteTour(tourId);
        }

        public bool CheckUserInput(string input)
        {
            if(Regex.Match(input, "^[a-zA-Z0-9-,]*$").Success)
                return true;
            return false;
        }

        public bool CheckUserInputWithSymbols(string input)
        {
            if (Regex.Match(input, "^[a-zA-Z0-9,.!€$?-]*$").Success)
                return true;
            return false;
        }

        public bool CalculateChildFriendly()
        {
            throw new NotImplementedException();
        }

        public PopularityEnum CalculatePopularity()
        {
            throw new NotImplementedException();
        }
    }
}
