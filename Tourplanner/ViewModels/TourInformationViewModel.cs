using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Tourplanner.BusinessLayer;
using Tourplanner.Models;
using Tourplanner.Shared;

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
        public ObservableCollection<Log> AllLogs { get; private set; } = new();
        public event EventHandler<Log> SelectedLogChanged;
        private readonly ITourLogManager _logManager;

        private readonly ILogger _logger = LogingManager.GetLogger<TourInformationViewModel>();

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

        private bool isEditingLog;

        public bool IsEditingLog
        {
            get { return isEditingLog; }
            set
            {
                isEditingLog = value;
                if (isEditingLog == false)
                {
                    //Log = newLog;
                }
            }
        }

        public string Comment
        {
            get => _comment;
            set 
            { 
                _comment = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => _name;
            set 
            { 
                _name = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get => _description;
            set 
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        public string StartLocation
        {
            get => _startLocation;
            set 
            {
                _startLocation = value;
                OnPropertyChanged();
            }
        }

        public string EndLocation
        {
            get => _endLocation;
            set
            { 
                _endLocation = value;
                OnPropertyChanged();
            }
        }

        public string TransportType
        {
            get => _transportType;
            set 
            { 
                _transportType = value;
                OnPropertyChanged();
            }
        }

        public string Distance
        {
            get => _distance;
            set 
            { 
                _distance = value;
                OnPropertyChanged();
            }
        }

        public string ImagePath
        {
            get => _imagePath;
            set 
            { 
                _imagePath = value;
                OnPropertyChanged();
            }
        }

        public Tour? Tour
        {
            get => _tour;
            set
            {
                _tour = value;
                OnTourChanged();
                OnPropertyChanged();
            }
        }

        public Log Log
        {
            get => _log;
            set
            {
                _log = value;
                OnPropertyChanged();
                OnSelectedLogChanged();
                if (Log is not null)
                {
                    Comment = _log.Comment;
                    DateAndTime = _log.Date;
                    TotalTime = _log.TotalTime;
                    SelectedRatEnumType = _log.Rating;
                    SelectedDiffEnumType = _log.Difficulty;
                }
                else
                {
                    Log = new Log();
                }
            }
        }

        private void OnSelectedLogChanged()
        {
            SelectedLogChanged?.Invoke(this, Log);
        }

        private void OnTourChanged()
        {
            var getAllLogsFromTour = _logManager.GetAllLogsByTourId(Tour.Id);
            AllLogs.Clear();
            getAllLogsFromTour.ToList().ForEach(l => AllLogs.Add(l));
            if (Log is not null)
            {
                Log = null;
            }
        }

        public ICommand LogSaveButtonCommand { get; init; }
        public ICommand UpdateButtonCommand { get; init; }
        public ICommand DeleteLogButtonCommand { get; init; }
        public ICommand ResetLogButtonCommand { get; init; }

        public TourInformationViewModel(ITourLogManager logManager)
        {
            this._logManager = logManager;

            LogSaveButtonCommand = new RelayCommand((_) =>
            {
                IsEditingLog = false;
                if (!IsEditingLog && Tour is not null)
                {
                    try
                    {
                        var log = _logManager.CreateLog(Comment, _totalTime, DateAndTime, _difficulty, _rating, Tour.Id).Result;
                        if (log is not null)
                        {
                            AllLogs.Add(log);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("An error happend while creating a tour log: " + ex.Message);
                    }
                }
            });

            UpdateButtonCommand = new RelayCommand((_) =>
            {
                if (Log is null) return;

                IsEditingLog = true;
                if (IsEditingLog)
                {
                    Log oldLog = Log;
                    var thisLog = Log;
                    try
                    {
                        thisLog = _logManager.UpdateLog(Comment, _totalTime, DateAndTime, _difficulty, _rating, thisLog).Result;
                        if(thisLog is not null)
                        {
                            AllLogs.Remove(oldLog);
                            AllLogs.Add(thisLog);
                        }
                    }
                    catch(Exception ex)
                    {
                        _logger.Error("An error happend while updating a tour log: " + ex.Message);
                    }
                }
            });

            DeleteLogButtonCommand = new RelayCommand((_) =>
            {
                if (Log is null) return;

                try
                {
                    _logManager.DeleteLog(Log.Id);
                    AllLogs.Remove(Log);
                }
                catch (Exception ex)
                {
                    _logger.Error("An error happend while deleting a tour log: " + ex.Message);
                }
            });

            ResetLogButtonCommand = new RelayCommand((_) =>
            {
                if(Log is null) return;
                else
                {
                    Comment = "";
                    DateAndTime = DateTime.Now;
                    TotalTime = 0;
                    SelectedRatEnumType = PopularityEnum.Perfect;
                    SelectedDiffEnumType = DifficultyEnum.Hard;
                }
            });
        }
    }
}
