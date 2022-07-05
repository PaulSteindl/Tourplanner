using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourplanner.Models;
using System.Drawing;
using Npgsql;
using System.Data;
using Tourplanner.Exceptions;
using System.Globalization;

namespace Tourplanner.DataAccessLayer
{
    public class TourDAO : ITourDAO
    {
        private const string InsertTourCommand = "INSERT INTO tours(name, description, start, finish, transporttype, distance, time, picpath, popularity, childfriendly) VALUES (@name, @description, @start, @finish, @transporttype, @distance, @time, @picpath, @popularity, @childfriendly)";
        private const string SelectTourByIdCommand = "SELECT * FROM tours WHERE tour_id = @id";
        private const string SelectAllToursCommand = "SELECT * FROM tours";
        private const string UpdateTourByIdCommand = "UPDATE tours SET name = @name, description = @description, start = @start, finish = @finish, transporttype = @transporttype, distance = @distance, time = @time, picpath = @picpath, popularity = @popularity, childfriendly = @childfriendly WHERE tour_id = @id";
        private const string DeleteTourByIdCommand = "DELETE FROM tours WHERE tour_id = @id";

        private IDatabaseManager databaseManager;

        public TourDAO(IDatabaseManager databaseManager)
        {
            this.databaseManager = databaseManager;
        }

        public bool InsertTour(Tour newTour)
        {
            var affectedRows = 0;

            try
            {
                affectedRows = databaseManager.ExecuteWithConnection(connection =>
                {
                    using var cmd = new NpgsqlCommand(InsertTourCommand, connection);
                    cmd.Parameters.AddWithValue("name", newTour.Name);
                    cmd.Parameters.AddWithValue("description", newTour.Description);
                    cmd.Parameters.AddWithValue("start", newTour.From);
                    cmd.Parameters.AddWithValue("finish", newTour.To);
                    cmd.Parameters.AddWithValue("transporttype", newTour.Transporttype.ToString());
                    cmd.Parameters.AddWithValue("distance", newTour.Distance);
                    cmd.Parameters.AddWithValue("time", newTour.Time);
                    cmd.Parameters.AddWithValue("picpath", newTour.PicPath);
                    cmd.Parameters.AddWithValue("popularity", newTour.Popularity.ToString());
                    cmd.Parameters.AddWithValue("childfriendly", newTour.ChildFriendly);
                    return cmd.ExecuteNonQuery();
                });
            }
            catch (PostgresException)
            {
            }

            return affectedRows > 0;
        }

        public Tour? SelectTourById(Guid tourId)
        {
            try
            {
                return databaseManager.ExecuteWithConnection(connection =>
                {
                    Tour? tour = null;
                    using var cmd = new NpgsqlCommand(SelectTourByIdCommand, connection);
                    cmd.Parameters.AddWithValue("tour_id", tourId);
                    using var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        tour = ReadTourDetails(reader);
                    }

                    return tour;
                });
            }
            catch (PostgresException)
            {
            }

            return null;
        }

        public List<Tour> SelectAllTours()
        {
            try
            {
                return databaseManager.ExecuteWithConnection(connection =>
                {
                    var tours = new List<Tour>();

                    using var cmd = new NpgsqlCommand(SelectAllToursCommand, connection);
                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var tour = ReadTourDetails(reader);
                        tours.Add(tour);
                    }

                    return tours;
                });
            }
            catch (PostgresException)
            {
            }

            return new List<Tour>();
        }

        public bool UpdateTourById(Tour updatedTour)
        {
            var affectedRows = 0;

            try
            {
                affectedRows = databaseManager.ExecuteWithConnection(connection =>
                {
                    using var cmd = new NpgsqlCommand(UpdateTourByIdCommand, connection);
                    cmd.Parameters.AddWithValue("id", updatedTour.Id);
                    cmd.Parameters.AddWithValue("name", updatedTour.Name);
                    if (!String.IsNullOrEmpty(updatedTour.Description))
                        cmd.Parameters.AddWithValue("description", updatedTour.Description);
                    cmd.Parameters.AddWithValue("start", updatedTour.From);
                    cmd.Parameters.AddWithValue("finish", updatedTour.To);
                    cmd.Parameters.AddWithValue("transporttype", updatedTour.Transporttype);
                    cmd.Parameters.AddWithValue("distance", updatedTour.Distance);
                    cmd.Parameters.AddWithValue("time", updatedTour.Time);
                    cmd.Parameters.AddWithValue("picpath", updatedTour.PicPath);
                    if (updatedTour.Popularity.HasValue)
                        cmd.Parameters.AddWithValue("popularity", updatedTour.Popularity);
                    cmd.Parameters.AddWithValue("childfriendly", updatedTour.ChildFriendly);

                    return cmd.ExecuteNonQuery();
                });
            }
            catch (PostgresException)
            {
            }

            return affectedRows > 0;
        }

        public bool DeleteTourById(Guid id)
        {
            var affectedRows = 0;

            try
            {
                affectedRows = databaseManager.ExecuteWithConnection(connection =>
                {
                    using var cmd = new NpgsqlCommand(DeleteTourByIdCommand, connection);
                    cmd.Parameters.AddWithValue("id", id);

                    return cmd.ExecuteNonQuery();
                });
            }
            catch (PostgresException)
            {

            }

            return affectedRows > 0;
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
                Transporttype = Enum.Parse<TransportType>(Convert.ToString(record["transporttype"]) ?? String.Empty),
                Distance = double.Parse(Convert.ToString(record["distance"]) ?? String.Empty),
                Time = Convert.ToInt32(record["time"]),
                PicPath = Convert.ToString(record["picpath"]) ?? String.Empty,
                Popularity = Enum.Parse<PopularityEnum>(Convert.ToString(record["popularity"]) ?? String.Empty),
                ChildFriendly = Convert.ToBoolean(record["childfriendly"])
            };

            return message;
        }
    }
}

