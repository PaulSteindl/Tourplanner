using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Tourplanner.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {
        private readonly ResultViewModel resultViewModel;

        private string? _resultText;

        public string? ResultText
        {
            get { return _resultText; }
            set
            {
                _resultText = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel(SearchBarViewModel searchBarViewModel, ResultViewModel resultViewModel)
        {
            this.resultViewModel = resultViewModel;
            searchBarViewModel.SearchTextChanged += (_, searchText) =>
            {
                Search(searchText);
            };
        }

        private void Search(string? searchText)
        {
            
        }
    }
}
