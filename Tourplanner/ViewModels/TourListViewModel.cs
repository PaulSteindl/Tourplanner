using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tourplanner.Models;

namespace Tourplanner.ViewModels
{
    class TourListViewModel : BaseViewModel, ICloseWindow
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

        // TOUR
        public Tour? _tour;
        public event EventHandler<Tour> SelectedTourChanged;
        public ObservableCollection<Tour> AllTours { get; } = new();

        public Tour? Tour
        {
            get => _tour;
            set
            {
                _tour = value;
                OnPropertyChanged();
                OnSelectedTourChanged();
            }
        }

        private void OnSelectedTourChanged()
        {
            SelectedTourChanged?.Invoke(this, Tour);
        }
    }
}
