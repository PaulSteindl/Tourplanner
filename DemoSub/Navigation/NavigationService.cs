using Tourplanner.ViewModels;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Tourplanner.Navigation
{
    internal class NavigationService : INavigationService
    {
        private readonly Dictionary<Type, Func<BaseViewModel, Window>> viewMapping = new();
        private readonly IServiceProvider serviceProvider;

        public NavigationService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public bool? NavigateTo<TViewModel>(TViewModel viewModel, NavigationMode navigationMode = NavigationMode.Modal) where TViewModel : BaseViewModel
        {
            viewModel.NavigationService = this;

            var instantiator = viewMapping[typeof(TViewModel)];
            var window = instantiator(viewModel);

            bool? result = null;

            switch (navigationMode)
            {
                case NavigationMode.Modal:
                    result = window.ShowDialog();
                    break;
                case NavigationMode.Modeless:
                    window.Show();
                    break;
            }

            return result;
        }

        public bool? NavigateTo<TViewModel>(NavigationMode navigationMode = NavigationMode.Modal) where TViewModel : BaseViewModel
        {
            var viewModel = serviceProvider.GetRequiredService<TViewModel>();
            return NavigateTo(viewModel, navigationMode);
        }

        public void RegisterNavigation<TViewModel, TWindow>(Action<TViewModel, TWindow>? initializer = null)
            where TViewModel : BaseViewModel
            where TWindow : Window, new()
        {
            var instantiator = (BaseViewModel vm) =>
            {
                var viewModel = (TViewModel)vm;
                viewModel.NavigationService = this;

                var window = new TWindow
                {
                    DataContext = viewModel
                };

                initializer?.Invoke(viewModel, window);

                return window;
            };

            viewMapping[typeof(TViewModel)] = instantiator;
        }
    }
}
