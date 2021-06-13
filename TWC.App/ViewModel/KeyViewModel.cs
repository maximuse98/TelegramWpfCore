using System;
using TWC.Data.Models;
using TWC.App.Service;

namespace TWC.App.ViewModels
{
    public class KeyViewModel
    {
        #region Properties
        private Key _key;

        public string Value 
        { 
            get { return KeyEncryptor.Decrypt(_key.KeyValue); }
        }
        public DateTime ExpiryDate
        { 
            get { return _key.ExpiryDate; } 
            set 
            { 
                _key.ExpiryDate = value;
                UpdateKeyStatus();
            } 
        }
        public int Count
        {
            get { return _key.Count; }
            set
            {
                _key.Count = value;
                UpdateKeyStatus();
            }
        }
        public bool IsDeactivated
        {
            get { return _key.IsDeactivated; }
            set
            {
                _key.IsDeactivated = value;
                UpdateKeyStatus();
            }
        }

        public string Status { get; private set; }
        #endregion

        public KeyViewModel(Key key)
        {
            _key = key;

            UpdateKeyStatus();
        }

        #region Commands
        private RelayCommand removeCommand;
        public RelayCommand RemoveCommand
        {
            get
            {
                return removeCommand ??
                    (removeCommand = new RelayCommand(obj =>
                    {
                        IsDeactivated = true;
                    })
                );
            }
        }
        #endregion

        public void UpdateKeyStatus()
        {
            if (DateTime.Now > ExpiryDate || Count <= 0)
            {               
                Status = "Expired";
            }
            else if (IsDeactivated)
            {
                Status = "Removed";
            }
            else
            {
                Status = "Active";
            }
        }
    }
}
