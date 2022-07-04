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

        private readonly NpgsqlConnection _connection;

        public LogDAO(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public bool InsertLog(Log newLog)
        {
            var affectedRows = 0;

            try
            {
                using var cmd = new NpgsqlCommand(InsertLogCommand, _connection);
                cmd.Parameters.AddWithValue("date", newLog.Date);
                if(!String.IsNullOrEmpty(newLog.Comment))
                    cmd.Parameters.AddWithValue("comment", newLog.Comment);
                cmd.Parameters.AddWithValue("difficulty", newLog.Difficulty);
                cmd.Parameters.AddWithValue("totaltime", newLog.TotalTime);
                cmd.Parameters.AddWithValue("rating", newLog.Rating);

                affectedRows = cmd.ExecuteNonQuery();
            }
            catch (PostgresException)
            {
            }

            return affectedRows > 0;
        }

        public List<Log> SelectLogsByTourId(Guid tourId)
        {
            var logs = new List<Log>();

            try
            {
                using var cmd = new NpgsqlCommand(SelectLogsByTourIdCommand, _connection);
                cmd.Parameters.AddWithValue("tour_id", tourId);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var log = ReadLogsDetails(reader);
                    logs.Add(log);
                }
            }
            catch (PostgresException)
            {
            }

            return logs;
        }

        public bool UpdateLogById(Log updatedLog)
        {
            var affectedRows = 0;

            try
            {
                using var cmd = new NpgsqlCommand(UpdateLogByIdCommand, _connection);
                cmd.Parameters.AddWithValue("id", updatedLog.Id);
                cmd.Parameters.AddWithValue("name", updatedLog.Date);
                if (!String.IsNullOrEmpty(updatedLog.Comment))
                    cmd.Parameters.AddWithValue("description", updatedLog.Comment);
                cmd.Parameters.AddWithValue("from", updatedLog.Difficulty);
                cmd.Parameters.AddWithValue("to", updatedLog.TotalTime);
                cmd.Parameters.AddWithValue("transportMode", updatedLog.Rating);

                affectedRows = cmd.ExecuteNonQuery();
            }
            catch (PostgresException)
            {
            }

            return affectedRows > 0;
        }

        public bool DeleteLogById(Guid id)
        {
            var rowsAffected = 0;

            try
            {
                using var cmd = new NpgsqlCommand(DeleteLogByIdCommand, _connection);
                cmd.Parameters.AddWithValue("id", id);
                rowsAffected = cmd.ExecuteNonQuery();
            }
            catch (PostgresException)
            {
            }

            return rowsAffected > 0;
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
