namespace Vivus.Client.Core.Animations
{
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media.Animation;
    using Vivus.Client.Core.Pages;

    /// <summary>
    /// Represents a collection of helpers to animate page in specific ways.
    /// </summary>
    public static class PageAnimations
    {
        /// <summary>
        /// Sides a page in from the right.
        /// </summary>
        /// <param name="page">Page to animate.</param>
        /// <param name="seconds"> The time the animation will take.</param>
        /// <returns></returns>
        public static async Task SlideAndFadeInFromRight(this BasePage page, float seconds)
        {

            // Create the storyboard
            var storyBoard = new Storyboard();
            
            // Add slide from right animation
            storyBoard.AddSlideFromRight(seconds, page.ActualWidth + 210);

            // Add fade in animation
            storyBoard.AddFadeIn(seconds);

            // Start animation
            storyBoard.Begin(page);

            // Make page visible
            page.Visibility = Visibility.Visible;

            // Wait for it to finish
            await Task.Delay((int)(seconds * 1000));
        }

        /// <summary>
        /// Sides a page out to the left.
        /// </summary>
        /// <param name="page">Page to animate.</param>
        /// <param name="seconds"> The time the animation will take.</param>
        /// <returns></returns>
        public static async Task SlideAndFadeOutToLeft(this BasePage page, float seconds)
        {

            // Create the storyboard
            var storyBoard = new Storyboard();

            // Add slide from right animation
            storyBoard.AddSlideToLeft(seconds, page.ActualWidth + 210);

            // Add fade in animation
            storyBoard.AddFadeOut(seconds);

            // Start animation
            storyBoard.Begin(page);

            // Make page visible
            page.Visibility = Visibility.Visible;

            // Wait for it to finish
            await Task.Delay((int)(seconds * 1000));
        }
    }
}
