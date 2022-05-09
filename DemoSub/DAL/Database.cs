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
    }
}
