using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourplanner.Models;
using System.Drawing;
using Npgsql;
using System.Data;

namespace Tourplanner.DataAccessLayer
{
    internal class TourDAO : ITourDAO
    {
        private const string InsertTourCommand = "INSERT INTO tours(name, description, from, to, transportMode, distance, time, picpath, popularity, childfriendly) VALUES (@name, @description, @from, @to, @transportMode, @distance, @time, @picpath, @popularity, @childfriendly)";
        private const string SelectTourByIdCommand = "SELECT * FROM tours WHERE tour_id = @id";
        private const string SelectAllToursCommand = "SELECT * FROM tours";
        private const string UpdateTourByIdCommand = "UPDATE tours SET name = @name, description = @description, from = @from, to = @to, transportMode = @transportMode, distance = @distance, time = @time, picpath = @picpath, popularity = @popularity, childfriendly = @childfriendly WHERE tour_id = @id";
        private const string DeleteTourByIdCommand = "DELETE FROM tours WHERE tour_id = @id";

        private readonly NpgsqlConnection _connection;

        public TourDAO(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public bool InsertTour(Tour newTour)
        {
            var affectedRows = 0;

            try
            {
                using var cmd = new NpgsqlCommand(InsertTourCommand, _connection);
                cmd.Parameters.AddWithValue("name", newTour.Name);
                if (!String.IsNullOrEmpty(newTour.Description))
                    cmd.Parameters.AddWithValue("description", newTour.Description);
                cmd.Parameters.AddWithValue("from", newTour.From);
                cmd.Parameters.AddWithValue("to", newTour.To);
                cmd.Parameters.AddWithValue("transportMode", newTour.Transporttype);
                cmd.Parameters.AddWithValue("distance", newTour.Distance);
                cmd.Parameters.AddWithValue("time", newTour.Time);
                cmd.Parameters.AddWithValue("picpath", newTour.PicPath);
                if (newTour.Popularity.HasValue)
                    cmd.Parameters.AddWithValue("popularity", newTour.Popularity);
                cmd.Parameters.AddWithValue("childfriendly", newTour.ChildFriendly);

                affectedRows = cmd.ExecuteNonQuery();
            }
            catch (PostgresException)
            {
            }

            return affectedRows > 0;
        }

        public Tour SelectTourById(Guid tourId)
        {
            var tour = new Tour();

            try
            {
                using var cmd = new NpgsqlCommand(SelectTourByIdCommand, _connection);
                cmd.Parameters.AddWithValue("tour_id", tourId);
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    tour = ReadTourDetails(reader);
                }
            }
            catch (PostgresException)
            {
            }

            return tour;
        }

        public List<Tour> SelectAllTours()
        {
            var tours = new List<Tour>();

            try
            {
                using var cmd = new NpgsqlCommand(SelectAllToursCommand, _connection);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var tour = ReadTourDetails(reader);
                    tours.Add(tour);
                }
            }
            catch (PostgresException)
            {
            }

            return tours;
        }

        public bool UpdateTourById(Tour updatedTour)
        {
            var affectedRows = 0;

            try
            {
                using var cmd = new NpgsqlCommand(UpdateTourByIdCommand, _connection);
                cmd.Parameters.AddWithValue("id", updatedTour.Id);
                cmd.Parameters.AddWithValue("name", updatedTour.Name);
                if (!String.IsNullOrEmpty(updatedTour.Description))
                    cmd.Parameters.AddWithValue("description", updatedTour.Description);
                cmd.Parameters.AddWithValue("from", updatedTour.From);
                cmd.Parameters.AddWithValue("to", updatedTour.To);
                cmd.Parameters.AddWithValue("transportMode", updatedTour.Transporttype);
                cmd.Parameters.AddWithValue("distance", updatedTour.Distance);
                cmd.Parameters.AddWithValue("time", updatedTour.Time);
                cmd.Parameters.AddWithValue("picpath", updatedTour.PicPath);
                if (updatedTour.Popularity.HasValue)
                    cmd.Parameters.AddWithValue("popularity", updatedTour.Popularity);
                cmd.Parameters.AddWithValue("childfriendly", updatedTour.ChildFriendly);

                affectedRows = cmd.ExecuteNonQuery();
            }
            catch (PostgresException)
            {
            }

            return affectedRows > 0;
        }

        public bool DeleteTourById(Guid id)
        {
            var rowsAffected = 0;

            try
            {
                using var cmd = new NpgsqlCommand(DeleteTourByIdCommand, _connection);
                cmd.Parameters.AddWithValue("id", id);
                rowsAffected = cmd.ExecuteNonQuery();
            }
            catch (PostgresException)
            {

            }

            return rowsAffected > 0;
        }

        private Tour ReadTourDetails(IDataRecord record)
        {
            var message = new Tour
            {
                Id = record.GetGuid(0),
                Name = Convert.ToString(record["name"]) ?? String.Empty,
                Description = Convert.ToString(record["description"]),
                From = Convert.ToString(record["from"]) ?? String.Empty,
                To = Convert.ToString(record["to"]) ?? String.Empty,
                Transporttype = Enum.Parse<TransportType>(Convert.ToString(record["transportMode"]) ?? String.Empty),
                Distance = float.Parse(Convert.ToString(record["distance"]) ?? String.Empty),
                Time = Convert.ToInt32(record["time"]),
                PicPath = Convert.ToString(record["picpath"]) ?? String.Empty,
                Popularity = Enum.Parse<PopularityEnum>(Convert.ToString(record["popularity"]) ?? String.Empty),
                ChildFriendly = Convert.ToBoolean(record["childfriendly"])
            };

            return message;
        }
    }
}

