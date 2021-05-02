using System;
using System.Collections.Generic;
using System.Windows;
using TWC.App.Processors;
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
        private GoogleDriveService fileStore;

        public MainWindow(GoogleDriveService store, FileService fileService)
        {
            InitializeComponent();

            this.fileService = fileService;
            this.fileStore = store;

            UpdateDbContext();
            DataContext = new ApplicationViewModel(fileService);
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
    }
}
