using Tourplanner.Models;
using System;
using System.Data;
using System.Collections.Generic;
using System.Threading;
using Tourplanner.Shared;
using Tourplanner.DataAccessLayer;

namespace Tourplanner.DataAccessLayerInMemory
{
    public class LogInMemoryDAO : ILogDAO
    {
        private List<Log> logList;

        public LogInMemoryDAO()
        {
            logList = new List<Log>();
        }

        public bool InsertLog(Log newLog)
        {
            bool inserted = false;

            if(GetLogById(newLog.Id) == null)
            {
                logList.Add(newLog);
                inserted = true;
            }

            return inserted;
        }

        public Log? GetLogById(Guid id)
        {
            return logList.SingleOrDefault(l => l.Id == id);
        }

        public IEnumerable<Log> SelectLogsByTourId(Guid tourId)
        {
            return logList.Where(l => l.TourId == tourId);
        }

        public bool UpdateLogById(Log updatedLog)
        {
            bool inserted = false;
            Log? oldLog;

            if ((oldLog = GetLogById(updatedLog.Id)) != null)
            {
                logList.Remove(oldLog);
                logList.Add(updatedLog);
                inserted = true;
            }

            return inserted;
        }

        public bool DeleteLogById(Guid id)
        {
            bool inserted = false;
            Log? log;

            if ((log = GetLogById(id)) != null)
            {
                logList.Remove(log);
                inserted = true;
            }

            return inserted;
        }
    }
}
