﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourplanner.DataAccessLayer;
using Tourplanner.Models;
using Tourplanner.Exceptions;

namespace Tourplanner.BusinessLayer
{
    public class LogManager : ILogManager
    {
        ILogDAO logDAO;
        ICheckInput checkInput;
        ITourManager tourManager;

        public LogManager(ILogDAO logDAO, ICheckInput checkInput, ITourManager tourManager)
        {
            this.logDAO = logDAO;
            this.checkInput = checkInput;
            this.tourManager = tourManager;
        }

        public Log CreateLog(string comment, int time, DateTime date, DifficultyEnum difficulty, PopularityEnum rating, Guid tourId)
        {
            checkInput.CheckUserInputLog(comment);
            Log newLog = new Log();

            try
            {
                newLog.Id = new Guid();
                newLog.Id = tourId;
                newLog.Date = date;
                newLog.Comment = comment;
                newLog.Difficulty = difficulty;
                newLog.TotalTime = time;
                newLog.Rating = rating;
                    

                if (!logDAO.InsertLog(newLog)) throw new DataUpdateFailedException("New Log couldn't get inserted");

            }
            catch (Exception e)
            {
                throw new NullReferenceException("An error happend while creating a log -> log is null: " + e.Message);
            }

            return newLog;
        }

        public void UpdateLog(string comment, int time, DateTime date, DifficultyEnum difficulty, PopularityEnum rating, Log log)
        {
            checkInput.CheckUserInputLog(comment);
            Log newLog = new Log();

            try
            {
                log.Date = date;
                log.Comment = comment;
                log.Difficulty = difficulty;
                log.TotalTime = time;
                log.Rating = rating;

                if (!logDAO.UpdateLogById(log)) throw new DataUpdateFailedException("Log couldn't get inserted");
            }
            catch (Exception e)
            {
                throw new NullReferenceException("An error happend while updating a log: " + e.Message);
            }
        }

        public void DeleteLog(Guid logId)
        {
            logDAO.DeleteLogById(logId);
        }

        public List<Log> GetAllLogsByTourId(Guid tourId)
        {
            return logDAO.SelectLogsByTourId(tourId);
        }
    }
}
