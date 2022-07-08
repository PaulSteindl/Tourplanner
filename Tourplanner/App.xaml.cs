using log4net;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Tourplanner.ViewModels;
using Tourplanner.Configuration;
using Tourplanner.Shared.Log4Net;
using Tourplanner.Shared;

namespace Tourplanner
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly Shared.ILogger logger = LogingManager.GetLogger<App>();

        static App()
        {
            LogingManager.LoggerFactory = new Log4NetFactory("log4net.config");
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            logger.Debug("App started");

            var ioCConfig = new IoCContainerConfiguration();
            ioCConfig.NavigationService.NavigateTo<MainWindowViewModel>();
        }
    }
}
