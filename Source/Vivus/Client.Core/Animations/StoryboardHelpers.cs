namespace Vivus.Client.Core.Animations
{
    using System;
    using System.Windows;
    using System.Windows.Media.Animation;

    /// <summary>
    /// Represents a collection of animation helpers for <see cref="Storyboard"/>.
    /// </summary>
    public static class StoryboardHelpers
    {
        /// <summary>
        /// Adds a slide from right animation to the storyboard.
        /// </summary>
        /// <param name="storyBoard">The storyboard to add the animation to.</param>
        /// <param name="seconds">The time the animation will take.</param>
        /// <param name="offset">The distance to the right to start from.</param>
        /// <param name="decelerationRatio">The rate of deceleration.</param>
        /// <param name="keepMargin">Whether to keep the element at the same width during animation.</param>
        public static void AddSlideFromRight(this Storyboard storyBoard, float seconds, double offset, float decelerationRatio = .9f, bool keepMargin = true)
        {
            // Create the margin animate from right
            var animation = new ThicknessAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = new Thickness(keepMargin ? offset : 0, 0, -offset, 0),
                To = new Thickness(0),
                DecelerationRatio = decelerationRatio
            };

            // Set the target property name
            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

            // Add this to the storyboard
            storyBoard.Children.Add(animation);
        }

        /// <summary>
        /// Adds a slide from left animation to the storyboard.
        /// </summary>
        /// <param name="storyBoard">The storyboard to add the animation to.</param>
        /// <param name="seconds">The time the animation will take.</param>
        /// <param name="offset">The distance to the left to start from.</param>
        /// <param name="decelerationRatio">The rate of deceleration.</param>
        /// <param name="keepMargin">Whether to keep the element at the same width during animation.</param>
        public static void AddSlideFromLeft(this Storyboard storyBoard, float seconds, double offset, float decelerationRatio = .9f, bool keepMargin = true)
        {
            // Create the margin animate from right
            var animation = new ThicknessAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = new Thickness(-offset, 0, keepMargin ? offset : 0, 0),
                To = new Thickness(0),
                DecelerationRatio = decelerationRatio
            };

            // Set the target property name
            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

            // Add this to the storyboard
            storyBoard.Children.Add(animation);
        }

        /// <summary>
        /// Adds a fade in animation to the storyboard.
        /// </summary>
        /// <param name="storyBoard">The storyboard to add the animation to.</param>
        /// <param name="seconds">The time the animation will take.</param>
        public static void AddFadeIn(this Storyboard storyBoard, float seconds)
        {
            // Create the margin animate from right
            var animation = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = 0,
                To = 1
            };

            // Set the target property name
            Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));

            // Add this to the storyboard
            storyBoard.Children.Add(animation);
        }

        /// <summary>
        /// Adds a slide to left animation to the storyboard.
        /// </summary>
        /// <param name="storyBoard">The storyboard to add the animation to.</param>
        /// <param name="seconds">The time the animation will take.</param>
        /// <param name="offset">The distance to the left to end at.</param>
        /// <param name="decelerationRatio">The rate of deceleration.</param>
        /// <param name="keepMargin">Whether to keep the element at the same width during animation.</param>
        public static void AddSlideToLeft(this Storyboard storyBoard, float seconds, double offset, float decelerationRatio = .9f, bool keepMargin = true)
        {
            // Create the margin animate from right
            var animation = new ThicknessAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = new Thickness(0),
                To = new Thickness(-offset, 0, keepMargin ? offset : 0, 0),
                DecelerationRatio = decelerationRatio
            };

            // Set the target property name
            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

            // Add this to the storyboard
            storyBoard.Children.Add(animation);
        }

        /// <summary>
        /// Adds a slide to right animation to the storyboard.
        /// </summary>
        /// <param name="storyBoard">The storyboard to add the animation to.</param>
        /// <param name="seconds">The time the animation will take.</param>
        /// <param name="offset">The distance to the right to end at.</param>
        /// <param name="decelerationRatio">The rate of deceleration.</param>
        /// <param name="keepMargin">Whether to keep the element at the same width during animation.</param>
        public static void AddSlideToRight(this Storyboard storyBoard, float seconds, double offset, float decelerationRatio = .9f, bool keepMargin = true)
        {
            // Create the margin animate from right
            var animation = new ThicknessAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = new Thickness(0),
                To = new Thickness(keepMargin ? offset : 0, 0, -offset, 0),
                DecelerationRatio = decelerationRatio
            };

            // Set the target property name
            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

            // Add this to the storyboard
            storyBoard.Children.Add(animation);
        }

        /// <summary>
        /// Adds a fade out animation to the storyboard.
        /// </summary>
        /// <param name="storyBoard">The storyboard to add the animation to.</param>
        /// <param name="seconds">The time the animation will take.</param>
        public static void AddFadeOut(this Storyboard storyBoard, float seconds)
        {
            // Create the margin animate from right
            var animation = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = 1,
                To = 0
            };

            // Set the target property name
            Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));

            // Add this to the storyboard
            storyBoard.Children.Add(animation);
        }
    }
}
