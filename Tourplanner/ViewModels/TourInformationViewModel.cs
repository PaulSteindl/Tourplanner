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
        public class LogChangeEventArgs
        {
            public LogChangeEventArgs(Log oldLog, Log newLog)
            {
                OldLog = oldLog;
                NewLog = newLog;
            }

            public Log OldLog { get; }
            public Log NewLog { get; }
        }

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
        public event EventHandler<Log> SelectedLogChanged;
        public EventHandler<LogChangeEventArgs> LogUpdated;
        public EventHandler<Log> LogAdded;
        private readonly ITourLogManager _logManager;

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
                    Log = new Log();
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
                OnPropertyChanged();

                //Comment = _log.Comment;
                //DateAndTime = _log.Date;
                //TotalTime = _log.TotalTime;
                //SelectedRatEnumType = _log.Rating;
                //SelectedDiffEnumType = _log.Difficulty;
            }
        }

        public void OnLogAdded()
        {
            Close?.Invoke();
            LogAdded?.Invoke(this, Log);
        }

        public void OnLogUpdated(Log oldLog, Log newLog)
        {
            Close?.Invoke();
            LogUpdated?.Invoke(this, new LogChangeEventArgs(oldLog, newLog));
        }

        private void OnSelectedLogChanged()
        {
            SelectedLogChanged?.Invoke(this, Log);
        }

        public ICommand LogSaveButtonCommand { get; init; }
        public ICommand ModifyTourLogButtonCommand { get; init; }
        public ICommand DeleteTourLogButtonCommand { get; init; }

        public TourInformationViewModel(ITourLogManager logManager)
        {
            this._logManager = logManager;

            LogSaveButtonCommand = new RelayCommand((_) =>
            {
                IsEditingLog = false;
                if (!IsEditingLog)
                {
                    try
                    {
                        var log = _logManager.CreateLog(Tour.Id, Comment, _difficulty, _totalTime, _rating);
                        AllLogs.Add(log);
                        OnLogAdded();
                    }
                    catch (Exception ex)
                    {
                        throw new NullReferenceException("An error happend while creating a tour log: " + ex.Message);
                    }
                }
            });

            ModifyTourLogButtonCommand = new RelayCommand((_) =>
            {
                //IsEditingLog = true;
                //if (IsEditingLog)
                //{
                //    try
                //    {
                //        var log = _logManager.CreateLog(Comment, _totalTime, _dateAndTime, _difficulty, _rating, Tour.Id);
                //        AllLogs.Add(log);
                //    }
                //    catch (Exception ex)
                //    {
                //        throw new NullReferenceException("An error happend while creating a tour log: " + ex.Message);
                //    }
                //}
            });

            DeleteTourLogButtonCommand = new RelayCommand((_) =>
            {

            });
        }
    }
}
