using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourplanner.Models;
using System.Drawing;
using Npgsql;
using System.Data;
using Tourplanner.Exceptions;
using System.Globalization;
using Tourplanner.Shared;
using Tourplanner.DataAccessLayer;

namespace Tourplanner.DataAccessLayerInMemory
{
    public class TourInMemoryDAO : ITourDAO
    {
        private List<Tour> tourList;

        public TourInMemoryDAO()
        {
            this.tourList = new List<Tour>();
        }

        public bool InsertTour(Tour newTour)
        {
            bool inserted = false;

            if (SelectTourById(newTour.Id) == null)
            {
                tourList.Add(newTour);
                inserted = true;
            }

            return inserted;
        }

        public Tour? SelectTourById(Guid id)
        {
            return tourList.SingleOrDefault(t => t.Id == id);
        }

        public IEnumerable<Tour> SelectAllTours()
        {
            return tourList;
        }

        public bool UpdateTourById(Tour updatedTour)
        {
            bool inserted = false;
            Tour? oldLog;

            if ((oldLog = SelectTourById(updatedTour.Id)) != null)
            {
                tourList.Remove(oldLog);
                tourList.Add(updatedTour);
                inserted = true;
            }

            return inserted;
        }

        public bool DeleteTourById(Guid id)
        {
            bool inserted = false;
            Tour? tour;

            if ((tour = SelectTourById(id)) != null)
            {
                tourList.Remove(tour);
                inserted = true;
            }

            return inserted;
        }
    }
}

