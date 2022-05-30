using Tourplanner.Models;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace Tourplanner.DAL
{
    public interface ITourRepository
    {
        Task<Route> GetRoute(string from, string to);

        /// <summary>
        /// Fügt eine neue Tour hinzu
        /// </summary>
        /// <returns>True wenn eine Zeile inserted wurde</returns>
        bool InsertTour(Tour newTour);

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
