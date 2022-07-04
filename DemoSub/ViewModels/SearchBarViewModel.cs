using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Tourplanner.ViewModels
{
    internal class SearchBarViewModel : BaseViewModel
    {
        public event EventHandler<string?>? SearchTextChanged;

        private string? _searchText;
        public string? SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
            }
        }

        public ICommand SearchCommand { get; }

        public SearchBarViewModel()
        {
            SearchCommand = new RelayCommand((_) =>
            {
                SearchTextChanged?.Invoke(this, SearchText);
            });
        }
    }
}
