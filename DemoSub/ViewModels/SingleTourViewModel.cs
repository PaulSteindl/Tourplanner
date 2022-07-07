using Tourplanner.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourplanner.Models;
using Tourplanner.Views;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Tourplanner.BusinessLayer;
using System.Windows;

namespace Tourplanner.ViewModels
{
    internal class SingleTourViewModel : BaseViewModel
    {
        // Busy
        private bool isBusy;
        // Log
        private Log _log;
        private ObservableCollection<Log> AllLogs = new ObservableCollection<Log>();
        public ObservableCollection<Log> ShownLogs { get; set; } = new ObservableCollection<Log>();
        private ILogManager _logManager;

        private string _name = String.Empty;
        private string _description = String.Empty;
        private string _startLocation = String.Empty;
        private string _endLocation = String.Empty;
        private string _transportType = String.Empty;
        private string _distance = String.Empty;
        private string _imagePath = String.Empty;

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


        //public SingleTourViewModel(Tour tour)
        //{

        //}

        public SingleTourViewModel(ILogManager logManager)
        {
            AddTourLogButtonCommand = new RelayCommand(AddTourLog);
            ModifyTourLogButtonCommand = new RelayCommand(ModifyTourLog);
            DeleteTourLogButtonCommand = new RelayCommand(DeleteTourLog);
            this._logManager = logManager;
        }
        public SingleTourViewModel(SingleTourView view)
            => view.DataContext = this;

        private void UpdateShownTours()
        {
            if (AllLogs is null) return;

            // Without clean, by adding a tour the previous tour will also be added again
            Application.Current.Dispatcher.Invoke(() => ShownLogs.Clear());

            foreach (var log in AllLogs)
            {
                Application.Current.Dispatcher.Invoke(() => ShownLogs.Add(log));
            }
        }

        private void DeleteTourLog(object? obj)
        {
            throw new NotImplementedException();
        }

        private void ModifyTourLog(object? obj)
        {
            throw new NotImplementedException();
        }

        private void AddTourLog(object? obj)
        {
            if(isBusy) return;

            var window = new Views.LogManagerView();
            var log = new LogManagerViewModel(window)
            {
                LogCancelButtonCommand = new RelayCommand(LogCancelButton),
                LogSaveButtonCommand = new RelayCommand(LogSaveButton)
            };

            if (window.ShowDialog() is not true) return;

            try
            {
                Log? newLog = null;
                //newLog = _logManager.CreateLog(log.Comment, log.);
                if (newLog != null)
                {
                    AllLogs.Add(newLog);
                    UpdateShownTours();
                }
            }
            catch (Exception ex)
            {
                throw new NullReferenceException("An error happend while creating a tour -> tour is null: " + ex.Message);
            }

            void LogSaveButton(object? obj)
            {
                window.DialogResult = true;
                window.Close();
            }

            void LogCancelButton(object? obj)
            {
                window.DialogResult = false;
                window.Close();
            }
        }
    }
}
