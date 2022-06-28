using DemoSub.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tourplanner.Views;

namespace Tourplanner.ViewModels
{
    class TourManagerViewModel : BaseViewModel
    {
        private string _name = String.Empty;
        private string _description = String.Empty;
        private string _startLocation = String.Empty;
        private string _endLocation = String.Empty;
        private string _transportType = String.Empty;

        public string Name
        {
            get => _name;
            set {  _name = value; }
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

        public ICommand CancelButtonCommand { get; init; }
        public ICommand SaveButtonCommand { get; init; }

        public TourManagerViewModel() { }

        public TourManagerViewModel(TourManagerView view)
            => view.DataContext = this;
    }
}
