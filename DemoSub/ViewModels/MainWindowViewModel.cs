using DemoSub.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tourplanner.Models;

namespace Tourplanner.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private string _searchText = String.Empty;
        private Tour? _tour;

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
            AddTourCommand = new RelayCommand(AddTour);
            ModifyTourCommand = new RelayCommand(ModifyTour);
            DeleteTourCommand = new RelayCommand(DeleteTour);
            AddTourLogCommand = new RelayCommand(AddTourLog);
            ModifyTourLogCommand = new RelayCommand(ModifyTourLog);
            DeleteTourLogCommand = new RelayCommand(DeleteTourLog);
            ImportTourCommand = new RelayCommand(ImportTour);
            ExportTourCommand = new RelayCommand(ExportTour);
            TourReportCommand = new RelayCommand(TourReport);
            SummaryTourReportCommand = new RelayCommand(SummaryTourReport);
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

        private void SummaryTourReport(object? obj)
        {
            throw new NotImplementedException();
        }

        private void TourReport(object? obj)
        {
            throw new NotImplementedException();
        }

        private void ExportTour(object? obj)
        {
            throw new NotImplementedException();
        }

        private void ImportTour(object? obj)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        private void DeleteTour(object? obj)
        {
            throw new NotImplementedException();
        }

        private void ModifyTour(object? obj)
        {
            throw new NotImplementedException();
        }

        private async Task AddTour()
        {
            var window = new Views.TourManagerView();
            var tour = new TourManagerViewModel(window)
            {
                CancelButtonCommand = new RelayCommand(CancelButton),
                SaveButtonCommand = new RelayCommand(SaveButton)
            };
        }

        private void SaveButton(object? obj)
        {
            throw new NotImplementedException();
        }

        private void CancelButton(object? obj)
        {
            throw new NotImplementedException();
        }
    }
}
