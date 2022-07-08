using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourplanner.DataAccessLayer;
using Tourplanner.Models;
using Tourplanner.Exceptions;
using Tourplanner.Shared;

namespace Tourplanner.BusinessLayer
{
    public class LogManager : ILogManager
    {
        ILogDAO logDAO;
        ICheckInput checkInput;
        private readonly ILogger logger = Shared.LogManager.GetLogger<LogManager>();

        public LogManager(ILogDAO logDAO, ICheckInput checkInput)
        {
            this.logDAO = logDAO;
            this.checkInput = checkInput;
        }

        public Log? CreateLog(string comment, int time, DateTime date, DifficultyEnum difficulty, PopularityEnum rating, Guid tourId)
        {
            checkInput.CheckUserInputLog(comment);
            Log newLog = null;

            try
            {
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
            }
            catch (Exception ex)
            {
                logger.Error($"Couldn't create new Log, [{ex.Message}]");
                throw new NullReferenceException("An error happend while creating a log -> log is null: " + ex.Message);
            }

            return newLog;
        }

        public async Task<Log?> UpdateLog(string comment, int time, DateTime date, DifficultyEnum difficulty, PopularityEnum rating, Log log)
        {
            checkInput.CheckUserInputLog(comment);

            try
            {
                log.Date = date;
                log.Comment = comment;
                log.Difficulty = difficulty;
                log.TotalTime = time;
                log.Rating = rating;

                if (!logDAO.UpdateLogById(log)) throw new DataUpdateFailedException("Log couldn't get inserted");

                logger.Debug($"Log updated with id: [{log.Id}]");
            }
            catch (Exception ex)
            {
                logger.Error($"Tour couldn't update with id: [{log.Id}], [{ex.Message}]");
                throw new NullReferenceException("An error happend while updating a log: " + ex.Message);
            }

            return log;
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
