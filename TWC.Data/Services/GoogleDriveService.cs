using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace TWC.Data.Services
{
    public class GoogleDriveService
    {
        private readonly ILogger<GoogleDriveService> _logger;

        private static string[] Scopes = { DriveService.Scope.DriveReadonly };
        private static string ApplicationName = "Drive API .NET";

        private IConfiguration Configuration;

        private UserCredential Credential;
        private DriveService DriveService;

        private readonly IServiceScopeFactory ScopeFactory;

        public IList<Google.Apis.Drive.v3.Data.File> Files { get; private set; }

        public GoogleDriveService(ILogger<GoogleDriveService> logger, IConfiguration configuration, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;

            ScopeFactory = scopeFactory;
            Configuration = configuration;

            using (var stream = new FileStream(Configuration["GoogleDriveTokenPath"], FileMode.Open, FileAccess.Read))
            {
                string credPath = configuration["GoogleDriveOutputCredentialPath"];
                Credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;

                _logger.LogInformation("Google Credential file saved to {0}", credPath);
            }

            DriveService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = Credential,
                ApplicationName = ApplicationName
            });

            UploadFilesInfo();
        }

        public void UploadFilesInfo()
        {
            FilesResource.ListRequest filesRequest = DriveService.Files.List();
            filesRequest.PageSize = 10;
            filesRequest.Fields = "nextPageToken, files(id, name)";

            Files = filesRequest.Execute().Files;
        }

        public void DownloadByKey(string key)
        {
            using var scope = ScopeFactory.CreateScope();

            FileService fileService = scope.ServiceProvider.GetRequiredService<FileService>();
            string fileId = fileService.GetFileByKey(key).SourceId;
            DownloadFile(fileId);

            fileService.ReduceKeyCount(key);
        }

        public string GetFileNameByKey(string key)
        {
            using var scope = ScopeFactory.CreateScope();
            return scope.ServiceProvider.GetRequiredService<FileService>().GetFileByKey(key).Name;
        }

        public void DownloadFile(string fileId)
        {
            Google.Apis.Drive.v3.Data.File file = Files.Where(f => f.Id == fileId).FirstOrDefault();

            StreamWriter output = new StreamWriter(Configuration["DownloadPath"] + "/" + file.Name);
            DriveService.Files.Get(file.Id).Download(output.BaseStream);

            output.Close();
        } 
    }
}