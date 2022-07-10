using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourplanner.DataAccessLayer;
using Tourplanner.Models;
using Tourplanner.Shared;

namespace Tourplanner.BusinessLayer
{
    public class TourLogManager : ITourLogManager
    {
        ILogDAO logDAO;
        ICheckInput checkInput;
        private readonly ILogger logger = LogingManager.GetLogger<TourLogManager>();

        public TourLogManager(ILogDAO logDAO, ICheckInput checkInput)
        {
            this.logDAO = logDAO;
            this.checkInput = checkInput;
        }

        public async Task<Log?> CreateLog(string comment, int time, DateTime date, DifficultyEnum difficulty, PopularityEnum rating, Guid tourId)
        {
            Log newLog = null;

            try
            {
                checkInput.CheckUserInputLog(comment);

                newLog = new Log
                {
                    Id = Guid.NewGuid(),
                    TourId = tourId,
                    Date = date,
                    Comment = comment,
                    Difficulty = difficulty,
                    TotalTime = time,
                    Rating = rating,
                }; 

                if (!logDAO.InsertLog(newLog)) throw new DataUpdateFailedException("New Log couldn't get inserted");

                logger.Debug($"New Log created with id: [{newLog.Id}]");
                return newLog;
            }
            catch (Exception ex)
            {
                logger.Error($"Couldn't create new Log, [{ex.Message}]");
            }

            return null;
        }

        public async Task<Log?> UpdateLog(string comment, int time, DateTime date, DifficultyEnum difficulty, PopularityEnum rating, Log log)
        {
            try
            {
                checkInput.CheckUserInputLog(comment);

                log.Date = date;
                log.Comment = comment;
                log.Difficulty = difficulty;
                log.TotalTime = time;
                log.Rating = rating;

                if (!logDAO.UpdateLogById(log)) throw new DataUpdateFailedException("Log couldn't get inserted");

                logger.Debug($"Log updated with id: [{log.Id}]");
                return log;
            }
            catch (Exception ex)
            {
                logger.Error($"Tour couldn't update with id: [{log.Id}], [{ex.Message}]");
            }

            logger.Error($"Tour couldn't update with id: [{log.Id}]");
            return null;
        }

        public void DeleteLog(Guid logId)
        {
            logDAO.DeleteLogById(logId);
        }

        public IEnumerable<Log> GetAllLogsByTourId(Guid tourId)
        {
            return logDAO.SelectLogsByTourId(tourId);
        }
    }
}
