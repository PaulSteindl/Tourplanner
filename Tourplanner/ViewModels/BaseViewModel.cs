using System.ComponentModel;
using System.Runtime.CompilerServices;
using Tourplanner.Navigation;

namespace Tourplanner.ViewModels
{
    internal abstract class BaseViewModel : INotifyPropertyChanged
    {
        public INavigationService? NavigationService { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
