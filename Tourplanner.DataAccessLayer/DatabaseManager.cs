using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace Tourplanner.DataAccessLayer
{
    public class DatabaseManager
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

        private readonly NpgsqlConnection _connection;

        public DatabaseManager(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public void EnsureTables()
        {
            using var cmdTour = new NpgsqlCommand(CreateTourTableCommand, _connection);
            cmdTour.ExecuteNonQuery();

            using var cmdLog = new NpgsqlCommand(CreateLogTableCommand, _connection);
            cmdLog.ExecuteNonQuery();

            using var difCmd = new NpgsqlCommand(createDifficultyCommand, _connection);
            difCmd.ExecuteNonQuery();

            using var popCmd = new NpgsqlCommand(createPopularityCommand, _connection);
            popCmd.ExecuteNonQuery();

            using var transCmd = new NpgsqlCommand(createTransportTypeCommand, _connection);
            transCmd.ExecuteNonQuery();
        }

    }
}
