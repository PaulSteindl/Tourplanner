﻿using Tourplanner.Models;
using System.Collections.Generic;
using System;

namespace Tourplanner.DataAccessLayer
{
    public interface ILogDAO
    {
        /// <summary>
        /// Fügt einen neuen Log hinzu
        /// </summary>
        /// <returns>True wenn eine Zeile inserted wurde</returns>
        bool InsertLog(Log newLog);

        /// <summary>
        /// Holt alle Logs von einer Tour
        /// </summary>
        IEnumerable<Log> SelectLogsByTourId(Guid id);

        /// <summary>
        /// Updated einen Log über die Id
        /// </summary>
        /// <returns>True wenn eine Zeile geupdated wurde</returns>
        bool UpdateLogById(Log updatedLog);

        /// <summary>
        /// Löscht einen Log über die Id
        /// </summary>
        /// <returns>True wenn eine Zeile gelöscht wurde</returns>
        bool DeleteLogById(Guid id);

    }
}
