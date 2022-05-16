using Npgsql;
using Tourplanner.Models;
using System;
using System.Data;
using System.Collections.Generic;
using System.Threading;
using Tourplanner.Enums;

namespace Tourplanner.DAL
{
    class DatabaseTourRepository : ITourRepository
    {
        private const string CreateTableCommand = @"create table if not exists tours
                                                    (
                                                        tour_id         uuid           not null
                                                            constraint tours_pk
                                                                primary key,
                                                        name            text           not null,
                                                        description     text           not null,
                                                        ""from""          text           not null,
                                                        ""to""            text           not null,
                                                        ""transportMode"" transporttype  not null,
                                                        distance        real           not null,
                                                        time            integer        not null,
                                                        picpath         text           not null,
                                                        popularity      popularityenum not null,
                                                        childfriendly   boolean        not null
                                                    );

                                                    alter table tours
                                                        owner to postgres;

                                                    create unique index if not exists tours_picpath_uindex
                                                        on tours (picpath);

                                                    create unique index if not exists tours_tour_id_uindex
                                                        on tours (tour_id);";

        private const string InsertTourCommand = "INSERT INTO tours(name, description, from, to, transportMode, distance, time, picpath, popularity, childfriendly) VALUES (@name, @description, @from, @to, @transportMode, @distance, @time, @picpath, @popularity, @childfriendly)";
        private const string SelectAllToursCommand = "SELECT * FROM tours";
        private const string UpdateTourByIdCommand = "UPDATE tours SET name = @name, description = @description, from = @from, to = @to, transportMode = @transportMode, distance = @distance, time = @time, picpath = @picpath, popularity = @popularity, childfriendly = @childfriendly WHERE tour_id = @id";
        private const string DeleteTourByIdCommand = "DELETE FROM tours WHERE tour_id = @id";

        private readonly NpgsqlConnection _connection;

        public DatabaseTourRepository(NpgsqlConnection connection)
        {
            _connection = connection;
            EnsureTables();
        }

        private void EnsureTables()
        {
            using var cmd = new NpgsqlCommand(CreateTableCommand, _connection);
            cmd.ExecuteNonQuery();
        }

        public bool InsertTour(Tour newTour)
        {
            var affectedRows = 0;

            try
            {
                using var cmd = new NpgsqlCommand(InsertTourCommand, _connection);
                cmd.Parameters.AddWithValue("name", newTour.Name);
                if(!String.IsNullOrEmpty(newTour.Description))
                    cmd.Parameters.AddWithValue("description", newTour.Description);
                cmd.Parameters.AddWithValue("from", newTour.From);
                cmd.Parameters.AddWithValue("to", newTour.To);
                cmd.Parameters.AddWithValue("transportMode", newTour.TransportMode);
                cmd.Parameters.AddWithValue("distance", newTour.Distance);
                cmd.Parameters.AddWithValue("time", newTour.Time);
                cmd.Parameters.AddWithValue("picpath", newTour.PicPath);
                if(newTour.Popularity.HasValue)
                    cmd.Parameters.AddWithValue("popularity", newTour.Popularity);
                cmd.Parameters.AddWithValue("childfriendly", newTour.ChildFriendly);

                affectedRows = cmd.ExecuteNonQuery();
            }
            catch (PostgresException)
            {
            }

            return affectedRows > 0;
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
                cmd.Parameters.AddWithValue("transportMode", updatedTour.TransportMode);
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
                TransportMode = Enum.Parse<TransportType>(Convert.ToString(record["transportMode"]) ?? String.Empty),
                Distance = float.Parse(Convert.ToString(record["distance"]) ?? String.Empty),
                Time = Convert.ToInt32(record["time"]),
                PicPath = Convert.ToString(record["picpath"]) ?? String.Empty,
                Popularity = Enum.Parse<PopularityEnum>(Convert.ToString(record["popularity"]) ?? String.Empty) ,
                ChildFriendly = Convert.ToBoolean(record["childfriendly"])
            };

            return message;
        }
    }
}
