namespace Vivus.Client.Core.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using Vivus.Client.Core.Pages;

    /// <summary>
    /// Interaction logic for PageHostControl.xaml
    /// </summary>
    public partial class PageHostControl : UserControl
    {
        #region Dependency Properties

        /// <summary>
        /// Gets or sets the current page to show in the page host.
        /// </summary>
        public BasePage CurrentPage
        {
            get => (BasePage)GetValue(CurrentPageProperty);

            set => SetValue(CurrentPageProperty, value);
        }

        /// <summary>
        /// Registers the <see cref="CurrentPage"/> as a dependency property.
        /// </summary>
        public static readonly DependencyProperty CurrentPageProperty = DependencyProperty.Register(nameof(CurrentPage), typeof(BasePage), typeof(PageHostControl), new UIPropertyMetadata(CurrentPagePropertyChanged));

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PageHostControl"/> class with the default values.
        /// </summary>
        public PageHostControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Raised when the <see cref="CurrentPage"/> value changes.
        /// </summary>
        /// <param name="d">The current <see cref="PageHostControl"/>.</param>
        /// <param name="e">The event arguments.</param>
        private static void CurrentPagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Get the frames
            var newPageFrame = (d as PageHostControl).NewPage;
            var oldPageFrame = (d as PageHostControl).OldPage;

            // Store the current page content as the old page
            var oldPageContent = newPageFrame.Content;

            // Remove the current page from the new page frame
            newPageFrame.Content = null;

            // Move the previous page into the old page frame
            oldPageFrame.Content = oldPageContent;

            // Animate out the previous page when the Loaded event fires right after this call due to moving frames
            if (oldPageContent is BasePage oldPage)
                oldPage.ShouldAnimateOut = true;

            // Set the new page content
            newPageFrame.Content = e.NewValue;
        }

        #endregion
    }
}
