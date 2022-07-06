﻿using Tourplanner.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourplanner.Models;
using Tourplanner.Views;
using System.Windows.Input;

namespace Tourplanner.ViewModels
{
    class SingleTourViewModel : BaseViewModel
    {
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

        public ICommand GoBackButtonCommand { get; init; }
        public ICommand AddTourLogButtonCommand { get; init; }
        public ICommand ModifyTourLogButtonCommand { get; init; }
        public ICommand DeleteTourLogButtonCommand { get; init; }


        public SingleTourViewModel(Tour tour)
        {

        }

        public SingleTourViewModel(SingleTourView view)
            => view.DataContext = this;
    }
}
