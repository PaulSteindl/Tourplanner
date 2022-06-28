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
        Task<bool> InsertTour(Tour newTour);

        /// <summary>
        /// Holt alle Tours
        /// </summary>
        Task<List<Tour>> SelectAllTours();

        /// <summary>
        /// Updated eine Tour über die Id
        /// </summary>
        /// <returns>True wenn eine Zeile geupdated wurde</returns>
        Task<bool> UpdateTourById(Tour updatedTour);

        /// <summary>
        /// Löscht eine Tour über die Id
        /// </summary>
        /// <returns>True wenn eine Zeile gelöscht wurde</returns>
        Task<bool> DeleteTourById(Guid id);
    }
}