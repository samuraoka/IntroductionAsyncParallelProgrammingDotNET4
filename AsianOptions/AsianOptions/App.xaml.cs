using System.Globalization;
using System.Windows;

namespace AsianOptions
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            // How to change Application Culture in wpf?
            // https://stackoverflow.com/questions/24135675/how-to-change-application-culture-in-wpf
            var culture = new CultureInfo("en");
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var app = new MainWindow(
                new ViewModel.MainWindowViewModel());
            app.Show();
        }
    }
}
