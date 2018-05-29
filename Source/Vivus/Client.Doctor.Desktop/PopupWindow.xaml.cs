namespace Vivus.Client.Doctor.Desktop
{
    using System;
    using System.Windows;
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

        private Point leftMouseButtonDownPosition;
        private BaseViewModel viewModel;

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
            leftMouseButtonDownPosition = Mouse.GetPosition(this);
        }

        /// <summary>
        /// Raised when the left mouse button was released.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void PopupWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point leftMouseButtonUpPosition = Mouse.GetPosition(this);

            // If the both mouse press and release were on the padding, close the window
            if (MouseClickOnPadding(leftMouseButtonUpPosition) && MouseClickOnPadding(leftMouseButtonDownPosition))
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

        #region Private Methods

        /// <summary>
        /// Checks whether the mouse click was on the container's padding or not.
        /// </summary>
        /// <param name="location">The mouse click location.</param>
        /// <returns></returns>
        private bool MouseClickOnPadding(Point location)
        {
            // If the click was on the left/right of the container, return true
            if (location.X >= 0 && location.X <= Width && (location.X < Container.Padding.Left || location.X > Width - Container.Padding.Right))
                return true;

            // If the click was on the top/bottom of the container, close the window
            if (location.Y >= 0 && location.Y <= Height && (location.Y < Container.Padding.Top || location.Y > Height - Container.Padding.Bottom))
                return true;

            // Return false otherwise
            return false;
        }

        #endregion
    }
}
