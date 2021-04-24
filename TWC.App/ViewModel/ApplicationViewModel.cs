using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TWC.App.Models;

namespace TWC.App.ViewModels
{
    public class ApplicationViewModel<T> : INotifyPropertyChanged where T : File
    {
        private FileViewModel<T> selectedFile;

        public ObservableCollection<FileViewModel<T>> Files { get; set; }

        public FileViewModel<T> SelectedFile
        {
            get { return selectedFile; }
            set
            {
                selectedFile = value;
                OnPropertyChanged("SelectedFile");
            }
        }

        public ApplicationViewModel(List<T> files, List<Key> keys)
        {
            Files = new ObservableCollection<FileViewModel<T>>();

            foreach (var file in files)
            {
                var fileKeys = keys.FindAll(x => x.SourceId == file.SourceId);
                Files.Add(new FileViewModel<T>(file, fileKeys));
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
