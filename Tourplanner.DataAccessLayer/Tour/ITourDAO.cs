using System;
using Tourplanner.Models;

namespace Tourplanner.DataAccessLayer
{
    public interface ITourDAO
    {
        /// <summary>
        /// Fügt eine neue Tour hinzu
        /// </summary>
        /// <returns>True wenn eine Zeile inserted wurde</returns>
        bool InsertTour(Tour newTour);

        /// <summary>
        /// Holt eine Tour per Id
        /// </summary>
        public Tour SelectTourById(Guid tourId);

        /// <summary>
        /// Holt alle Tours
        /// </summary>
        List<Tour> SelectAllTours();

        /// <summary>
        /// Updated eine Tour über die Id
        /// </summary>
        /// <returns>True wenn eine Zeile geupdated wurde</returns>
        bool UpdateTourById(Tour updatedTour);

        /// <summary>
        /// Löscht eine Tour über die Id
        /// </summary>
        /// <returns>True wenn eine Zeile gelöscht wurde</returns>
        bool DeleteTourById(Guid id);

    }
}