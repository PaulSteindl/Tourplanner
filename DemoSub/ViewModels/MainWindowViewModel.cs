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

namespace Tourplanner.ViewModels
{
    internal class MainWindowViewModel : BaseViewModel, ICloseWindow
    {
        private bool isBusy;
        // Suche
        private string _searchText = String.Empty;
        // Exit
        public Action? Close { get; set; }
        // Tour
        private Tour? _tour;
        private TourManager _tourManager;
        private TransportType _transportType;
        private ObservableCollection<Tour> AllTours = new ObservableCollection<Tour>();
        public ObservableCollection<Tour> ShownTours { get; set; } = new ObservableCollection<Tour>();
        public event EventHandler<Tour> TourChanged;
        // Log
        private Log _log;
        private ListCollectionView _logList;
        private ILogManager _logManager;
        
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

        public ListCollectionView LogList
        {
            get => _logList;
            set
            {
                _logList = value;
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
        public ICommand AddTourLogCommand  { get; }
        public ICommand ModifyTourLogCommand { get; }
        public ICommand DeleteTourLogCommand { get; }
        public ICommand ImportTourCommand { get; }
        public ICommand ExportTourCommand { get; }
        public ICommand TourReportCommand { get; }
        public ICommand SummaryTourReportCommand { get; }
        public ICommand SearchFieldCommand { get; }
        public ICommand ClearSearchFieldCommand { get; }
        public ICommand ExitApplicationCommand { get; }

        public MainWindowViewModel()
        {
            IsBusy = true;
            AddTourCommand = new AsyncCommand(AddTour);
            ModifyTourCommand = new AsyncCommand(ModifyTour);
            DeleteTourCommand = new RelayCommand(DeleteTour);
            AddTourLogCommand = new RelayCommand(AddTourLog);
            ModifyTourLogCommand = new RelayCommand(ModifyTourLog);
            DeleteTourLogCommand = new RelayCommand(DeleteTourLog);
            ImportTourCommand = new AsyncCommand(ImportTour);
            ExportTourCommand = new AsyncCommand(ExportTour);
            TourReportCommand = new AsyncCommand(TourReport);
            SummaryTourReportCommand = new AsyncCommand(SummaryTourReport);
            SearchFieldCommand = new RelayCommand(SearchField);
            ClearSearchFieldCommand = new RelayCommand(ClearSearchField);
            ExitApplicationCommand = new RelayCommand(ExitApplication);
            this._tourManager = tourManager;
        }

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

            foreach(var tour in AllTours)
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
            Close?.Invoke();
        }

        private void ClearSearchField(object? obj)
        {
            SearchText = String.Empty;
        }

        private void SearchField(object? obj)
        {
            throw new NotImplementedException();
        }

        private async Task SummaryTourReport()
        {
            throw new NotImplementedException();
        }

        private async Task TourReport()
        {
            throw new NotImplementedException();
        }

        private async Task ExportTour()
        {
            throw new NotImplementedException();
        }

        private async Task ImportTour()
        {
            throw new NotImplementedException();
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
                    _tourManager.UpdateTour(tour.Name, tour.Description, tour.StartLocation, tour.EndLocation, _transportType, Tour);
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
                    //Tour newTour = await new Tour { Id = new Guid(), Name = "NameTest", Description = "Desc", From = "Vienna", To = "Graz", Transporttype = _transportType };
                    
                    TransportType transportType = ConverStringToTransportType(tour.TransportType);
                    Tour? newTour = null;
                    newTour = await _tourManager.newTour(tour.Name, tour.Description, tour.StartLocation, tour.EndLocation, _transportType);
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

        private void AddTourLog(object ?obj)
        {
            if (isBusy || Tour is null) return;

            var window = new Views.SingleTourView();
            var tour = new SingleTourViewModel(window);

            //var newLog = _logManager.CreateLog();
            //ThisLog.AddNewItem(newLog);
            //ThisLog.CommitNew();
            //Log = newLog;
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
