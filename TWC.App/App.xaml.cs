using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using TWC.App.Processors;
using TWC.Data;
using TWC.Data.Repositories;
using TWC.Data.Services;

namespace TWC.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);

            serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            string googleConfigPath = "credentials.json";

            services.AddDbContext<FileContext>(options =>
            {              
                options.UseSqlServer("Data Source=DESKTOP-JPLM5L8\\SQLEXPRESS;Initial Catalog=TelegramWpfCore;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            });
            services.AddSingleton(p => new GoogleDriveService(googleConfigPath));
            services.AddSingleton<MainWindow>();

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
