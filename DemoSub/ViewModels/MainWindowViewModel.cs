using Tourplanner.ViewModels;
using Tourplanner.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tourplanner.Models;
using Tourplanner.BusinessLayer;
using AsyncAwaitBestPractices.MVVM;
using System.Windows;
using System.Collections.ObjectModel;
using Tourplanner;
using System.ComponentModel;
using System.Windows.Data;
using System.IO;
using Tourplanner.Views;

namespace Tourplanner.ViewModels
{
    internal class MainWindowViewModel : BaseViewModel, ICloseWindow
    {
        // Busy
        private bool isBusy;
        // Suche
        private string _searchText = String.Empty;
        // Exit
        public Action? Close { get; set; }
        // Tour
        private Tour? _tour;
        private ITourManager _tourManager;
        private TransportType _transportType;
        private ObservableCollection<Tour> AllTours = new ObservableCollection<Tour>();
        public ObservableCollection<Tour> ShownTours { get; set; } = new ObservableCollection<Tour>();
        public event EventHandler<Tour> TourChanged;
        // Log
        private Log _log;
        private ObservableCollection<Log> AllLogs = new ObservableCollection<Log>();
        public ObservableCollection<Log> ShownLogs { get; set; } = new ObservableCollection<Log>();
        private ILogManager _logManager;
        // Import
        private IImportManager _importManager;
        // Export
        private IExportManager _exportManager;
        // Report
        private IReportManager _reportManager;
        

