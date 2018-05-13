namespace Vivus.Client.Core.Animations
{
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media.Animation;

    /// <summary>
    /// Represents a collection of helpers to animate framework elements in specific ways.
    /// </summary>
    public static class FrameworkElementAnimations
    {
        /// <summary>
        /// Sides an element in from the right.
        /// </summary>
        /// <param name="element">The element to animate.</param>
        /// <param name="seconds"> The time the animation will take.</param>
        /// <param name="keepMargin">Whether to keep the element at the same width during animation.</param>
        /// <returns></returns>
        public static async Task SlideAndFadeInFromRight(this FrameworkElement element, float seconds = .3f, bool keepMargin = true)
        {

            // Create the storyboard
            var storyBoard = new Storyboard();

            // Add slide from right animation
            storyBoard.AddSlideFromRight(seconds, element.ActualWidth, keepMargin: keepMargin);

            // Add fade in animation
            storyBoard.AddFadeIn(seconds);

            // Start animation
            storyBoard.Begin(element);

            // Make page visible
            element.Visibility = Visibility.Visible;

            // Wait for it to finish
            await Task.Delay((int)(seconds * 1000));
        }

        /// <summary>
        /// Sides an element in from the right.
        /// </summary>
        /// <param name="element">The element to animate.</param>
        /// <param name="seconds"> The time the animation will take.</param>
        /// <param name="keepMargin">Whether to keep the element at the same width during animation.</param>
        /// <returns></returns>
        public static async Task SlideAndFadeInFromLeft(this FrameworkElement element, float seconds = .3f, bool keepMargin = true)
        {

            // Create the storyboard
            var storyBoard = new Storyboard();

            // Add slide from right animation
            storyBoard.AddSlideFromLeft(seconds, element.ActualWidth, keepMargin: keepMargin);

            // Add fade in animation
            storyBoard.AddFadeIn(seconds);

            // Start animation
            storyBoard.Begin(element);

            // Make page visible
            element.Visibility = Visibility.Visible;

            // Wait for it to finish
            await Task.Delay((int)(seconds * 1000));
        }

        /// <summary>
        /// Sides an element out to the left.
        /// </summary>
        /// <param name="element">The element to animate.</param>
        /// <param name="seconds"> The time the animation will take.</param>
        /// <param name="keepMargin">Whether to keep the element at the same width during animation.</param>
        /// <returns></returns>
        public static async Task SlideAndFadeOutToLeft(this FrameworkElement element, float seconds = .3f, bool keepMargin = true)
        {

            // Create the storyboard
            var storyBoard = new Storyboard();

            // Add slide from right animation
            storyBoard.AddSlideToLeft(seconds, element.ActualWidth, keepMargin: keepMargin);

            // Add fade in animation
            storyBoard.AddFadeOut(seconds);

            // Start animation
            storyBoard.Begin(element);

            // Make page visible
            element.Visibility = Visibility.Visible;

            // Wait for it to finish
            await Task.Delay((int)(seconds * 1000));
        }

        /// <summary>
        /// Sides an element out to the right.
        /// </summary>
        /// <param name="element">The element to animate.</param>
        /// <param name="seconds"> The time the animation will take.</param>
        /// <param name="keepMargin">Whether to keep the element at the same width during animation.</param>
        /// <returns></returns>
        public static async Task SlideAndFadeOutToRight(this FrameworkElement element, float seconds = .3f, bool keepMargin = true)
        {

            // Create the storyboard
            var storyBoard = new Storyboard();

            // Add slide from right animation
            storyBoard.AddSlideToRight(seconds, element.ActualWidth, keepMargin: keepMargin);

            // Add fade in animation
            storyBoard.AddFadeOut(seconds);

            // Start animation
            storyBoard.Begin(element);

            // Make page visible
            element.Visibility = Visibility.Visible;

            // Wait for it to finish
            await Task.Delay((int)(seconds * 1000));
        }
    }
}
