using DemoSub.Search;
using DemoSub.ViewModels;
using Tourplanner.DAL.Database;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DemoSub
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var dataBase = new Database("Host=localhost;Port=5432;Username=postgres;Password=123;Database=swe2tourdb");
            var searchEngine = new StandardSearchEngine();
            var searchBarViewModel = new SearchBarViewModel();
            var resultViewModel = new ResultViewModel();
            var mainViewModel = new MainViewModel(searchEngine, searchBarViewModel, resultViewModel);

            var wnd = new MainWindow()
            {
                DataContext = mainViewModel,
                SearchBar = { DataContext = searchBarViewModel },
                ResultView = { DataContext = resultViewModel }
            };
            wnd.Show();
        }
    }
}
