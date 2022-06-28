using DemoSub.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tourplanner.Models;
using AsyncAwaitBestPractices.MVVM;


namespace Tourplanner.ViewModels
{
    class MainWindowViewModel : BaseViewModel
    {
        private string _searchText = String.Empty;
        private Tour? _tour;
        private bool isBusy;

        public string SearchText
        {
            get => _searchText;
            set => _searchText = value; 
        }
        public Tour? Tour
        {
            get => _tour;
            set => _tour = value;
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
            ModifyTourCommand = new RelayCommand(ModifyTour);
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

        private void ExitApplication(object? obj)
        {
            throw new NotImplementedException();
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
            if (Tour is null) return;
        }

        private void ModifyTour(object? obj)
        {
            if (Tour is null) return;
        }

        private async Task AddTour()
        {
            var window = new Views.TourManagerView();
            var tour = new TourManagerViewModel(window)
            {
                CancelButtonCommand = new RelayCommand(CancelButton),
                SaveButtonCommand = new RelayCommand(SaveButton)
            };

            if (window.ShowDialog() is not true) return;

            // @TODO finish implementing AddTour with MapQuestAPI

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
