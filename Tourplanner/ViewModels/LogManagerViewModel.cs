using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tourplanner.Models;
using Tourplanner.Views;

namespace Tourplanner.ViewModels
{
    class LogManagerViewModel : BaseViewModel
    {
        private string _dateAndTime = String.Empty;
        private string _comment = String.Empty;
        private string _difficulty = String.Empty;
        private string _totalTime = String.Empty;
        private string _rating = String.Empty;

        public string DateAndTime
        {
            get => _dateAndTime;
            set { _dateAndTime = value; }
        }

        public string Comment
        {
            get => _comment;
            set { _comment = value; }
        }

        public string Difficulty
        {
            get => _difficulty;
            set { _difficulty = value; }
        }

        public string TotalTime
        {
            get => _totalTime;
            set { _totalTime = value; }
        }

        public string Rating
        {
            get => _rating;
            set { _rating = value; }
        }

        public ICommand LogCancelButtonCommand { get; init; }
        public ICommand LogSaveButtonCommand { get; init; }

        public LogManagerViewModel(Tour tour)
        {

        }

        public LogManagerViewModel(LogManagerView view)
            => view.DataContext = this;
    }
}
