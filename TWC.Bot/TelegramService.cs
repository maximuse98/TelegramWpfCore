using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using TWC.Data.Services;

namespace TWC.Bot.Service
{
    public class TelegramService : BackgroundService, IDisposable
    {
        private readonly ILogger<TelegramService> _logger;

        private readonly IConfiguration Configuration;
        private static ITelegramBotClient BotClient;

        private readonly GoogleDriveService GoogleDrive;

        public TelegramService(ILogger<TelegramService> logger, IConfiguration configuration, GoogleDriveService googleDrive)
        {
            _logger = logger;

            Configuration = configuration;
            GoogleDrive = googleDrive;
            BotClient = new TelegramBotClient(Configuration["ApiKey"]);

            BotClient.OnMessage += OnMessage;
            BotClient.StartReceiving();

            _logger.LogInformation("Starting listening Telegram bot with id {0} and name {1}",
                BotClient.BotId,
                BotClient.GetMeAsync().Result.Username.ToString());
        }

        private async void OnMessage(object sender, MessageEventArgs e)
        {
            Message message;

            string messageText = e.Message.Text.ToString();
            string fileName = GoogleDrive.GetFileNameByKey(messageText);
            GoogleDrive.DownloadByKey(messageText);

            using (FileStream fileStream = System.IO.File.Open(Configuration["DownloadPath"] + "/" + fileName, FileMode.Open))
            {
                message = await BotClient.SendDocumentAsync(
                    chatId: e.Message.Chat,
                    document: new InputOnlineFile(fileStream, fileName)
                );
            }

            _logger.LogInformation(
                $"{message.From.FirstName} sent file {fileName} " +
                $"to user {e.Message.From.Username} at {message.Date}. "
            );
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(100000, stoppingToken);
            }
        }

        public override void Dispose()
        {
            BotClient.StopReceiving();

            GC.SuppressFinalize(this);
        }
    }
}
