using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using Tourplanner.Exceptions;
using Tourplanner.Models;

namespace Tourplanner.DataAccessLayer
{
    public class DatabaseManager : IDatabaseManager
    {
        private const string CreateTourTableCommand = @"create table if not exists tours
                                                        (
                                                            tour_id         uuid
                                                                constraint tours_pk
                                                                    primary key,
                                                            name            text            not null,
                                                            description     text            not null,
                                                            finish          text            not null,
                                                            start           text            not null,
                                                            transporttype   text            not null,
                                                            distance        numeric         not null,
                                                            time            integer         not null,
                                                            picpath         text            not null,
                                                            popularity      text            not null,
                                                            childfriendly   boolean         not null
                                                        );

                                                        alter table tours
                                                            owner to postgres;

                                                                create unique index if not exists tours_picpath_uindex
                                                                    on tours(picpath);

                                                                create unique index if not exists tours_tour_id_uindex
                                                                    on tours(tour_id);";

        private const string CreateLogTableCommand = @"create table if not exists logs
                                                        (
                                                            log_id     uuid
                                                                constraint table_name_pk
                                                                    primary key,
                                                            tour_id    uuid         not null
                                                                constraint logs_tours_tour_id_fk
                                                                    references tours
                                                                    on update cascade on delete cascade
                                                                    deferrable,
                                                            date       date            not null,
                                                            comment    text,
                                                            difficulty text            not null,
                                                            totaltime  integer         not null,
                                                            rating     text            not null
                                                        );

                                                        alter table logs
                                                            owner to postgres;

                                                        create unique index if not exists table_name_log_id_uindex
                                                            on logs (log_id);

                                                        ";

        private readonly IPostgreSqlDAOConfiguration configuration;

        public DatabaseManager(IPostgreSqlDAOConfiguration configuration)
        {
            this.configuration = configuration;

            EnsureTables();
        }

        private void EnsureTables()
        {
            CreateTable(CreateTourTableCommand);

            CreateTable(CreateLogTableCommand);
        }

        private void CreateTable(string command)
        {
            ExecuteWithConnection(connection =>
            {
                using var cmd = new NpgsqlCommand(command, connection);
                NpgsqlConnection.GlobalTypeMapper.MapEnum<TransportType>();
                return cmd.ExecuteNonQuery();
            });
        }

        public T ExecuteWithConnection<T>(Func<NpgsqlConnection, T> command)
        {
            try
            {
                using var connection = new NpgsqlConnection(configuration.ConnectionString);
                connection.Open();

                return command(connection);
            }
            catch (NpgsqlException e)
            {
                throw new DataAccessFailedException("Could not connect to the database", e);
            }
        }
    }
}
