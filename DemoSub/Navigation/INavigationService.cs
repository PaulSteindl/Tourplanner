using DemoSub.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tourplanner.Navigation
{
    internal enum NavigationMode
    {
        Modal,
        Modeless
    }

    internal interface INavigationService
    {
        bool? NavigateTo<TViewModel>(TViewModel viewModel, NavigationMode navigationMode = NavigationMode.Modal) where TViewModel : BaseViewModel;
        bool? NavigateTo<TViewModel>(NavigationMode navigationMode = NavigationMode.Modal) where TViewModel : BaseViewModel;
    }
}
