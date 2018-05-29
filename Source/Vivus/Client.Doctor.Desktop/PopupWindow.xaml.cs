namespace Vivus.Client.Doctor.Desktop
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Vivus.Client.Core.Pages;
    using Vivus.Core.DataModels;
    using Vivus.Core.ViewModels;
    using Windows.Theme.Data;

    /// <summary>
    /// Interaction logic for PopupWindow.xaml
    /// </summary>
    public partial class PopupWindow : Window, IPopup
    {
        #region Private Members
        
        private BaseViewModel viewModel;
        private object clickedObject;

        #endregion

        #region Consturctors

        /// <summary>
        /// Initializes a new instance of the <see cref="PopupWindow"/> class with the given value.
        /// </summary>
        /// <param name="windowViewModel">The window viewmodel.</param>
        public PopupWindow(Vivus.Core.Doctor.ViewModels.WindowViewModel windowViewModel)
        {
            InitializeComponent();

            // Take care of the flickering and delayed data loading
            Visibility = System.Windows.Visibility.Collapsed;
            Opacity = 0;
            Width = 0;
            Height = 0;

            MaxHeight = SystemParameters.WorkArea.Height;
            MaxWidth = SystemParameters.WorkArea.Width;

            DataContext = windowViewModel;

            PreviewKeyDown += PopupWindow_PreviewKeyDown;
            MouseLeftButtonDown += PopupWindow_MouseLeftButtonDown;
            MouseLeftButtonUp += PopupWindow_MouseLeftButtonUp;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the owner of that popup window.
        /// </summary>
        IWindow IPopup.Owner { get => (IWindow)Owner; set => Owner = (Window)value; }

        /// <summary>
        /// Gets or sets the current page on the popup.
        /// </summary>
        public IPage CurrentPage { get => (IPage)Frame.Content; set => Frame.Content = value; }

        /// <summary>
        /// Gets or sets the viewmodel of the current page.
        /// </summary>
        public BaseViewModel PageViewModel { get => (Frame.Content as BasePage).ViewModel; set => (Frame.Content as BasePage).ViewModel = value; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Opens a popup and returns only when the newly opened popup is closed.
        /// </summary>
        /// <param name="viewModel">The page viewmodel.</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns></returns>
        bool? IPopup.ShowDialog(BaseViewModel viewModel)
        {
            this.viewModel = viewModel;

            return ShowDialog();
        }

        #endregion

        #region Private Handlers

        /// <summary>
        /// Raised when a key was pressed.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void PopupWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // If escape was hit, close the window
            if (e.Key == Key.Escape)
                Close();

            // If ALT + F4 was hit, don't close the window
            if (e.Key == Key.System && e.SystemKey == Key.F4)
                e.Handled = true;
        }

        /// <summary>
        /// Raised when the left mouse button was pressed.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void PopupWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            clickedObject = e.OriginalSource;
        }

        /// <summary>
        /// Raised when the left mouse button was released.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void PopupWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // If the button up source is the same with the button down source and the sourse is the container, close
            if (e.OriginalSource is Border && e.OriginalSource == clickedObject && (e.OriginalSource as Border).Name == "Container")
                Close();
        }

        #endregion

        #region Protected Handlers
        
        /// <summary>
        /// Raised when the window is fully rendered.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            // Set the maximum height of the Frame to the maximum height of the content
            Frame.MaxHeight = (Frame.Content as Page).MaxHeight;

            // If the sent parameter is not null, change the page viewmodel
            if (viewModel != null)
            {
                viewModel.ParentPage = CurrentPage;
                PageViewModel = viewModel;
            }

            if (Owner.WindowState != WindowState.Maximized)
            {
                WindowState = Owner.WindowState;
                Left = Owner.Left;
                Top = Owner.Top;
                Width = Owner.Width;
                Height = Owner.Height;
            }
            else
            {
                Left = 0;
                Top = 0;
                WindowState = Owner.WindowState;
            }

            Opacity = 1;
            Visibility = System.Windows.Visibility.Visible;
        }

        #endregion
    }
}
