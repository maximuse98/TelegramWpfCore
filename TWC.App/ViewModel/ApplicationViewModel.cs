using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

using TWC.Data.Models;
using TWC.Data.Services;

namespace TWC.App.ViewModels
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        #region Properties
        private FileService fileService;
        private FileViewModel selectedFile;

        public ObservableCollection<FileViewModel> Files { get; set; }

        public FileViewModel SelectedFile
        {
            get { return selectedFile; }
            set
            {
                selectedFile = value;
                List<Key> keys = fileService.GetKeys()
                                        .Where(x => x.SourceId == selectedFile.SourceId)
                                        .ToList();
                selectedFile.SetKeys(keys);
                OnPropertyChanged("SelectedFile");
            }
        }
        #endregion

        public ApplicationViewModel(FileService fileService)
        {
            this.fileService = fileService;
            Files = new ObservableCollection<FileViewModel>();

            foreach (var file in fileService.GetFiles())
            {
                Files.Add(new FileViewModel(file));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
