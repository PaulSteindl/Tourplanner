using DemoSub.ViewModels;
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
using Tourplanner.Views;

namespace DemoSub
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class Application : System.Windows.Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var mainWindowViewModel = new MainWindowViewModel();
            //var tourManagerViewModel = new TourManagerViewModel();
            //var singleTourViewModel = new SingleTourViewModel();

            var wnd = new MainWindowView()
            {
                DataContext = mainWindowViewModel,
            };
            wnd.Show();
        }
    }
}
