namespace Vivus.Client.Donor.Desktop
{
    using System;
    using System.Windows;
    using System.Windows.Interop;
    using Vivus = Vivus.Core.Console;
    using Windows.Theme.Data;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IWindow
    {
        public MainWindow()
        {
            Vivus.Console.WriteLine("Loading application.");

            InitializeComponent();

            DataContext = new WindowViewModel();

            SourceInitialized += MainWindow_SourceInitialized;
        }

        /// <summary>
        /// This event is raised to support interoperation with Win32. See <see cref="HwndSource"/>.
        /// </summary>
        /// <param name="sender">The element that raised the event.</param>
        /// <param name="e">Arguments of the event.</param>
        private void MainWindow_SourceInitialized(object sender, EventArgs e)
        {
            IntPtr mWindowHandle;

            mWindowHandle = (new WindowInteropHelper(this)).Handle;
            HwndSource.FromHwnd(mWindowHandle).AddHook(new HwndSourceHook(WindowHelper.WindowProc));
        }
    }
}
