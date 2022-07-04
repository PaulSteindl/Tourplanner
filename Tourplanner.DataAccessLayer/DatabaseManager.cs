using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using Tourplanner.Exceptions;

namespace Tourplanner.DataAccessLayer
{
    public class DatabaseManager : IDatabaseManager
    {
        private const string CreateTourTableCommand = @"create table if not exists tours
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

        private const string CreateLogTableCommand = @"create table if not exists logs
                                                    (
                                                        log_id     uuid           not null
                                                            constraint table_name_pk
                                                                primary key,
                                                        tour_id    uuid           not null
                                                            constraint logs_tours_tour_id_fk
                                                                references tours
                                                                on update cascade on delete cascade
                                                                deferrable,
                                                        date       date           not null,
                                                        comment    text,
                                                        difficulty difficultyenum not null,
                                                        totaltime  integer        not null,
                                                        rating     popularityenum not null
                                                    );

                                                    alter table logs
                                                        owner to postgres;

                                                    create unique index if not exists table_name_log_id_uindex
                                                        on logs (log_id);";

        private const string createDifficultyCommand = @"DO $$
                                                            BEGIN
                                                                IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'difficultyenum') THEN
                                                                    CREATE TYPE difficultyenum AS ENUM ('Extreme', 'Complex', 'Hard', 'Medium', 'Easy');
                                                                END IF;
                                                            END
                                                         $$";

        private const string createPopularityCommand = @"DO $$
                                                            BEGIN
                                                                IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'popularityenum') THEN
                                                                    CREATE TYPE popularityenum AS ENUM ('Perfect', 'Excellent', 'Good', 'Okay', 'Bad');
                                                                END IF;
                                                            END
                                                         $$";

        private const string createTransportTypeCommand = @"DO $$
                                                                BEGIN
                                                                    IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'transporttype') THEN
                                                                        CREATE TYPE transporttype AS ENUM ('Auto', 'Walking', 'Bicyle');
                                                                    END IF;
                                                                END
                                                             $$";

        private readonly IPostgreSqlDAOConfiguration configuration;

        public DatabaseManager(IPostgreSqlDAOConfiguration configuration)
        {
            this.configuration = configuration;
            EnsureTables();
        }

        private void EnsureTables()
        {
            CreateTable(createDifficultyCommand);

            CreateTable(createPopularityCommand);

            CreateTable(createTransportTypeCommand);

            CreateTable(CreateTourTableCommand);

            CreateTable(CreateLogTableCommand);

            CreateTable(createDifficultyCommand);

            CreateTable(createPopularityCommand);

            CreateTable(createTransportTypeCommand);
        }

        private void CreateTable(string command)
        {
            ExecuteWithConnection(connection =>
            {
                using var cmd = new NpgsqlCommand(command, connection);
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
