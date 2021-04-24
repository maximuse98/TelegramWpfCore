using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TWC.App.Models;

namespace TWC.App.ViewModels
{
    public class FileViewModel<T> : INotifyPropertyChanged where T : File
    {
        private T _file;

        public string SourceId
        {
            get { return _file.SourceId; }
            set
            {
                _file.SourceId = value;
                OnPropertyChanged("Id");
            }
        }
        public string Name
        {
            get { return _file.Name; }
            set
            {
                _file.Name = value;
                OnPropertyChanged("Name");
            }
        }
        public ObservableCollection<KeyViewModel> Keys { get; set; }

        public FileViewModel(T file, List<Key> keys)
        {
            _file = file;
            Keys = new ObservableCollection<KeyViewModel>();

            foreach (var key in keys)
            {
                Keys.Add(new KeyViewModel(key));
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