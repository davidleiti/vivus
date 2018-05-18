namespace Vivus.Client.DCPersonnel.Desktop
{
    using System;
    using System.Windows;
    using System.Windows.Interop;
    using VivusConsole = Vivus.Core.Console.Console;
    using Windows.Theme.Data;
    using Vivus.Core.DCPersonnel.IoC;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IWindow
    {
        public MainWindow()
        {
            VivusConsole.WriteLine("Loading application.");

            InitializeComponent();

            DataContext = IoCContainer.Get<Vivus.Core.DCPersonnel.ViewModels.WindowViewModel>();

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
