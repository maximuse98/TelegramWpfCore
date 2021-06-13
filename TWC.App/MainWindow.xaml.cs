using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using TWC.App.Service;
using TWC.App.ViewModels;
using TWC.Data.Services;
using TWC.Data.Dtos;

namespace TWC.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly FileService fileService;
        private readonly GoogleDriveService fileStore;

        private readonly ApplicationViewModel context;

        public MainWindow(GoogleDriveService store, FileService fileService)
        {
            InitializeComponent();

            this.fileService = fileService;
            this.fileStore = store;
            this.context = new ApplicationViewModel(fileService);

            UpdateDbContext();
            DataContext = context;
        }

        private void UpdateDbContext()
        {
            IList<Google.Apis.Drive.v3.Data.File> googleFiles = fileStore.Files;
            List<FileDto> modelFilesDto = new List<FileDto>();

            foreach (var file in googleFiles)
            {
                modelFilesDto.Add(new FileDto {
                    SourceId = file.Id,
                    Name = file.Name
                });
            }

            fileService.AddOrUpdateFiles(modelFilesDto);
        }

        #region Event Handlers
        private void KeysGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                KeysGrid.Items.Refresh();
            }
            catch (InvalidOperationException)
            {

            }
            finally
            {
                fileService.SaveChanges();
            }
        }

        private void AddKey_Click(object sender, RoutedEventArgs e)
        {
            AddKeyWindow window = new AddKeyWindow(context.SelectedFile);

            window.AddHandler(AddKeyWindow.AcceptEvent, new RoutedEventHandler(AddKey));
            window.Show();
        }

        private void AddKey(object sender, RoutedEventArgs e)
        {
            AddKeyWindow requestWindow = sender as AddKeyWindow;

            //encrypt input key value before insert in db
            requestWindow.Key.KeyValue = KeyEncryptor.Encrypt(requestWindow.Key.KeyValue);

            fileService.AddKey(requestWindow.Key);
            requestWindow.Close();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ContentPanel.Visibility = Visibility.Visible;
        }
        #endregion
    }
}