        // Suche
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                UpdateShownTours();
            }
        }

        // Tour
        public Tour? Tour
        {
            get => _tour;
            set
            {
                _tour = value;
                //OnPropertyChanged();
                //UpdateShownTours();
            }
        }

        // Log
        public Log Log
        {
            get => _log;
            set
            {
                _log = value;
            }
        }

        public bool IsBusy
        {
            get => isBusy;
            private set
            {
                isBusy = value;
                OnPropertyChanged();

                // Occurs when the CommandManager detects conditions that might change the ability of a command to execute.
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public ICommand AddTourCommand { get; }
        public ICommand ModifyTourCommand { get; }
        public ICommand DeleteTourCommand { get; }
        public ICommand ShowTourDetailsCommand { get; }
        public ICommand AddTourLogCommand  { get; }
        public ICommand ModifyTourLogCommand { get; }
        public ICommand DeleteTourLogCommand { get; }
        public ICommand ImportTourCommand { get; }
        public ICommand ExportTourCommand { get; }
        public ICommand TourReportCommand { get; }
        public ICommand SummaryTourReportCommand { get; }
        public ICommand SearchFieldCommand { get; }
        public ICommand ResetSearchFieldCommand { get; }
        public ICommand ExitApplicationCommand { get; }


        public MainWindowViewModel(ITourManager tourManager, IImportManager importManager, IExportManager exportManager, IReportManager reportManager)
        {
            //IsBusy = true;
            AddTourCommand = new AsyncCommand(AddTour);
            ModifyTourCommand = new AsyncCommand(ModifyTour);
            DeleteTourCommand = new RelayCommand(DeleteTour);
            ShowTourDetailsCommand = new RelayCommand(ShowTourDetails);
            AddTourLogCommand = new RelayCommand(AddTourLog);
            ModifyTourLogCommand = new RelayCommand(ModifyTourLog);
            DeleteTourLogCommand = new RelayCommand(DeleteTourLog);
            ImportTourCommand = new AsyncCommand(ImportTour);
            ExportTourCommand = new AsyncCommand(ExportTour);
            TourReportCommand = new AsyncCommand(TourReport);
            SummaryTourReportCommand = new AsyncCommand(SummaryTourReport);
            SearchFieldCommand = new RelayCommand(SearchField);
            ResetSearchFieldCommand = new RelayCommand(ResetSearchField);
            ExitApplicationCommand = new RelayCommand(ExitApplication);
            this._tourManager = tourManager;
            this._importManager = importManager;
            this._exportManager = exportManager;
            this._reportManager = reportManager;
        }

        public MainWindowViewModel(MainWindowView view)
            => view.DataContext = this;

        // A BusyIndicator control provides an alternative to a wait cursor to show user an indication that an application is busy doing some processing.
        private async Task BusyIndicatorFunc(Func<Task> section)
        {
            isBusy = true;
            await section();
            isBusy = false;
        }

        private void UpdateShownTours()
        {
            if (AllTours is null) return;

            // Without clean, by adding a tour the previous tour will also be added again
            Application.Current.Dispatcher.Invoke(() => ShownTours.Clear());

            foreach (var tour in AllTours)
            {
                Application.Current.Dispatcher.Invoke(() => ShownTours.Add(tour));
            }
        }

        private TransportType ConverStringToTransportType(string transportType)
        {
            switch (transportType)
            {
                case "Fastest":
                    _transportType = (TransportType)0;
                    break;
                case "Shortest":
                    _transportType = (TransportType)1;
                    break;
                case "Pedestrian":
                    _transportType = (TransportType)2;
                    break;
                case "Bicycle":
                    _transportType = (TransportType)3;
                    break;
                default:
                    break;
            }
            return _transportType;
        }

        private void ExitApplication(object? obj)
        {
            // @TODO: check maybe if smth has changed => save of cancel changes with messagebox
            Application.Current.Shutdown();
        }

        private void ResetSearchField(object? obj)
            => SearchText = String.Empty;

        private void SearchField(object? obj)
        {
            if (AllTours is null) return;

            Func<Tour, bool> searchContainsName = containsName;
            Application.Current?.Dispatcher.Invoke(() => ShownTours.Clear());

            foreach(var tour in AllTours)
            {
                if (searchContainsName(tour))
                {
                    Application.Current?.Dispatcher.Invoke(() => ShownTours.Add(tour));
                }
            }

            bool containsName(Tour tour) => tour.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
        }

        private async Task SummaryTourReport()
        {
            throw new NotImplementedException();
        }

        private async Task TourReport()
        {
            if (isBusy) return;

            if (Tour is null) return;

            var selectedTour = Tour;
            var directoryPath = @"C:\TourReport\";

            try
            {
                var boolean = _reportManager.CreateReport(selectedTour, directoryPath);
            }
            catch (Exception ex)
            {
                throw new NullReferenceException("An error happend while creating a tour report: " + ex.Message);
            }
        }

        private async Task ExportTour()
        {
            if (isBusy) return;

            if (Tour is null) return;

            var selectedTourId = Tour.Id;
            var directoryPath = @"C:\";

            try
            {
                _exportManager.ExportTourById(selectedTourId, directoryPath);
            }
            catch (Exception ex)
            {
                throw new NullReferenceException("An error happend while exporting a tour: " + ex.Message);
            }
        }

        private async Task ImportTour()
        {
            if (isBusy) return;

            var window = new Views.ImportWindowView();
            var path = new ImportWindowViewModel(window)
            {
                CancelImportButtonCommand = new RelayCommand(CancelImportButton),
                ImportButtonCommand = new RelayCommand(ImportButton)
            }; ;

            if (window.ShowDialog() is not true) return;

            await BusyIndicatorFunc(async () =>
            {
                try
                {
                    string directoryPath = path.DirectoryPath;
                    string name = Path.GetFileName(directoryPath);
                    var importedTour = await _importManager.ImportTour(directoryPath);
                    if (importedTour != null)
                    {
                        AllTours.Add(importedTour);
                        UpdateShownTours();
                    }
                }
                catch (Exception ex)
                {
                    throw new NullReferenceException("An error happend while importing a tour: " + ex.Message);
                }
            });

            void ImportButton(object? obj)
            {
                window.DialogResult = true;
                window.Close();
            }

            void CancelImportButton(object? obj)
            {
                window.DialogResult = false;
                window.Close();
            }
        }

        private void DeleteTour(object? obj)
        {
            if (isBusy) return;

            var thisTour = Tour;
            if (thisTour is null) return;

            Tour = null;
            AllTours.Remove(thisTour);
            ShownTours.Remove(thisTour);
            _tourManager.DeleteTour(thisTour.Id);
        }

        private async Task ModifyTour()
        {
            if (isBusy) return;

            
            if(Tour is null) return;
            var thisTour = Tour;

            var window = new Views.TourManagerView();
            var tour = new TourManagerViewModel(window)
            {
                CancelButtonCommand = new RelayCommand(CancelButton),
                SaveButtonCommand = new RelayCommand(SaveButton)
            };

            if (window.ShowDialog() is not true) return;

            await BusyIndicatorFunc(async () =>
            {
                try
                {
                    AllTours.Remove(thisTour);
                    thisTour = await _tourManager.UpdateTour(tour.Name, tour.Description, tour.StartLocation, tour.EndLocation, _transportType, Tour);
                    Tour = null;
                    AllTours.Add(thisTour);
                    UpdateShownTours();
                }
                catch (Exception ex)
                {
                    throw new NullReferenceException("An error happend while updating a tour: " + ex.Message);
                }
            });

            void SaveButton(object? obj)
            {
                window.DialogResult = true;
                window.Close();
            }

            void CancelButton(object? obj)
            {
                window.DialogResult = false;
                window.Close();
            }
        }

        private async Task AddTour()
        {
            if(isBusy) return;

            var window = new Views.TourManagerView();
            var tour = new TourManagerViewModel(window)
            {
                CancelButtonCommand = new RelayCommand(CancelButton),
                SaveButtonCommand = new RelayCommand(SaveButton)
            };

            if (window.ShowDialog() is not true) return;

            await BusyIndicatorFunc(async () =>
            {
                try
                {
                    TransportType transportType = ConverStringToTransportType(tour.TransportType);
                    Tour? newTour = null;
                    newTour = await _tourManager.NewTour(tour.Name, tour.Description, tour.StartLocation, tour.EndLocation, transportType);
                    if(newTour != null)
                    {
                        AllTours.Add(newTour);
                        UpdateShownTours();
                    }
                }
                catch(Exception ex)
                {
                    throw new NullReferenceException("An error happend while creating a tour -> tour is null: " + ex.Message);
                }
            });

            void SaveButton(object? obj)
            {
                window.DialogResult = true;
                window.Close();
            }

            void CancelButton(object? obj)
            {
                window.DialogResult = false;
                window.Close();
            }
        }

        private void ShowTourDetails(object ?obj)
        {
            if (isBusy) return;

            if (Tour is null) return;

            var window = new Views.SingleTourView();
            var tour = new SingleTourViewModel(window)
            {
                GoBackButtonCommand = new RelayCommand(GoBackButton)
            };

            if (window.ShowDialog() is not true) return;

            void GoBackButton(object? obj)
            {
                window.DialogResult = false;
                window.Close();
            }
        }

        private void AddTourLog(object ?obj)
        {
            if (isBusy) return;

            var window = new Views.TourManagerView();
            var tour = new TourManagerViewModel(window)
            {
                CancelButtonCommand = new RelayCommand(CancelButton),
                SaveButtonCommand = new RelayCommand(SaveButton)
            };

            if (window.ShowDialog() is not true) return;

            
            //try
            //{
            //    TransportType transportType = ConverStringToTransportType(tour.TransportType);
            //    Tour? newTour = null;
            //    newTour = await _tourManager.newTour(tour.Name, tour.Description, tour.StartLocation, tour.EndLocation, transportType);
            //    if (newTour != null)
            //    {
            //        AllTours.Add(newTour);
            //        UpdateShownTours();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new NullReferenceException("An error happend while creating a tour -> tour is null: " + ex.Message);
            //}

            void SaveButton(object? obj)
            {
                window.DialogResult = true;
                window.Close();
            }

            void CancelButton(object? obj)
            {
                window.DialogResult = false;
                window.Close();
            }
        }

        private void ModifyTourLog(object? obj)
        {
            if (isBusy || Tour is null) return;

            // @TODO ModifyTourLog method
        }

        private void DeleteTourLog(object? obj)
        {
            if (isBusy || Tour is null) return;

            // @TODO DeleteTourLog method
        }
    }
}
