using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Tourplanner.Views;

namespace Tourplanner.ViewModels
{
    class ImportWindowViewModel : BaseViewModel
    {
        private string _directoryPath = String.Empty;

        public ICommand CancelImportButtonCommand { get; init; }
        public ICommand ImportButtonCommand { get; init; }

        public string DirectoryPath
        {
            get => _directoryPath;
            set { _directoryPath = value; }
        }

        public ImportWindowViewModel(ImportWindowView view)
            => view.DataContext = this;
    }
}
