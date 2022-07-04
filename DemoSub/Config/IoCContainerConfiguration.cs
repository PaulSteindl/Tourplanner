using Tourplanner.BusinessLayer;
using Tourplanner.DataAccessLayer;
using Tourplanner.ViewModels;
using Tourplanner.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourplanner.Navigation;

namespace Tourplanner.Configuration
{
    internal class IoCContainerConfiguration
    {
        private readonly IServiceProvider serviceProvider;

        public IoCContainerConfiguration()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IConfiguration>((_) =>
            {
                return new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", false, true)
                    .Build();
            });

            // Configuration setup
            services.AddSingleton<AppConfiguration>();
            services.AddSingleton<IPostgreSqlDAOConfiguration>(s => s.GetRequiredService<AppConfiguration>());
            services.AddSingleton<IMapQuestConfiguration>( s => s.GetRequiredService<AppConfiguration>());

            // Logger setup
            /*
            services.AddSingleton<ILoggerFactory, Log4NetFactory>(s =>
            {
                return new Log4NetFactory("log4net.config");
            });
            */

            // DAL setup
            services.AddSingleton<ITourDAO, TourDAO>();
            services.AddSingleton<ILogDAO, LogDAO>();
            services.AddSingleton<IFileDAO, FileDAO>();

            // BL setup
            services.AddSingleton<ICalculateAttributes, CalculateAttributes>();
            services.AddSingleton<ICheckInput, CheckInput>();
            services.AddSingleton<IImportManager, ImportManager>();
            services.AddSingleton<IExportManager, ExportManager>();
            services.AddSingleton<ILogManager, Tourplanner.BusinessLayer.LogManager>();
            services.AddSingleton<IDirections, Directions>();
            services.AddSingleton<IRouteManager, RouteManager>();

            // UI setup
            services.AddSingleton<INavigationService, NavigationService>(s =>
            {
                var navigationService = new NavigationService(s);
                //navigationService.RegisterNavigation<AboutViewModel, AboutDialog>();
                navigationService.RegisterNavigation<MainWindowViewModel, MainWindowView>((viewModel, window) =>
                {
                    //window.SearchBar.DataContext = viewModel.TourManagerViewModel;
                    //window.ResultView.DataContext = viewModel.SingleTourViewModel;
                });

                return navigationService;
            });
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<TourManagerViewModel>();
            services.AddTransient<SingleTourViewModel>();

            // finished
            serviceProvider = services.BuildServiceProvider();
        }

        public MainViewModel MainViewModel => serviceProvider.GetRequiredService<MainViewModel>();
        public MainWindowViewModel MainWindowViewModel => serviceProvider.GetRequiredService<MainWindowViewModel>();
        public TourManagerViewModel TourManagerViewModel => serviceProvider.GetRequiredService<TourManagerViewModel>();
        public SingleTourViewModel SingleTourViewModel => serviceProvider.GetRequiredService<SingleTourViewModel>();

        public INavigationService NavigationService => serviceProvider.GetRequiredService<INavigationService>();
    }
}
