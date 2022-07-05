using Npgsql;
using Tourplanner.Models;
using System;
using System.Data;
using System.Collections.Generic;
using System.Threading;

namespace Tourplanner.DataAccessLayer
{
    public class LogDAO : ILogDAO
    {        
        private const string InsertLogCommand = "INSERT INTO logs(date, comment, difficulty, time, rating) VALUES (@date, @comment, @difficulty, @time, @rating)";
        private const string SelectLogsByTourIdCommand = "SELECT * FROM logs WHERE tour_id = @tour_id";
        private const string UpdateLogByIdCommand = "UPDATE logs SET date = @date, comment = @comment, difficulty = @difficulty, totaltime = @totaltime, rating = @rating WHERE log_id = @id";
        private const string DeleteLogByIdCommand = "DELETE FROM logs WHERE log_id = @id";

        private IDatabaseManager databaseManager;

        public LogDAO(IDatabaseManager databaseManager)
        {
            this.databaseManager = databaseManager;
        }

        public bool InsertLog(Log newLog)
        {
            var affectedRows = 0;

            try
            {
                affectedRows = databaseManager.ExecuteWithConnection(connection =>
                {
                    using var cmd = new NpgsqlCommand(InsertLogCommand, connection);
                    cmd.Parameters.AddWithValue("date", newLog.Date);
                    if (!String.IsNullOrEmpty(newLog.Comment))
                        cmd.Parameters.AddWithValue("comment", newLog.Comment);
                    cmd.Parameters.AddWithValue("difficulty", newLog.Difficulty);
                    cmd.Parameters.AddWithValue("totaltime", newLog.TotalTime);
                    cmd.Parameters.AddWithValue("rating", newLog.Rating);

                    return cmd.ExecuteNonQuery();
                });
            }
            catch (PostgresException)
            {
            }

            return affectedRows > 0;
        }

        public List<Log> SelectLogsByTourId(Guid tourId)
        {


            try
            {
                return databaseManager.ExecuteWithConnection(connection =>
                {
                    var logs = new List<Log>();

                    using var cmd = new NpgsqlCommand(SelectLogsByTourIdCommand, connection);
                    cmd.Parameters.AddWithValue("tour_id", tourId);
                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var log = ReadLogsDetails(reader);
                        logs.Add(log);
                    }

                    return logs;
                });
            }
            catch (PostgresException)
            {
            }

            return new List<Log>();
        }

        public bool UpdateLogById(Log updatedLog)
        {
            var affectedRows = 0;

            try
            {
                affectedRows = databaseManager.ExecuteWithConnection(connection =>
                {
                    using var cmd = new NpgsqlCommand(UpdateLogByIdCommand, connection);
                    cmd.Parameters.AddWithValue("id", updatedLog.Id);
                    cmd.Parameters.AddWithValue("name", updatedLog.Date);
                    if (!String.IsNullOrEmpty(updatedLog.Comment))
                        cmd.Parameters.AddWithValue("description", updatedLog.Comment);
                    cmd.Parameters.AddWithValue("from", updatedLog.Difficulty);
                    cmd.Parameters.AddWithValue("to", updatedLog.TotalTime);
                    cmd.Parameters.AddWithValue("transportMode", updatedLog.Rating);

                    return cmd.ExecuteNonQuery();
                });
            }
            catch (PostgresException)
            {
            }

            return affectedRows > 0;
        }

        public bool DeleteLogById(Guid id)
        {
            var affectedRows = 0;

            try
            {
                affectedRows = databaseManager.ExecuteWithConnection(connection =>
                {
                    using var cmd = new NpgsqlCommand(DeleteLogByIdCommand, connection);
                    cmd.Parameters.AddWithValue("id", id);

                    return cmd.ExecuteNonQuery();
                });
            }
            catch (PostgresException)
            {
            }

            return affectedRows > 0;
        }

        private Log ReadLogsDetails(IDataRecord record)
        {
            var message = new Log
            {
                Id = record.GetGuid(0),
                Date= Convert.ToDateTime(record["date"]),
                Comment = Convert.ToString(record["comment"]) ?? String.Empty,
                Difficulty = Enum.Parse<DifficultyEnum>(Convert.ToString(record["difficulty"]) ?? String.Empty),
                TotalTime = Convert.ToInt32(record["totaltime"]),
                Rating = Enum.Parse<PopularityEnum>(Convert.ToString(record["rating"]) ?? String.Empty)
            };

            return message;
        }
    }
}
