using Tourplanner.BusinessLayer;
using Tourplanner.DataAccessLayer;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Npgsql;
using Tourplanner.Models;
using Tourplanner.Configuration;

namespace UnitTest
{
    public class Tests
    {
        //TODO einlesen von config
        static string MapQuestKey = "eb3UNjx8FKV32PpUHCVZMIt6zLzO0EcC";
        static string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=123;Database=swe2tourdb";
        static readonly NpgsqlConnection _connection = new NpgsqlConnection(connectionString);
        static HttpClient _client = new HttpClient();
        static IFileDAO _fileDAO = new FileDAO();
        static IDirections _directions = new Directions(MapQuestKey, _client);
        static IPostgreSqlDAOConfiguration _postgre = new ;
        static IRouteManager _routeManager = new RouteManager(_fileDAO, _directions);
        static IDatabaseManager _databaseManager = new DatabaseManager(_postgre);
        static ITourDAO _tourDAO = new TourDAO(_databaseManager);
        static ICheckInput _checkInput = new CheckInput();
        static ILogDAO _logDAO = new LogDAO(_connection);
        static ICalculateAttributes _calculateAttributes = new CalculateAttributes(_logDAO);
        static ITourManager _tourmanager = new TourManager(_routeManager, _tourDAO, _checkInput, _calculateAttributes);

        [TestFixture]
        public class TourTests
        {
            [Test]
            public async Task CreateTour()
            {
                var test = await _tourmanager.NewTour("testtour", "das ist ein Test, fuer unsere tour", "Washington, DC", "Vienna, MO", TransportType.Fastest);

                Console.WriteLine(test.Distance);

            }
        }
    }
}