using NUnit.Framework;
using Tourplanner.Shared;
using Tourplanner.Shared.Log4Net;
using Tourplanner.BusinessLayer;
using Tourplanner.DataAccessLayer;
using Tourplanner.DataAccessLayerInMemory;
using Tourplanner.Models;
using Moq;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Tourplanner.UnitTests
{
    [TestFixture]
    public class CheckInputTests
    {
        [Test]
        [TestCase("a b c d e f g h i j k l m n o p q r s t u v w x y z")]
        [TestCase("A B C D E F G H I J K L M N O P Q R S T U V W X Y Z")]
        [TestCase("1 2 3 4 5 6 7 8 9 0")]
        [TestCase("- ,")]
        public void TestCheckUserInputTourFunctionValidInput(string testString)
        {
            ICheckInput checkInput = new CheckInput();

            Assert.IsTrue(checkInput.CheckUserInputTour(testString, testString, testString, testString));
        }

        [Test]
        [TestCase("±!@#$%^&*()_+=§£™¡¢∞§¶•ªº≠")]
        [TestCase("œ∑´®†¥¨ˆøπ“‘åß∂ƒ©˙˚¬…æ«`~Ω≈ç√∫˜µ≤≥÷₩")]
        [TestCase("ÈÉÊËĒĖĘÀÁÂÄÆÃĀŚŠŸÛÜÙÚŪÎÏÍĪĮÌÔÖÒÓŒŌÕŁŽŹŻÇĆČÑŃ")]
        [TestCase("èéêëēėęàáâäæãāśšÿûüùúūîïíīįìôöòóœōõłžźżçćčñń")]
        public void TestCheckUserInputTourFunctionInvalidInput(string testString)
        {
            ICheckInput checkInput = new CheckInput();

            foreach (char c in testString)
                Assert.Catch<ArgumentException>(() => checkInput.CheckUserInputTour(c.ToString(), c.ToString(), c.ToString(), c.ToString()));
        }
    }

    [TestFixture]
    public class TourManagerTests
    {
        Mock<IRouteManager> routeManager = new();
        Mock<ITourLogManager> logManager = new();
        Mock<ITourDAO> tourDAO = new();
        Mock<ILogDAO> logDAO = new();
        Mock<ICheckInput> checkInput = new();
        Mock<ICalculateAttributes> calcA = new();
        Mock<IFileDAO> fileDAO = new();

        string? routeGuid = String.Empty;
        Guid tourGuid = new();
        Guid logGuid = new();

        RouteInfo? nullRouteInfo = null;
        Tour? nullTour = null;

        Route testRoute = new();
        Tour testTour = new();
        Log testLog = new();
        RouteInfo testRouteInfo = new();
        Info testInfo = new();

        [SetUp]
        public void Init()
        {
            LogingManager.LoggerFactory = new Log4NetFactory("..\\Tourplanner\\log4net.config");

            routeManager = new Mock<IRouteManager>();
            logManager = new Mock<ITourLogManager>();
            tourDAO = new Mock<ITourDAO>();
            logDAO = new Mock<ILogDAO>();
            checkInput = new Mock<ICheckInput>();
            calcA = new Mock<ICalculateAttributes>();
            fileDAO = new Mock<IFileDAO>();

            routeGuid = "11111111-2222-3333-4444-555555555555";
            tourGuid = new Guid("55555555-4444-3333-2222-111111111111");
            logGuid = new Guid("33333333-2222-1111-4444-555555555555");

            nullRouteInfo = null;
            nullTour = null;

            testRoute = new Route
            {
                SessionId = routeGuid,
                Distance = 123456.123456,
                PicPath = "TestPicPath",
                Time = 12345,
                BoundingBox = new BoundingBox
                {
                    Lr = new LrUl
                    {
                        Lat = 111,
                        Lng = 222
                    },
                    Ul = new LrUl
                    {
                        Lat = 333,
                        Lng = 444
                    }
                }
            };

            testInfo = new Info
            {
                Messages = null,
                Statuscode = 0
            };

            testRouteInfo = new RouteInfo
            {
                Route = testRoute,
                Info = testInfo

            };

            testTour = new Tour
            {
                Id = tourGuid,
                Name = "TestName",
                Description = "TestDescription",
                From = "TestFrom",
                To = "TestTo",
                Transporttype = TransportType.Bicycle,
                ChildFriendly = false,
                Logs = new List<Log>(),
                Distance = 123456.123456,
                PicPath = "TestPicPath",
                Popularity = PopularityEnum.Bad,
                Time = 12345
            };

            testLog = new Log
            {
                Comment = "TestComment",
                Date = DateTime.MinValue,
                Difficulty = DifficultyEnum.Medium,
                Id = logGuid,
                Rating = PopularityEnum.Excellent,
                TotalTime = 123,
                TourId = tourGuid
            };
        }

        [Test]
        public void TestTourManagerNewTourValidInput()
        {
            ITourManager tourManager = new TourManager
                (routeManager.Object, logManager.Object, tourDAO.Object, logDAO.Object, checkInput.Object, calcA.Object, fileDAO.Object);

            checkInput.Setup(t => t.CheckUserInputTour("TestName", "TestDescription", "TestFrom", "TestTo")).Returns(true);
            routeManager.Setup(t => t.GetFullRoute("TestFrom", "TestTo", TransportType.Bicycle, It.IsAny<Guid>())).ReturnsAsync(testRouteInfo);
            tourDAO.Setup(t => t.InsertTour(It.IsAny<Tour>())).Returns(true);

            var newTour = tourManager.NewTour("TestName", "TestDescription", "TestFrom", "TestTo", TransportType.Bicycle).Result;

            newTour.Id = tourGuid;

            Assert.That(JsonConvert.SerializeObject(newTour), Is.EqualTo(JsonConvert.SerializeObject(testTour)));
        }

        [Test]
        public void TestTourManagerNewTourRouteIsNull()
        {
            ITourManager tourManager = new TourManager
                (routeManager.Object, logManager.Object, tourDAO.Object, logDAO.Object, checkInput.Object, calcA.Object, fileDAO.Object);

            checkInput.Setup(t => t.CheckUserInputTour("TestName", "TestDescription", "TestFrom", "TestTo")).Returns(true);
            routeManager.Setup(t => t.GetFullRoute("TestFrom", "TestTo", TransportType.Bicycle, It.IsAny<Guid>())).ReturnsAsync(nullRouteInfo);
            tourDAO.Setup(t => t.InsertTour(It.IsAny<Tour>())).Returns(true);

            var newTour = tourManager.NewTour("TestName", "TestDescription", "TestFrom", "TestTo", TransportType.Bicycle).Result;

            Assert.That(JsonConvert.SerializeObject(newTour), Is.EqualTo(JsonConvert.SerializeObject(nullTour)));
        }

        [Test]
        public void TestTourManagerNewTourInvalidUserInput()
        {
            ITourManager tourManager = new TourManager
                (routeManager.Object, logManager.Object, tourDAO.Object, logDAO.Object, checkInput.Object, calcA.Object, fileDAO.Object);

            checkInput.Setup(t => t.CheckUserInputTour("TestName", "TestDescription", "TestFrom", "TestTo")).Throws(new ArgumentException());
            routeManager.Setup(t => t.GetFullRoute("#TestFrom", "TestTo", TransportType.Bicycle, It.IsAny<Guid>())).ReturnsAsync(testRouteInfo);
            tourDAO.Setup(t => t.InsertTour(It.IsAny<Tour>())).Returns(false);

            var res = tourManager.NewTour("#TestName", "TestDescription", "TestFrom", "TestTo", TransportType.Bicycle).Result;

            Assert.That(res, Is.EqualTo(nullTour));
        }

        [Test]
        public void TestTourManagerNewTourDidntSaveInDAO()
        {
            ITourManager tourManager = new TourManager
                (routeManager.Object, logManager.Object, tourDAO.Object, logDAO.Object, checkInput.Object, calcA.Object, fileDAO.Object);

            checkInput.Setup(t => t.CheckUserInputTour("TestName", "TestDescription", "TestFrom", "TestTo")).Returns(true);
            routeManager.Setup(t => t.GetFullRoute("TestFrom", "TestTo", TransportType.Bicycle, It.IsAny<Guid>())).ReturnsAsync(nullRouteInfo);
            tourDAO.Setup(t => t.InsertTour(It.IsAny<Tour>())).Returns(false);

            var newTour = tourManager.NewTour("TestName", "TestDescription", "TestFrom", "TestTo", TransportType.Bicycle).Result;

            Assert.That(newTour, Is.EqualTo(nullTour));
        }

        [Test]
        public void TestTourManagerUpdateTourValidInput()
        {
            ITourManager tourManager = new TourManager
                (routeManager.Object, logManager.Object, tourDAO.Object, logDAO.Object, checkInput.Object, calcA.Object, fileDAO.Object);

            checkInput.Setup(t => t.CheckUserInputTour("TestNameNew", "TestDescriptionNew", "TestFromNew", "TestToNew")).Returns(true);
            routeManager.Setup(t => t.GetFullRoute("TestFromNew", "TestToNew", TransportType.Fastest, It.IsAny<Guid>())).ReturnsAsync(testRouteInfo);
            tourDAO.Setup(t => t.UpdateTourById(It.IsAny<Tour>())).Returns(true);
            calcA.Setup(t => t.CalculateChildFriendly(It.IsAny<IEnumerable<Log>>(), It.IsAny<double>())).Returns(true);
            calcA.Setup(t => t.CalculatePopularity(It.IsAny<IEnumerable<Log>>())).Returns(PopularityEnum.Good);

            var updatedTour = tourManager.UpdateTour("TestNameNew", "TestDescriptionNew", "TestFromNew", "TestToNew", TransportType.Fastest, testTour).Result;

            Tour updatedTestTour = new Tour
            {
                Id = tourGuid,
                Name = "TestNameNew",
                Description = "TestDescriptionNew",
                From = "TestFromNew",
                To = "TestToNew",
                Transporttype = TransportType.Fastest,
                ChildFriendly = true,
                Logs = testTour.Logs,
                Distance = testRoute.Distance,
                PicPath = testRoute.PicPath,
                Popularity = PopularityEnum.Good,
                Time = testRoute.Time
            };

            Assert.That(JsonConvert.SerializeObject(updatedTour), Is.EqualTo(JsonConvert.SerializeObject(updatedTestTour)));
        }

        [Test]
        public void TestTourManagerUpdatedTourRouteIsNull()
        {
            ITourManager tourManager = new TourManager
                (routeManager.Object, logManager.Object, tourDAO.Object, logDAO.Object, checkInput.Object, calcA.Object, fileDAO.Object);

            checkInput.Setup(t => t.CheckUserInputTour("TestNameNew", "TestDescriptionNew", "TestFromNew", "TestToNew")).Returns(true);

            routeManager.Setup(t => t.GetFullRoute("TestFromNew", "TestToNew", TransportType.Fastest, It.IsAny<Guid>())).ReturnsAsync(nullRouteInfo);
            tourDAO.Setup(t => t.UpdateTourById(It.IsAny<Tour>())).Returns(true);
            calcA.Setup(t => t.CalculateChildFriendly(It.IsAny<IEnumerable<Log>>(), It.IsAny<double>())).Returns(true);
            calcA.Setup(t => t.CalculatePopularity(It.IsAny<IEnumerable<Log>>())).Returns(PopularityEnum.Good);

            var updatedTour = tourManager.UpdateTour("TestNameNew", "TestDescriptionNew", "TestFromNew", "TestToNew", TransportType.Fastest, testTour).Result;

            Tour updatedTestTour = new Tour
            {
                Id = tourGuid,
                Name = "TestNameNew",
                Description = "TestDescriptionNew",
                From = "TestFromNew",
                To = "TestToNew",
                Transporttype = TransportType.Fastest,
                ChildFriendly = true,
                Logs = testTour.Logs,
                Distance = testRoute.Distance,
                PicPath = testRoute.PicPath,
                Popularity = PopularityEnum.Good,
                Time = testRoute.Time
            };

            Assert.That(JsonConvert.SerializeObject(updatedTour), Is.EqualTo(JsonConvert.SerializeObject(testTour)));
        }


        [Test]
        public void TestTourManagerUpdatedTourInvalidUserInput()
        {
            ITourManager tourManager = new TourManager
                (routeManager.Object, logManager.Object, tourDAO.Object, logDAO.Object, checkInput.Object, calcA.Object, fileDAO.Object);

            checkInput.Setup(t => t.CheckUserInputTour("TestName", "TestDescription", "TestFrom", "TestTo")).Returns(true);
            routeManager.Setup(t => t.GetFullRoute("#TestFrom", "TestTo", TransportType.Bicycle, It.IsAny<Guid>())).ReturnsAsync(testRouteInfo);
            tourDAO.Setup(t => t.InsertTour(It.IsAny<Tour>())).Returns(false);

            var updatedTour = tourManager.UpdateTour("TestNameNew", "TestDescriptionNew", "TestFromNew", "TestToNew", TransportType.Fastest, testTour).Result;

            Assert.That(updatedTour, Is.EqualTo(nullTour));
        }

        [Test]
        public void TestTourManagerUpdatedTourDidntSaveInDAO()
        {
            ITourManager tourManager = new TourManager
                (routeManager.Object, logManager.Object, tourDAO.Object, logDAO.Object, checkInput.Object, calcA.Object, fileDAO.Object);

            checkInput.Setup(t => t.CheckUserInputTour("TestName", "TestDescription", "TestFrom", "TestTo")).Returns(true);
            routeManager.Setup(t => t.GetFullRoute("TestFrom", "TestTo", TransportType.Bicycle, It.IsAny<Guid>())).ReturnsAsync(nullRouteInfo);
            tourDAO.Setup(t => t.InsertTour(It.IsAny<Tour>())).Returns(false);

            var updatedTour = tourManager.UpdateTour("TestNameNew", "TestDescriptionNew", "TestFromNew", "TestToNew", TransportType.Fastest, testTour).Result;

            Assert.That(updatedTour, Is.EqualTo(nullTour));
        }

        [Test]
        public void TestTourManagerLoadToursWithNoEntry()
        {
            ITourDAO tourDAO = new TourInMemoryDAO();

            ITourManager tourManager = new TourManager
                (routeManager.Object, logManager.Object, tourDAO, logDAO.Object, checkInput.Object, calcA.Object, fileDAO.Object);

            var emptyTourList = new List<Tour>();

            var retrieveList = tourManager.LoadTours().Result;

            Assert.That(retrieveList, Is.EqualTo(emptyTourList));
        }

        [Test]
        public void TestTourManagerLoadToursWithTwoEntrys()
        {

            ITourManager tourManager = new TourManager
                (routeManager.Object, logManager.Object, tourDAO.Object, logDAO.Object, checkInput.Object, calcA.Object, fileDAO.Object);

            var tours = new List<Tour>
            {
                testTour,
                testTour
            };

            var logs = new List<Log>
            {
                testLog,
                testLog
            };

            tours[0].Logs = new List<Log>();
            tours[1].Logs = new List<Log>();

            var resTours = new List<Tour>
            {
                testTour,
                testTour
            };

            resTours[0].Logs = logs;
            resTours[1].Logs = logs;

            tourDAO.Setup(t => t.SelectAllTours()).Returns(tours);
            logDAO.Setup(t => t.SelectLogsByTourId(It.IsAny<Guid>())).Returns(logs);

            var retrieveList = tourManager.LoadTours().Result;

            Assert.That(JsonConvert.SerializeObject(retrieveList), Is.EqualTo(JsonConvert.SerializeObject(resTours)));
        }

        [Test]
        public void TestTourManagerDoesMapExistAsync()
        {
            ITourManager tourManager = new TourManager
                    (routeManager.Object, logManager.Object, tourDAO.Object, logDAO.Object, checkInput.Object, calcA.Object, fileDAO.Object);

            var tryRoute = testRoute;
            tryRoute.PicPath = "NewTestPicPath";
            var resTour = testTour;
            resTour.PicPath = tryRoute.PicPath;
            var tryRouteInfo = testRouteInfo;
            testRouteInfo.Route = tryRoute;

            routeManager.Setup(t => t.GetFullRoute(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TransportType>(), It.IsAny<Guid>())).ReturnsAsync(tryRouteInfo);

            var updatedTour = tourManager.DoesMapExistAsync(testTour).Result;

            Assert.That(JsonConvert.SerializeObject(updatedTour), Is.EqualTo(JsonConvert.SerializeObject(resTour)));
        }
    }

    [TestFixture]
    public class TourLogManagerTest
    {
        Mock<ILogDAO> logDAO = new();
        Mock<ICheckInput> checkInput = new();

        Guid tourGuid = new();
        Guid logGuid = new();

        Log? nullLog = null;
        Tour? nullTour = null;
        Log testLog = new();
        Log testLog2 = new();
        Tour testTour = new();

        [SetUp]
        public void Init()
        {
            LogingManager.LoggerFactory = new Log4NetFactory("..\\Tourplanner\\log4net.config");

            logDAO = new Mock<ILogDAO>();
            checkInput = new Mock<ICheckInput>();

            tourGuid = new Guid("55555555-4444-3333-2222-111111111111");
            logGuid = new Guid("33333333-2222-1111-4444-555555555555");

            nullLog = null;
            nullTour = null;

            testLog = new Log
            {
                Comment = "TestComment",
                Date = DateTime.MinValue,
                Difficulty = DifficultyEnum.Medium,
                Id = logGuid,
                Rating = PopularityEnum.Excellent,
                TotalTime = 123,
                TourId = tourGuid
            };

            testLog2 = new Log
            {
                Comment = "TestComment2",
                Date = DateTime.MaxValue,
                Difficulty = DifficultyEnum.Extreme,
                Id = logGuid,
                Rating = PopularityEnum.Bad,
                TotalTime = 321,
                TourId = tourGuid
            };

            testTour = new Tour
            {
                Id = tourGuid,
                Name = "TestName",
                Description = "TestDescription",
                From = "TestFrom",
                To = "TestTo",
                Transporttype = TransportType.Bicycle,
                ChildFriendly = false,
                Logs = new List<Log>(),
                Distance = 123456.123456,
                PicPath = "TestPicPath",
                Popularity = PopularityEnum.Bad,
                Time = 12345
            };
        }

        [Test]
        public void TestTourLogManagerCreateLogValidInput()
        {
            ITourLogManager tourLogManager = new TourLogManager(logDAO.Object, checkInput.Object);

            checkInput.Setup(t => t.CheckUserInputLog("TestComment")).Returns(true);
            logDAO.Setup(t => t.InsertLog(It.IsAny<Log>())).Returns(true);

            var newLog = tourLogManager.CreateLog("TestComment", 123, DateTime.MinValue, DifficultyEnum.Medium, PopularityEnum.Excellent, tourGuid).Result;

            newLog.Id = logGuid;

            Assert.That(JsonConvert.SerializeObject(newLog), Is.EqualTo(JsonConvert.SerializeObject(testLog)));
        }

        [Test]
        public void TestTourLogManagerCreateLogInvalidInput()
        {
            ITourLogManager tourLogManager = new TourLogManager(logDAO.Object, checkInput.Object);

            checkInput.Setup(t => t.CheckUserInputLog("TestComment")).Throws(new ArgumentException());

            var newLog = tourLogManager.CreateLog("TestComment", 123, DateTime.MinValue, DifficultyEnum.Medium, PopularityEnum.Excellent, tourGuid).Result;

            Assert.That(JsonConvert.SerializeObject(newLog), Is.EqualTo(JsonConvert.SerializeObject(nullLog)));
        }

        [Test]
        public void TestTourLogManagerCreateLogCantBeSavedInDAO()
        {
            ITourLogManager tourLogManager = new TourLogManager(logDAO.Object, checkInput.Object);

            checkInput.Setup(t => t.CheckUserInputLog("TestComment")).Returns(true);
            logDAO.Setup(t => t.InsertLog(It.IsAny<Log>())).Returns(false);

            var newLog = tourLogManager.CreateLog("TestComment", 123, DateTime.MinValue, DifficultyEnum.Medium, PopularityEnum.Excellent, tourGuid).Result;

            Assert.That(JsonConvert.SerializeObject(newLog), Is.EqualTo(JsonConvert.SerializeObject(nullLog)));
        }

        [Test]
        public void TestTourLogManagerUpdateLogValidInput()
        {
            ITourLogManager tourLogManager = new TourLogManager(logDAO.Object, checkInput.Object);

            checkInput.Setup(t => t.CheckUserInputLog("TestComment2")).Returns(true);
            logDAO.Setup(t => t.UpdateLogById(It.IsAny<Log>())).Returns(true);

            var updatedLog = tourLogManager.UpdateLog("TestComment2", 321, DateTime.MaxValue, DifficultyEnum.Extreme, PopularityEnum.Bad, testLog).Result;

            Assert.That(JsonConvert.SerializeObject(updatedLog), Is.EqualTo(JsonConvert.SerializeObject(testLog2)));
        }

        [Test]
        public void TestTourLogManagerUpdateLogInvalidInput()
        {
            ITourLogManager tourLogManager = new TourLogManager(logDAO.Object, checkInput.Object);

            checkInput.Setup(t => t.CheckUserInputLog("TestComment2")).Throws(new ArgumentException());

            var updatedLog = tourLogManager.UpdateLog("TestComment2", 321, DateTime.MaxValue, DifficultyEnum.Extreme, PopularityEnum.Bad, testLog).Result;

            Assert.That(JsonConvert.SerializeObject(updatedLog), Is.EqualTo(JsonConvert.SerializeObject(nullLog)));
        }

        [Test]
        public void TestTourLogManagerUpdateLogCantBeSavedInDAO()
        {
            ITourLogManager tourLogManager = new TourLogManager(logDAO.Object, checkInput.Object);

            checkInput.Setup(t => t.CheckUserInputLog("TestComment2")).Returns(true);
            logDAO.Setup(t => t.UpdateLogById(It.IsAny<Log>())).Returns(false);

            var updatedLog = tourLogManager.UpdateLog("TestComment2", 321, DateTime.MaxValue, DifficultyEnum.Extreme, PopularityEnum.Bad, testLog).Result;

            Assert.That(JsonConvert.SerializeObject(updatedLog), Is.EqualTo(JsonConvert.SerializeObject(nullLog)));
        }

        [Test]
        public void TestTourLogManagerGetAllLogsByTourIdWithEmptyDB()
        {
            ILogDAO logDAO = new LogInMemoryDAO();
            ITourLogManager tourLogManager = new TourLogManager(logDAO, checkInput.Object);

            var logList = new List<Log>();

            var retList = tourLogManager.GetAllLogsByTourId(tourGuid);

            Assert.That(JsonConvert.SerializeObject(retList), Is.EqualTo(JsonConvert.SerializeObject(logList)));
        }
    }
}
