namespace Vivus.Client.Doctor.Desktop
{
    using System.Globalization;
    using System.Threading;
    using System.Windows;
    using Vivus.Core.Doctor.IoC;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Loads the IoC and the window.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            // Let the base application do what is needed to be done
            base.OnStartup(e);

            // Setup the culture
            Thread.CurrentThread.CurrentCulture = new CultureInfo("ro-RO");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("ro-RO");

            // Setup the IoC
            IoCContainer.Setup();

            // Show the main window
            Current.MainWindow = new MainWindow();
            Current.MainWindow.Show();
        }
    }
}
