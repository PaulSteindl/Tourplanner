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
using Tourplanner.Exceptions;

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

            foreach(char c in testString)
                Assert.Catch<ArgumentException>(() => checkInput.CheckUserInputTour(c.ToString(), c.ToString(), c.ToString(), c.ToString()));
        }
    }

    [TestFixture]
    public class TourManagerTests
    {
        Mock<IRouteManager> routeManager = new Mock<IRouteManager>();
        Mock<ITourLogManager> logManager = new Mock<ITourLogManager>();
        Mock<ITourDAO> tourDAO = new Mock<ITourDAO>();
        Mock<ILogDAO> logDAO = new Mock<ILogDAO>();
        Mock<ICheckInput> checkInput = new Mock<ICheckInput>();
        Mock<ICalculateAttributes> calcA = new Mock<ICalculateAttributes>();
        Mock<IFileDAO> fileDAO = new Mock<IFileDAO>();

        static readonly string? routeGuid = "11111111-2222-3333-4444-555555555555";
        static readonly Guid tourGuid = new Guid("55555555-4444-3333-2222-111111111111");

        Route testRoute = new Route
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

        Tour testTour = new Tour
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

        [Test]
        public void TestTourManagerNewTourValidInput()
        {
            LogingManager.LoggerFactory = new Log4NetFactory("..\\Tourplanner\\log4net.config");

            ITourManager tourManager = new TourManager
                (routeManager.Object, logManager.Object, tourDAO.Object, logDAO.Object, checkInput.Object, calcA.Object, fileDAO.Object);

            checkInput.Setup(t => t.CheckUserInputTour("TestName", "TestDescription", "TestFrom", "TestTo")).Returns(true);
            routeManager.Setup(t => t.GetFullRoute("TestFrom", "TestTo", TransportType.Bicycle, It.IsAny<Guid>())).ReturnsAsync(testRoute);
            tourDAO.Setup(t => t.InsertTour(It.IsAny<Tour>())).Returns(true);

            var newTour = tourManager.NewTour("TestName", "TestDescription", "TestFrom", "TestTo", TransportType.Bicycle).Result;

            newTour.Id = tourGuid;

            Assert.That(JsonConvert.SerializeObject(newTour), Is.EqualTo(JsonConvert.SerializeObject(testTour)));
        }

        [Test]
        public void TestTourManagerNewTourRouteIsNull()
        {
            LogingManager.LoggerFactory = new Log4NetFactory("..\\Tourplanner\\log4net.config");

            ITourManager tourManager = new TourManager
                (routeManager.Object, logManager.Object, tourDAO.Object, logDAO.Object, checkInput.Object, calcA.Object, fileDAO.Object);

            Route? nullRoute = null;
            Tour? nullTour = null;

            checkInput.Setup(t => t.CheckUserInputTour("TestName", "TestDescription", "TestFrom", "TestTo")).Returns(true);
            routeManager.Setup(t => t.GetFullRoute("TestFrom", "TestTo", TransportType.Bicycle, It.IsAny<Guid>())).ReturnsAsync(nullRoute);
            tourDAO.Setup(t => t.InsertTour(It.IsAny<Tour>())).Returns(true);

            var newTour = tourManager.NewTour("TestName", "TestDescription", "TestFrom", "TestTo", TransportType.Bicycle).Result;

            Assert.That(JsonConvert.SerializeObject(newTour), Is.EqualTo(JsonConvert.SerializeObject(nullTour)));
        }

        [Test]
        public void TestTourManagerNewTourCantInsertIntoDB()
        {
            LogingManager.LoggerFactory = new Log4NetFactory("..\\Tourplanner\\log4net.config");

            ITourManager tourManager = new TourManager
                (routeManager.Object, logManager.Object, tourDAO.Object, logDAO.Object, checkInput.Object, calcA.Object, fileDAO.Object);

            checkInput.Setup(t => t.CheckUserInputTour("TestName", "TestDescription", "TestFrom", "TestTo")).Returns(true);
            routeManager.Setup(t => t.GetFullRoute("TestFrom", "TestTo", TransportType.Bicycle, It.IsAny<Guid>())).ReturnsAsync(testRoute);
            tourDAO.Setup(t => t.InsertTour(It.IsAny<Tour>())).Returns(false);

            Assert.Throws<NullReferenceException>(() => tourManager.NewTour("TestName", "TestDescription", "TestFrom", "TestTo", TransportType.Bicycle));
        }
    }
}
