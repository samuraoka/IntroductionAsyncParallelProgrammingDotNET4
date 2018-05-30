using System;
using System.Windows;

namespace AsianOptions
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            mnuFileSave.Click += mnuFileSave_Click;
            mnuFileExit.Click += mnuFileExit_Click;
            cmdPriceOption.Click += cmdPriceOption_Click;
        }

        //TODO

        private void cmdPriceOption_Click(object sender, RoutedEventArgs e)
        {
            //TODO
            throw new NotImplementedException();
        }

        private void mnuFileExit_Click(object sender, RoutedEventArgs e)
        {
            //TODO
            throw new NotImplementedException();
        }

        private void mnuFileSave_Click(object sender, RoutedEventArgs e)
        {
            //TODO
            throw new NotImplementedException();
        }
    }
}
