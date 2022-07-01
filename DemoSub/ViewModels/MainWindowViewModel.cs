using DemoSub.ViewModels;
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
using DemoSub;
using System.ComponentModel;

namespace Tourplanner.ViewModels
{
    internal class MainWindowViewModel : BaseViewModel
    {
        private string _searchText = String.Empty;
        private bool isBusy;

        private Tour? _tour;
        private TourManager _tourManager;
        private TransportType _transportType;
        private IList<Tour> AllTours = new List<Tour>();

        public event EventHandler<Tour> TourChanged;

        public ObservableCollection<Tour> ShownTours { get; set; } = new ObservableCollection<Tour>();

        public string SearchText
        {
            get => _searchText;
            set => _searchText = value; 
        }
        public Tour? Tour
        {
            get => _tour;
            set
            {
                _tour = value;
                OnPropertyChanged();
                UpdateShownTours();
            }
        }

        public bool IsBusy
        {
            get => isBusy;
            set
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
        }

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
                System.Windows.Application.Current.Dispatcher.Invoke(() => ShownTours.Add(tour));
            }
        }

        private void ExitApplication(object? obj)
        {
            if(isBusy) return;

            
        }

        private void ClearSearchField(object? obj)
        {
            throw new NotImplementedException();
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
                    var newTour = await _tourManager.newTour(tour.Name, tour.Description, tour.StartLocation, tour.EndLocation, _transportType);
                    AllTours.Add(newTour);
                    UpdateShownTours();
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
            if (Tour is null) return;

            // @TODO AddTourLog method
        }

        private void ModifyTourLog(object? obj)
        {
            if (Tour is null) return;

            // @TODO ModifyTourLog method
        }

        private void DeleteTourLog(object? obj)
        {
            if (Tour is null) return;

            // @TODO DeleteTourLog method
        }
    }
}
