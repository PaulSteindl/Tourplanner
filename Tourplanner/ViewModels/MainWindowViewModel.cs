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
using static Tourplanner.ViewModels.TourManagerViewModel;
using Tourplanner.Shared;

namespace Tourplanner.ViewModels
{
    internal class MainWindowViewModel : BaseViewModel, ICloseWindow
    {
        // BUSY && EXIT
        private bool isBusy;
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

        private readonly ILogger _logger = LogingManager.GetLogger<MainWindowViewModel>();


        public event EventHandler<string?>? SearchTextChanged;

        private string? searchText;
        public string? SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                OnPropertyChanged();
            }
        }

        public Action? Close { get; set; }

        private readonly ITourManager _tourManager;
        private readonly IExportManager _exportManager;
        private readonly IReportManager _reportManager;
        private readonly ISearchManager _searchManager;

        // VIEW MODELS
        public TourManagerViewModel TourManagerViewModel { get; }
        public TourInformationViewModel TourInformationViewModel { get; }
        public TourListViewModel TourListViewModel { get; }

        // COMMANDS
        public ICommand AddTourCommand { get; }
        public ICommand ModifyTourCommand { get; }
        public ICommand DeleteTourCommand { get; }
        public ICommand ImportTourCommand { get; }
        public ICommand ExportTourCommand { get; }
        public ICommand TourReportCommand { get; }
        public ICommand SummaryTourReportCommand { get; }
        public ICommand SearchFieldCommand { get; }
        public ICommand ResetSearchFieldCommand { get; }
        public ICommand ExitApplicationCommand { get; }

        // CTOR
        public MainWindowViewModel(TourManagerViewModel tourManagerViewModel, TourInformationViewModel tourInformationViewModel, TourListViewModel tourListViewModel,
            ITourManager tourManager, IExportManager exportManager, IReportManager reportManager, ISearchManager searchManager)
        {
            this.TourManagerViewModel = tourManagerViewModel;
            this.TourInformationViewModel = tourInformationViewModel;
            this.TourListViewModel = tourListViewModel;
            this._tourManager = tourManager;
            this._exportManager = exportManager;
            this._reportManager = reportManager;
            this._searchManager = searchManager;

            _tourManager.LoadTours().Result.ToList().ForEach(j => TourListViewModel.AllTours.Add(j));
            TourListViewModel.AllTours.ToList().ForEach(j => j.Logs.ToList().ForEach(l => TourInformationViewModel.AllLogs.Add(l)));

            AddTourCommand = new RelayCommand((_) =>
            {
                TourManagerViewModel.IsEditingTour = false;
                NavigationService?.NavigateTo(TourManagerViewModel);
            });

            ModifyTourCommand = new RelayCommand((_) =>
            {
                TourManagerViewModel.IsEditingTour = true;
                TourManagerViewModel.Tour = TourListViewModel.Tour;
                NavigationService?.NavigateTo(TourManagerViewModel);
            });

            DeleteTourCommand = new RelayCommand((_) =>
            {
                if (TourListViewModel.Tour == null) return;

                try
                {
                    _tourManager.DeleteTour(TourListViewModel.Tour.Id);
                    TourListViewModel.AllTours.Remove(TourListViewModel.Tour);
                }
                catch (Exception ex)
                {
                    _logger.Error("An error happend while deleting a tour: " + ex.Message);
                }
            });

            ImportTourCommand = new RelayCommand((_) =>
            {
                NavigationService?.NavigateTo(TourManagerViewModel);
            });

            ExportTourCommand = new RelayCommand((_) =>
            {
                if (TourListViewModel.Tour == null) return;

                try
                {
                    var directoryPath = @"C:\";
                    _exportManager.ExportTourById(TourListViewModel.Tour.Id, directoryPath);
                }
                catch (Exception ex)
                {
                    _logger.Error("An error happend while exporting a tour: " + ex.Message);
                }
            });

            TourReportCommand = new RelayCommand((_) =>
            {
                if (TourListViewModel.Tour is null) return;

                try
                {
                    var directoryPath = @"C:\TourReport\";
                    var boolean = _reportManager.CreateTourReport(TourListViewModel.Tour, directoryPath);
                }
                catch (Exception ex)
                {
                    _logger.Error("An error happend while creating a tour report: " + ex.Message);
                }
            });

            SummaryTourReportCommand = new RelayCommand((_) =>
            {
                try
                {
                    var directoryPath = @"C:\SummarizeTourReport\";
                    var boolean = _reportManager.CreateSummarizeReport(directoryPath);
                }
                catch (Exception ex)
                {
                    _logger.Error("An error happend while creating a tour summarize report: " + ex.Message);
                }
            });

            SearchFieldCommand = new RelayCommand((_) =>
            {
                try
                {
                    var searchResult = _searchManager.FindMatchingTours(SearchText);
                    TourListViewModel.AllTours.Clear();
                    searchResult.ToList().ForEach(j => TourListViewModel.AllTours.Add(j));
                    SearchTextChanged?.Invoke(this, SearchText);
                }
                catch (Exception ex)
                {
                    _logger.Error("An error happend while searching through tours: " + ex.Message);
                }
            });

            ResetSearchFieldCommand = new RelayCommand((_) =>
            {
                try
                {
                    SearchText = "";
                    SearchTextChanged?.Invoke(this, SearchText);
                    TourListViewModel.AllTours.Clear();
                    _tourManager.LoadTours().Result.ToList().ForEach(j => TourListViewModel.AllTours.Add(j));
                }
                catch (Exception ex)
                {
                    _logger.Error("An error occured while resetting the search field: " + ex.Message);
                }
            });

            ExitApplicationCommand = new RelayCommand((_) =>
            {
                Close?.Invoke();
            });

            TourManagerViewModel.TourAdded += (_, _tour) =>
            {
                TourListViewModel.AllTours.Add(_tour);
            };
            TourManagerViewModel.TourUpdated += (_, change) =>
            {
                TourListViewModel.AllTours.Remove(change.OldTour);
                TourListViewModel.AllTours.Add(change.NewTour);
            };
            TourListViewModel.SelectedTourChanged += (_, _tour) =>
            {
                TourInformationViewModel.Tour = _tour;
            };
        }
    }
}
