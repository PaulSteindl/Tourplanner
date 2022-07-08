using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tourplanner.BusinessLayer;
using Tourplanner.Models;

namespace Tourplanner.ViewModels
{
    class TourInformationViewModel : BaseViewModel, ICloseWindow
    {
        // BUSY && CLOSE
        private bool isBusy;
        public Action? Close { get; set; }

        // LOG
        private Log? _log;
        private ObservableCollection<Log> AllLogs = new();
        private ILogManager _logManager;

        // PARAMS TOUR
        private string _name = String.Empty;
        private string _description = String.Empty;
        private string _startLocation = String.Empty;
        private string _endLocation = String.Empty;
        private string _transportType = String.Empty;
        private string _distance = String.Empty;
        private string _imagePath = String.Empty;
        private Tour? _tour;

        // PARAMS LOG
        private string _dateAndTime = String.Empty;
        private string _comment = String.Empty;
        private string _difficulty = String.Empty;
        private string _totalTime = String.Empty;
        private string _rating = String.Empty;

        public string DateAndTime
        {
            get => _dateAndTime;
            set { _dateAndTime = value; }
        }

        public string Comment
        {
            get => _comment;
            set { _comment = value; }
        }

        public string Difficulty
        {
            get => _difficulty;
            set { _difficulty = value; }
        }

        public string TotalTime
        {
            get => _totalTime;
            set { _totalTime = value; }
        }

        public string Rating
        {
            get => _rating;
            set { _rating = value; }
        }

        public Tour? Tour
        {
            get => _tour;
            set
            {
                _tour = value;
                OnPropertyChanged();
                //UpdateShownTours();
            }
        }
        public string Name
        {
            get => _name;
            set { _name = value; }
        }

        public string Description
        {
            get => _description;
            set { _description = value; }
        }

        public string StartLocation
        {
            get => _startLocation;
            set { _startLocation = value; }
        }

        public string EndLocation
        {
            get => _endLocation;
            set { _endLocation = value; }
        }

        public string TransportType
        {
            get => _transportType;
            set { _transportType = value; }
        }

        public string Distance
        {
            get => _distance;
            set { _distance = value; }
        }

        public string ImagePath
        {
            get => _imagePath;
            set { _imagePath = value; }
        }

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

        public ICommand GoBackButtonCommand { get; init; }
        public ICommand AddTourLogButtonCommand { get; init; }
        public ICommand ModifyTourLogButtonCommand { get; init; }
        public ICommand DeleteTourLogButtonCommand { get; init; }

        public TourInformationViewModel(ILogManager logManager)
        {
            this._logManager = logManager;

            GoBackButtonCommand = new RelayCommand((_) => Close?.Invoke());

            AddTourLogButtonCommand = new RelayCommand((_) =>
            {

            });

            ModifyTourLogButtonCommand = new RelayCommand((_) =>
            {

            });

            DeleteTourLogButtonCommand = new RelayCommand((_) =>
            {

            });
        }
    }
}
