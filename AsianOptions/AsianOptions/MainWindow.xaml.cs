using AsianOptions.ViewModel;
using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace AsianOptions
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _viewModel;

        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
            DataContext = _viewModel;

            mnuFileSave.Click += mnuFileSave_Click;
            mnuFileExit.Click += mnuFileExit_Click;
            cmdPriceOption.Click += cmdPriceOption_Click;
        }

        /// <summary>
        /// Exit the app.
        /// </summary>
        private void mnuFileExit_Click(object sender, RoutedEventArgs e)
        {
            // trigger "closed" event as if user had hit "X" on window:
            Close();
        }

        /// <summary>
        /// Saves the contents of the list box.
        /// </summary>
        private void mnuFileSave_Click(object sender, RoutedEventArgs e)
        {
            using (var file = new StreamWriter("results.txt"))
            {
                foreach (var item in lstPrices.Items)
                {
                    file.WriteLine(item);
                }
            }
        }

        /// <summary>
        /// Main button to run the simulation.
        /// </summary>
        private void cmdPriceOption_Click(object sender, RoutedEventArgs e)
        {
            //TODO create a command for this action.
            if (_viewModel.TaskCounter == 0)
            {
                spinnerWait.Visibility = Visibility.Visible;
                spinnerWait.Spin = true;
            }
            _viewModel.TaskCounter += 1;

            var param = new AsianOptionsPricing.Parameter(_viewModel);
            string result = string.Empty;
            Task.Factory.StartNew(() =>
            {
                //
                // Run simulation to price options:
                //
                var rand = new Random(Guid.NewGuid().GetHashCode());
                int start = Environment.TickCount;

                double price = AsianOptionsPricing.Simulation(rand, param);

                int stop = Environment.TickCount;

                double elapsedTimeInSecs = (stop - start) / 1000.0;

                // Standard Numeric Format Strings
                // https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings#example
                var ci = new CultureInfo("en");
                result = $"{price.ToString("C", ci)} [{elapsedTimeInSecs:#,##0.00} secs]";
            }).ContinueWith((antecedent) =>
            {
                //
                // Display the results:
                //
                _viewModel.Results.Insert(0, result);

                _viewModel.TaskCounter -= 1;
                if (_viewModel.TaskCounter == 0)
                {
                    spinnerWait.Spin = false;
                    spinnerWait.Visibility = Visibility.Collapsed;
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
