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

        public Action? Close { get; set; }

        private readonly ITourManager _tourManager;
        private readonly IExportManager _exportManager;
        private readonly IReportManager _reportManager;

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
            ITourManager tourManager, IExportManager exportManager, IReportManager reportManager)
        {
            this.TourManagerViewModel = tourManagerViewModel;
            this.TourInformationViewModel = tourInformationViewModel;
            this.TourListViewModel = tourListViewModel;
            this._tourManager = tourManager;
            this._exportManager = exportManager;
            this._reportManager = reportManager;

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
                    throw new NullReferenceException("An error happend while deleting a tour: " + ex.Message);
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
                    throw new NullReferenceException("An error happend while exporting a tour: " + ex.Message);
                }
            });

            TourReportCommand = new RelayCommand((_) =>
            {
                if (TourListViewModel.Tour is null) return;

                try
                {
                    var directoryPath = @"C:\TourReport\";
                    // @TODO: CHECK IF DIR EXISTS ??
                    var boolean = _reportManager.CreateTourReport(TourListViewModel.Tour, directoryPath);
                }
                catch (Exception ex)
                {
                    throw new NullReferenceException("An error happend while creating a tour report: " + ex.Message);
                }
            });

            SummaryTourReportCommand = new RelayCommand((_) =>
            {
                try
                {
                    var directoryPath = @"C:\SummarizeTourReport\";
                    // @TODO: CHECK IF DIR EXISTS ??
                    var boolean = _reportManager.CreateSummarizeReport(directoryPath);
                }
                catch (Exception ex)
                {
                    throw new NullReferenceException("An error happend while creating a tour summarize report: " + ex.Message);
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
