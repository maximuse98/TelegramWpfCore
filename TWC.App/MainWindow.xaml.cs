using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TWC.App.Context;
using TWC.App.Models;
using TWC.App.Processors;
using TWC.App.ViewModels;

namespace TWC.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FileContext db;

        public MainWindow(FileContext db, GoogleDriveService store)
        {
            InitializeComponent();

            this.db = db;

            db.Files.Load();
            db.Keys.Load();

            IList<Google.Apis.Drive.v3.Data.File> files = store.Files;
            List<GoogleFile> modelFiles = new List<GoogleFile>();

            foreach (var file in files)
            {
                modelFiles.Add(new GoogleFile(file));
            }
            db.UpdateFiles(modelFiles);

            List<Key> modelKeys = db.Keys.ToList();
            ApplicationViewModel<GoogleFile> appViewModel = new ApplicationViewModel<GoogleFile>(modelFiles, modelKeys);

            DataContext = appViewModel;
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
                db.SaveChanges();
            }
        }
    }
}
