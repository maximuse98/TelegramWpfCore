using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace TWC.App.Processors
{
    public class GoogleDriveService
    {
        private static string[] Scopes = { DriveService.Scope.DriveReadonly };
        private static string ApplicationName = "Drive API .NET";

        private UserCredential Credential;
        private DriveService DriveService;

        public IList<Google.Apis.Drive.v3.Data.File> Files { get; private set; }

        public GoogleDriveService(string settingsPath)
        {
            using (var stream = new FileStream(settingsPath, FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                Credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
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

        public void DownloadFile(string fileId)
        {
            Google.Apis.Drive.v3.Data.File file = Files.Where(f => f.Id == fileId).FirstOrDefault();

            StreamWriter output = new StreamWriter(file.Name);
            DriveService.Files.Get(file.Id).Download(output.BaseStream);
        }
    }
}
