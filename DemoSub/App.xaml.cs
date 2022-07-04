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

namespace Tourplanner
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {

        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var ioCConfig = new IoCContainerConfiguration();
            ioCConfig.NavigationService.NavigateTo<MainWindowViewModel>();
        }
    }
}
