using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using UIL.ViewModels;

namespace UIL
{
    internal class Configuration
    {
       private readonly ServiceProvider _serviceProvider;
        public NavigationService NavigationService => _serviceProvider.GetRequiredService<NavigationService>();

        public Configuration()
        {
            var services = new ServiceCollection();

            services.AddSingleton<NavigationService, NavigationService>(s =>
            {
                // The navigation service needs special setup for the possible navigations,
                // so we have to provide our own factory method
                var navigationService = new NavigationService(s);
                navigationService.RegisterNavigation<MainViewModel, MainWindow>((viewModel, window) =>
                {
                    window.SearchBar.DataContext = viewModel._SearchViewModel;
                    window.ResultView.DataContext = viewModel._ResultViewModel;
                });
                return navigationService;
            });
            services.AddTransient<SearchViewModel>();
            services.AddTransient<ResultViewModel>();
            services.AddTransient<MainViewModel>();

            _serviceProvider = services.BuildServiceProvider();
        }
       
    }
}
