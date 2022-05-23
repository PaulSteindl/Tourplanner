using Npgsql;
using Tourplanner.Exceptions;

namespace Tourplanner.DAL.Database
{
    public class Database
    {
        private readonly NpgsqlConnection _connection;
        public ITourRepository TourRepository { get; private set; }
        public ILogRepository LogRepository { get; private set; }

        public Database(string connectionString)
        {
            try
            {
                _connection = new NpgsqlConnection(connectionString);
                _connection.Open();

                CreateDBEnums();

                // zuerst Tour dann log, weil sie aufeinander aufbauen
                TourRepository = new DatabaseTourRepository(_connection);
                LogRepository = new DatabaseLogRepository(_connection);

            }
            catch (NpgsqlException e)
            {
                // provide our own custom exception
                throw new DataAccessFailedException("Could not connect to or initialize database", e);
            }
        }

        private void CreateDBEnums()
        {
            string createDifficultyCommand = @"DO $$
                                                BEGIN
                                                    IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'difficultyenum') THEN
                                                        CREATE TYPE difficultyenum AS ENUM ('Extreme', 'Complex', 'Hard', 'Medium', 'Easy');
                                                    END IF;
                                                END
                                             $$";

            string createPopularityCommand = @"DO $$
                                                BEGIN
                                                    IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'popularityenum') THEN
                                                        CREATE TYPE popularityenum AS ENUM ('Perfect', 'Excellent', 'Good', 'Okay', 'Bad');
                                                    END IF;
                                                END
                                             $$";

            string createTransportTypeCommand = @"DO $$
                                                    BEGIN
                                                        IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'transporttype') THEN
                                                            CREATE TYPE transporttype AS ENUM ('Auto', 'Walking', 'Bicyle');
                                                        END IF;
                                                    END
                                                 $$";

            using var difCmd = new NpgsqlCommand(createDifficultyCommand, _connection);
            difCmd.ExecuteNonQuery();

            using var popCmd = new NpgsqlCommand(createPopularityCommand, _connection);
            popCmd.ExecuteNonQuery();

            using var transCmd = new NpgsqlCommand(createTransportTypeCommand, _connection);
            transCmd.ExecuteNonQuery();
        }
    }
}
