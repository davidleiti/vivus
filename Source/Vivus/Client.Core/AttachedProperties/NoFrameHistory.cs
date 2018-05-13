namespace Vivus.Client.Core.AttachedProperties
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// The NoFrameHistory attached property for creating a <see cref="Frame"/> that never shows navigation and keeps the navigation history empty.
    /// </summary>
    public class NoFrameHistory : BaseAttachedProperty<NoFrameHistory, bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Frame frame;

            // Get the frame
            frame = (sender as Frame);

            // Hide the navigation bar
            frame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;

            // Clear history on navigate
            frame.Navigated += (ss, ee) => (ss as Frame)?.NavigationService.RemoveBackEntry();
        }
    }
}
