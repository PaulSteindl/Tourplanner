using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tourplanner.BusinessLayer;
using Tourplanner.Models;
using Tourplanner.Views;

namespace Tourplanner.ViewModels
{
    class TourManagerViewModel : BaseViewModel, ICloseWindow
    {
        public class TourChangeEventArgs
        {
            public TourChangeEventArgs(Tour oldTour, Tour newTour)
            {
                OldTour = oldTour;
                NewTour = newTour;
            }

            public Tour OldTour { get; }
            public Tour NewTour { get; }
        }
        // BUSY && CLOSE
        bool isBusy;

        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                OnPropertyChanged();

                CommandManager.InvalidateRequerySuggested();
            }
        }

        public Action? Close { get; set; }

        // PARAMS
        private string _name = String.Empty;
        private string _description = String.Empty;
        private string _startLocation = String.Empty;
        private string _endLocation = String.Empty;
        private TransportType _transportType;
        private string _directoryPath = String.Empty;

        // TOUR
        private Tour? _tour;
        public EventHandler<TourChangeEventArgs> TourUpdated;
        public EventHandler<Tour> TourAdded;
        private readonly ITourManager _tourManager;

        // IMPORT
        private readonly IImportManager _importManager;

        public TransportType SelectedMyEnumType
        {
            get { return _transportType; }
            set
            {
                _transportType = value;
                OnPropertyChanged("SelectedMyEnumType");
            }
        }

        public IEnumerable<TransportType> MyEnumTypeValues
        {
            get
            {
                return Enum.GetValues(typeof(TransportType))
                    .Cast<TransportType>();
            }
        }

        private bool isEditingTour;

        public bool IsEditingTour
        {
            get { return isEditingTour; }
            set
            {
                isEditingTour = value;
                if (isEditingTour == false)
                {
                    Tour = new Tour();
                }
            }
        }


        public Tour? Tour
        {
            get => _tour;
            set
            {
                _tour = value;
                OnPropertyChanged();

                Name = _tour.Name;
                Description = _tour.Description;
                StartLocation = _tour.From;
                EndLocation = _tour.To;
                //DirectoryPath = _tour.PicPath;
                SelectedMyEnumType = _tour.Transporttype;
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

        public string DirectoryPath
        {
            get => _directoryPath;
            set
            {
                _directoryPath = value;
                OnPropertyChanged();
            }
        }

        public void OnTourAdded()
        {
            Close?.Invoke();
            TourAdded?.Invoke(this, Tour);
        }

        public void OnTourUpdated(Tour oldTour, Tour newTour)
        {
            Close?.Invoke();
            TourUpdated?.Invoke(this, new TourChangeEventArgs(oldTour, newTour));
        }

        //public ICommand CancelButtonCommand { get; init; }
        public ICommand SaveButtonCommand { get; init; }
        public ICommand CancelImportButtonCommand { get; init; }
        public ICommand ImportButtonCommand { get; init; }

        public TourManagerViewModel(ITourManager tourManager, IImportManager importManager)
        {
            this._tourManager = tourManager;
            this._importManager = importManager;

            SaveButtonCommand = new RelayCommand(async (_) =>
            {
                isBusy = true;

                if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Description) || string.IsNullOrEmpty(StartLocation) || string.IsNullOrEmpty(EndLocation))
                {
                    isBusy = false;
                    return;
                }

                if (!IsEditingTour)
                {
                    try
                    {
                        Tour? newTour = null;
                        newTour = await _tourManager.NewTour(Name, Description, StartLocation, EndLocation, _transportType);
                        if (newTour is not null)
                        {
                            Tour = newTour;
                            OnTourAdded();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new NullReferenceException("An error happend while creating a tour: " + ex.Message);
                    }
                }
                else
                {
                    Tour oldTour = Tour;
                    var thisTour = Tour;
                    try
                    {
                        thisTour = await _tourManager.UpdateTour(Name, Description, StartLocation, EndLocation, SelectedMyEnumType, thisTour);
                        OnTourUpdated(oldTour, thisTour);
                    }
                    catch (Exception ex)
                    {
                        throw new NullReferenceException("An error happend while updating a tour: " + ex.Message);
                    }
                }
                isBusy = false;
            }, (_) => isBusy == false);

            CancelImportButtonCommand = new RelayCommand((_) => Close?.Invoke());

            ImportButtonCommand = new RelayCommand(async (_) =>
            {
                isBusy = true;

                try
                {
                    string directoryPath = DirectoryPath;
                    string name = Path.GetFileName(directoryPath);
                    var importedTour = await _importManager.ImportTour(directoryPath);
                    if (importedTour != null)
                    {
                        Tour = importedTour;
                    }
                }
                catch (Exception ex)
                {
                    throw new NullReferenceException("An error happend while importing a tour: " + ex.Message);
                }
                isBusy = false;
            }, (_) => isBusy == false);
        }
    }
}
