using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TWC.Bot.Service;
using TWC.Data;
using TWC.Data.Repositories;
using TWC.Data.Services;

namespace TWC.Bot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var config = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", optional: false)
                        .Build();

                    services.AddDbContext<FileContext>(options =>
                    {
                        options.UseSqlServer(config.GetSection("DatabasePath").Value.ToString());
                    });

                    services.AddHostedService<TelegramService>();

                    services.AddScoped<FileRepository>();
                    services.AddScoped<FileService>();

                    services.AddSingleton<GoogleDriveService>();
                });
    }
}
