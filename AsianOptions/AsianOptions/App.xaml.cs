using System.Windows;

namespace AsianOptions
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var app = new MainWindow(
                new ViewModel.MainWindowViewModel());
            app.Show();
        }
    }
}
