using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using OpenAI_API;
using QuickPromptGPT.Service;
using QuickPromptGPT.ViewModel;
using QuickPromptGPT.Windows;

namespace QuickPromptGPT
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainViewModel>();
            services.AddTransient<GPTChatWindow>();
            services.AddTransient<GPTChatWindowViewModel>();

            services.AddSingleton<IOpenAIAPI, OpenAIAPI>();
            services.AddSingleton<GlobalHookService>();
            services.AddSingleton<IGPTService, GPTService>();

        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetService<MainWindow>();
            var mainViewModel = _serviceProvider.GetService<MainViewModel>();
            mainWindow.DataContext = mainViewModel;
            mainWindow.Show();
        }
    }

}
