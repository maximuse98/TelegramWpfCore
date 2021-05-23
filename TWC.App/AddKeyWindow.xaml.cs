using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using TWC.App.Service;
using TWC.App.ViewModels;
using TWC.Data.Dtos;

namespace TWC.App
{
    /// <summary>
    /// Interaction logic for AddKeyWindow.xaml
    /// </summary>
    public partial class AddKeyWindow : Window
    {
        #region Properties
        public KeyDto Key { get; private set; }
        private FileViewModel SelectedFile;
        #endregion

        public AddKeyWindow(FileViewModel selectedFile)
        {
            InitializeComponent();

            this.SelectedFile = selectedFile;

            FileName.Content = selectedFile.Name;
        }

        public static readonly RoutedEvent AcceptEvent = EventManager.RegisterRoutedEvent("Accept", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(AddKeyWindow));

        public event RoutedEventHandler Accept
        {
            add { AddHandler(AddKeyWindow.AcceptEvent, value); }
            remove { RemoveHandler(AddKeyWindow.AcceptEvent, value); }
        }
        
        private void ValidateInput()
        {
            char[] inputKey = KeyInput.Text.Trim().ToCharArray();
            if (inputKey.Length != 23 || inputKey[5] != '-' || inputKey[11] != '-' || inputKey[17] != '-')
            {
                throw new Exception("Invalid input key format");
            }

            DateTime? inputDate = DateInput.SelectedDate;
            if (!inputDate.HasValue)
            {
                throw new Exception("Expiry date is empty");
            }
            if (inputDate.Value < DateTime.Now)
            {
                throw new Exception("Expiry date must be later than now");
            }

            string inputCount = CountInput.Text.Trim();
            if (inputCount.Length == 0 || !Int32.TryParse(inputCount, out int number) || number == 0)
            {
                throw new Exception("Invalid input count format");
            }
        }

        #region Event Handlers
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                ValidateInput();
            }
            catch(Exception ex)
            {
                ErrorPanel.Visibility = Visibility.Visible;
                ErrorOutput.Text = ex.Message;
                return;
            }

            this.Key = new KeyDto() 
            { 
                KeyValue = KeyInput.Text.Trim(),
                ExpiryDate = DateInput.SelectedDate.Value.AddHours(DateTime.Now.Hour),
                Count = Int32.Parse(CountInput.Text.Trim()),
                SourceId = SelectedFile.SourceId
            };

            //consumers of this event must provide their own custom handler(s)
            RaiseEvent(new RoutedEventArgs(AddKeyWindow.AcceptEvent, this));
        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            KeyInput.Text = KeyGenerator.Generate();
        }

        private void Input_Change(object sender, TextChangedEventArgs args)
        {
            ErrorPanel.Visibility = Visibility.Hidden;
        }

        private void DateInput_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ErrorPanel.Visibility = Visibility.Hidden;
        }
        #endregion
    }
}
