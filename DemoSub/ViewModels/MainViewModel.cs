using DemoSub.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DemoSub.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {
        private readonly ISearchEngine searchEngine;
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

        public MainViewModel(ISearchEngine searchEngine, SearchBarViewModel searchBarViewModel, ResultViewModel resultViewModel)
        {
            this.searchEngine = searchEngine;
            this.resultViewModel = resultViewModel;
            searchBarViewModel.SearchTextChanged += (_, searchText) =>
            {
                Search(searchText);
            };
        }

        private void Search(string? searchText)
        {
            var results = String.Join("\n", this.searchEngine.SearchFor(searchText));
            resultViewModel.DisplayResults(results);
        }
    }
}
