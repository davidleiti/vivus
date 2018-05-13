namespace Vivus.Client.Core.Pages
{
    using Vivus.Client.Core.Animations;
    using Vivus.Core.ViewModels;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Linq;

    /// <summary>
    /// Represetns a base page for all pages to gain base functionality.
    /// </summary>
    public class BasePage : Page
    {
        #region Public Properties

        /// <summary>
        /// The animation to play when the page is first loaded.
        /// </summary>
        public PageAnimation PageLoadAnimation { get; set; } //= PageAnimation.SlideAndFadeInFromRight;

        /// <summary>
        /// The animation to play when the page is unloaded.
        /// </summary>
        public PageAnimation PageUnloadAnimation { get; set; } //= PageAnimation.SlideAndFadeOutToLeft;

        /// <summary>
        /// The time any slide animation takes to complete.
        /// </summary>
        public float SlideSeconds { get; set; } = .8f;

        /// <summary>
        /// Gets or sets the flag that indicates if this page should animate out on load.
        /// Useful for when we are moving the page to another frame.
        /// </summary>
        public bool ShouldAnimateOut { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BasePage"/> class with the default values.
        /// </summary>
        public BasePage()
        {
            // If we are animation in, hide to begin with
            if (PageLoadAnimation != PageAnimation.None)
                Visibility = Visibility.Collapsed;

            // Listen out for the page loading
            Loaded += BasePage_Loaded;
        }

        #endregion

        #region Animation Load / Unload

        /// <summary>
        /// Once the page is loaded, perfrom any required animation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BasePage_Loaded(object sender, RoutedEventArgs e)
        {
            // If we are setup to animate out on load
            if (ShouldAnimateOut)
                // Animate the page out
                await AnimateOut();
            else
                // Animate the page in
                await AnimateIn();
        }

        /// <summary>
        /// Animates the current page in.
        /// </summary>
        /// <returns></returns>
        public async Task AnimateIn()
        {
            // Make sure there is something to do
            if (PageLoadAnimation == PageAnimation.None)
                return;

            switch (PageLoadAnimation)
            {
                case PageAnimation.SlideAndFadeInFromRight:

                    // Start the animation
                    await this.SlideAndFadeInFromRight(SlideSeconds);

                    break;
            }
        }

        /// <summary>
        /// Animates the current page out.
        /// </summary>
        /// <returns></returns>
        public async Task AnimateOut()
        {
            // Make sure there is something to do
            if (PageUnloadAnimation == PageAnimation.None)
                return;

            switch (PageUnloadAnimation)
            {
                case PageAnimation.SlideAndFadeOutToLeft:

                    // Start the animation
                    await this.SlideAndFadeOutToLeft(SlideSeconds);

                    break;
            }
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Handles a PreviewKeyDown event for a <see cref="TextBox"/> that should contain a text value, but spaces.
        /// </summary>
        /// <param name="sender">The caller of the event handler.</param>
        /// <param name="e">Arguments of the event.</param>
        public void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
                return;
            }
        }

        /// <summary>
        /// Handles a PreviewKeyDown event for a <see cref="TextBox"/> that should contain digits.
        /// </summary>
        /// <param name="sender">The caller of the event handler.</param>
        /// <param name="e">Arguments of the event.</param>
        public void DigitsTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
                return;
            }
        }

        /// <summary>
        /// Handles a PreviewTextInput event for a <see cref="TextBox"/> that should contain an integer value.
        /// </summary>
        /// <param name="sender">The caller of the event handler.</param>
        /// <param name="e">Arguments of the event.</param>
        public void DigitsTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox tb;
            bool handled;

            tb = (TextBox)sender;
            handled = true;

            // If the key is a digit
            if (e.Text.Length > 0 && char.IsDigit(e.Text[0]))
                // Don't handle
                handled = false;

            e.Handled = handled;
        }

        /// <summary>
        /// Handles a PreviewKeyDown event for a <see cref="TextBox"/> that should contain a numerical value.
        /// </summary>
        /// <param name="sender">The caller of the event handler.</param>
        /// <param name="e">Arguments of the event.</param>
        public void IntegerTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
                return;
            }
        }

        /// <summary>
        /// Handles a PreviewTextInput event for a <see cref="TextBox"/> that should contain an integer value.
        /// </summary>
        /// <param name="sender">The caller of the event handler.</param>
        /// <param name="e">Arguments of the event.</param>
        public void IntegerTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox tb;
            bool handled;

            tb = (TextBox)sender;
            handled = true;

            // If the first key is a zero, handle
            if (tb.Text.Length > 0 && tb.Text[0] == '0' && tb.SelectionLength == 0)
            {
                e.Handled = true;
                return;
            }

            // If the key is a digit
            if (e.Text.Length > 0 && char.IsDigit(e.Text[0]))
                // Don't handle
                handled = false;

            e.Handled = handled;
        }

        ///// <summary>
        ///// Handles a PreviewTextInput event for a <see cref="TextBox"/> that should contain a decimal value.
        ///// </summary>
        ///// <param name="sender">The caller of the event handler.</param>
        ///// <param name="e">Arguments of the event.</param>
        //public void DecimalTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        //{
        //    TextBox tb;
        //    bool handled;

        //    tb = (TextBox)sender;
        //    handled = true;

        //    // If the user tries to delete the comma, but not the 0 at the beggining of the number, handle
        //    if (tb.Text.Length > 0 && tb.Text[0] == '0' && tb.SelectionStart > 0 && tb.SelectedText.IndexOf(',') > -1)
        //    {
        //        e.Handled = true;
        //        return;
        //    }
            
        //    // If the first key is a zero, handle
        //    if (tb.Text.Length > 0 && tb.Text[0] == '0'  && tb.SelectionStart > 0 && tb.SelectedText.IndexOf(',') == -1 && tb.SelectionLength == 0)
        //    {
        //        e.Handled = true;
        //        return;
        //    }

        //    // If the key is a digit
        //    if (e.Text.Length > 0 && char.IsDigit(e.Text[0]))
        //        // Don't handle
        //        handled = false;

        //    // If the comma is not present yet and there is at least 1 digit and the selection is not at the beggining of the number
        //    if (tb.Text.Length > 0 && tb.SelectionStart > 0 && e.Text == "," && !tb.Text.Contains(","))
        //        // Don't handle
        //        handled = false;

        //    e.Handled = handled;
        //}

        /// <summary>
        /// Handles a PreviewKeyDown event for a <see cref="TextBox"/> that should contain a date value.
        /// </summary>
        /// <param name="sender">The caller of the event handler.</param>
        /// <param name="e">Arguments of the event.</param>
        public void DateTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
                return;
            }

            TextBox tb = (TextBox)sender;

            // If the user tries to delete (by either backspae or delete) a selection that contains a slash and there are still digits on the right of the selection, handle
            if ((e.Key == Key.Back || e.Key == Key.Delete) && tb.SelectionLength > 0 && tb.SelectionStart + tb.SelectionLength < tb.Text.Length && tb.SelectedText.IndexOf('/') > -1)
            {
                e.Handled = true;
                return;
            }

            if (e.Key == Key.Back && tb.Text.Length > 0 && tb.SelectionStart > 0)
                // If the user tries to delete (by backspace) a slash while there are still digits on the right of the slash, handle
                if (tb.Text[tb.SelectionStart - 1] == '/' && tb.SelectionLength == 0 && tb.Text.Length > tb.SelectionStart)
                {
                    e.Handled = true;
                    return;
                }

            if (e.Key == Key.Delete && tb.SelectionStart < tb.Text.Length)
                // If the user tries to delete (by delete) a slash while there are still digits on the right of the slash, handle
                if (tb.Text[tb.SelectionStart] == '/' && tb.SelectionLength == 0 && tb.SelectionStart < tb.Text.Length - 1)
                {
                    e.Handled = true;
                    return;
                }
        }

        /// <summary>
        /// Handles a PreviewTextInput event for a <see cref="TextBox"/> that should contain a date value.
        /// </summary>
        /// <param name="sender">The caller of the event handler.</param>
        /// <param name="e">Arguments of the event.</param>
        public void DateTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox tb;
            bool handled;
            int slashesCount;
            string[] tbSplits;

            handled = true;
            tb = (TextBox)sender;
            slashesCount = tb.Text.Count(c => c == '/');
            tbSplits = tb.Text.Split('/');

            // If the key is a digit
            if (e.Text.Length > 0 && char.IsDigit(e.Text[0]))
            {
                // Don't handle
                handled = false;

                // If there are already 2 digits and no slashes, handle
                if (slashesCount == 0 && tb.Text.Length == 2)
                {
                    e.Handled = true;
                    return;
                }

                // the user tries to add more than 2 digits to the first part (day) of the date, handle
                if (slashesCount > 0 && tb.SelectionStart <= tb.Text.IndexOf('/') && tb.SelectionLength == 0 && tbSplits[0].Length == 2)
                {
                    e.Handled = true;
                    return;
                }

                // If the user tries to add more than 2 digits to the second part (month) of the date, handle
                if (slashesCount > 0 && tb.SelectionStart > 1 && tb.SelectionStart <= tbSplits[0].Length + tbSplits[1].Length + 1 && tb.SelectionLength == 0 && tbSplits[1].Length == 2)
                {
                    e.Handled = true;
                    return;
                }

                // If the user tries to add more than 4 digits to the third part (year), handle
                if (slashesCount == 2 && tbSplits[2].Length == 4 && tb.SelectionStart > tbSplits[0].Length + tbSplits[1].Length + 1 && tb.SelectionStart <= tb.Text.Length && tb.SelectionLength == 0)
                {
                    e.Handled = true;
                    return;
                }
            }

            // If the user tries to add one more slash and there are not yet 2 slashes and the character before the insert position is not a slash, don't handle
            if (e.Text == "/" && tb.Text.Length > 0 && tb.Text[tb.SelectionStart - 1] != '/' && slashesCount < 2)
                // Don't handle
                handled = false;

            e.Handled = handled;
        }

        #endregion
    }

    /// <summary>
    /// Represetns a base page for all pages, containing a view model, to gain base functionality.
    /// </summary>
    public class BasePage<VM> : BasePage
        where VM : BaseViewModel, new()
    {
        private VM viewModel;

        #region Public Properties

        /// <summary>
        /// The View Model associated with this page.
        /// </summary>
        public VM ViewModel
        {
            get => viewModel;

            set
            {
                // If nothing changed, return
                if (viewModel == value)
                    return;

                // Update the value
                viewModel = value;

                // Set the data context for this page
                DataContext = viewModel;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BasePage"/> class with the default values.
        /// </summary>
        public BasePage() : base()
        {
            // Create a default view model
            ViewModel = new VM();
        }

        #endregion
    }
}
