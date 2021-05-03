using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TWC.App.ViewModels;

namespace TWC.App
{
    /// <summary>
    /// Interaction logic for AddKeyWindow.xaml
    /// </summary>
    public partial class AddKeyWindow : Window
    {
        #region Properties
        public KeyViewModel key { get; private set; }
        public FileViewModel selectedFile { get; set; }
        #endregion

        public AddKeyWindow(FileViewModel selectedFile)
        {
            InitializeComponent();

            this.key = new KeyViewModel();
            this.selectedFile = selectedFile;

            FileName.Content = selectedFile.Name;
        }

        public static readonly RoutedEvent AcceptEvent = EventManager.RegisterRoutedEvent("Accept", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(AddKeyWindow));

        public event RoutedEventHandler Accept
        {
            add { AddHandler(AddKeyWindow.AcceptEvent, value); }
            remove { RemoveHandler(AddKeyWindow.AcceptEvent, value); }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            //consumers of this event must provide their own custom handler(s)
            RaiseEvent(new RoutedEventArgs(AddKeyWindow.AcceptEvent, this));
        }
    }
}
