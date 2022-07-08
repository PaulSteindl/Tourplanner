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

        // LOG
        private Log? _log;
        public ObservableCollection<Log> AllLogs { get; } = new();
        private ITourLogManager _logManager;

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
        private DateTime _dateAndTime;
        private string _comment = String.Empty;
        private DifficultyEnum _difficulty;
        private int _totalTime;
        private PopularityEnum _rating;

        public DateTime DateAndTime
        {
            get { return _dateAndTime; }
            set
            {
                _dateAndTime = value;
                OnPropertyChanged();
            }
        }
        public int TotalTime
        {
            get { return _totalTime; }
            set
            {
                _totalTime = value;
                OnPropertyChanged();
            }
        }
        public DifficultyEnum SelectedDiffEnumType
        {
            get { return _difficulty; }
            set
            {
                _difficulty = value;
                OnPropertyChanged("SelectedDiffEnumType");
            }
        }

        public IEnumerable<DifficultyEnum> MyDiffTypeValues
        {
            get
            {
                return Enum.GetValues(typeof(DifficultyEnum))
                    .Cast<DifficultyEnum>();
            }
        }

        public PopularityEnum SelectedRatEnumType
        {
            get { return _rating; }
            set
            {
                _rating = value;
                OnPropertyChanged("SelectedRatEnumType");
            }
        }

        public IEnumerable<PopularityEnum> MyRatTypeValues
        {
            get
            {
                return Enum.GetValues(typeof(PopularityEnum))
                    .Cast<PopularityEnum>();
            }
        }

        public string Comment
        {
            get => _comment;
            set { _comment = value; }
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

        public Log Log
        {
            get => _log;
            set
            {
                _log = value;
            }
        }

        public ICommand LogSaveButtonCommand { get; init; }
        public ICommand ModifyTourLogButtonCommand { get; init; }
        public ICommand DeleteTourLogButtonCommand { get; init; }

        public TourInformationViewModel(ITourLogManager logManager)
        {
            this._logManager = logManager;


            LogSaveButtonCommand = new RelayCommand((_) =>
            {
                try
                {
                    var log = _logManager.CreateLog(Comment, _totalTime, _dateAndTime, _difficulty, _rating, Tour.Id);
                    AllLogs.Add(log);

                }
                catch (Exception ex)
                {
                    throw new NullReferenceException("An error happend while creating a tour log: " + ex.Message);
                }
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
