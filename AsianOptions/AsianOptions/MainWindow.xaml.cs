using System;
using System.IO;
using System.Windows;

namespace AsianOptions
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //TODO create a view model for this view.
        public MainWindow()
        {
            InitializeComponent();

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
            cmdPriceOption.IsEnabled = false;

            //TODO spinner settings

            double initial = Convert.ToDouble(txtInitialPrice.Text);
            double exercise = Convert.ToDouble(txtExercisePrice.Text);
            double up = Convert.ToDouble(txtUpGrowth.Text);
            double down = Convert.ToDouble(txtDownGrowth.Text);
            double interest = Convert.ToDouble(txtInterestRate.Text);
            long periods = Convert.ToInt64(txtPeriods.Text);
            long sims = Convert.ToInt64(txtSimulations.Text);

            //
            // Run simulation to price options:
            //
            var rand = new Random(Guid.NewGuid().GetHashCode());
            int start = Environment.TickCount;

            double price = AsianOptionsPricing.Simulation(
                rand, initial, exercise, up, down, interest, periods, sims);

            int stop = Environment.TickCount;

            double elapsedTimeInSecs = (stop - start) / 1000.0;

            var result = $"{price:C2} [{elapsedTimeInSecs:#,##0.00} secs]";

            //
            // Display the results:
            //
            lstPrices.Items.Insert(0, result);

            //TODO spinner settings

            cmdPriceOption.IsEnabled = true;
        }
    }
}
