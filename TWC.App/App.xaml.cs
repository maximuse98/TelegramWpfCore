using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Windows;
using TWC.App.Service;
using TWC.Data;
using TWC.Data.Repositories;
using TWC.Data.Services;
using TWC.Data.Validators;

namespace TWC.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider serviceProvider;

        public App()
        {        
            ServiceCollection services = new ServiceCollection();
            
            ConfigureServices(services);

            serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            KeyEncryptor.Key = configuration["EncryptionKey"];

            services.AddDbContext<FileContext>(options =>
            {              
                options.UseSqlServer(configuration["DatabasePath"]);
            });

            services.AddLogging(configure => configure.AddConsole());

            services.AddSingleton<IConfiguration>(p => configuration);

            services.AddSingleton<GoogleDriveService>();
            services.AddSingleton<MainWindow>();

            services.AddSingleton<IValidator, Validator>();
            services.AddTransient<FileService>();
            services.AddTransient<FileRepository>();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }
    }
}
