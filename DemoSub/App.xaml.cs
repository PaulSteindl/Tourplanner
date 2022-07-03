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
            var searchBarViewModel = new SearchBarViewModel();
            var resultViewModel = new ResultViewModel();
            var mainViewModel = new MainViewModel(searchBarViewModel, resultViewModel);

            var wnd = new MainWindowView()
            {
                DataContext = mainViewModel,
            };
            wnd.Show();
        }
    }
}
