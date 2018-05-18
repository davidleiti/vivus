namespace Vivus.Client.Core.AttachedProperties
{
    using Client.Core.Animations;
    using System.Windows;
    using VivusVisibility = Vivus.Core.DataModels.Visibility;

    /// <summary>
    /// Represents a base class to run any animation method when a boolean is set to visible
    /// and a reverse animation when set to hidden.
    /// </summary>
    /// <typeparam name="Parent"></typeparam>
    public abstract class AnimateBaseProperty<Parent> : BaseAttachedProperty<Parent, VivusVisibility>
        where Parent : BaseAttachedProperty<Parent, VivusVisibility>, new()
    {
        #region Public Properties

        public bool FirstLoad { get; set; } = true;

        #endregion

        public override void OnValueUpdated(DependencyObject sender, object value)
        {
            // If the sender is not a framework element, return
            if (!(sender is FrameworkElement element))
                return;

            // If the value was not changed and this is not the first run, return
            if (sender.GetValue(ValueProperty) == value && !FirstLoad)
                return;

            if (FirstLoad)
            {
                // Create single self-unhookable event for the element's Loaded event
                RoutedEventHandler onLoaded = null;
                onLoaded = (s, e) =>
                {
                    element.Loaded -= onLoaded;

                    // Executes the desired animation
                    DoAnimation(element, (VivusVisibility)value);

                    // Make sure you're not anymore in the first load
                    FirstLoad = false;
                };

                // Hook into the Loaded event of the element
                element.Loaded += onLoaded;
            }
            else
                // Executes the desired animation
                DoAnimation(element, (VivusVisibility)value);
        }

        /// <summary>
        /// The animation method that is fired when the value changes.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="visibility">The new value.</param>
        protected virtual void DoAnimation(FrameworkElement element, VivusVisibility value)
        {

        }
    }

    /// <summary>
    /// Animates a framework element sliding it in from the left on show and sliding out to the left on hide
    /// </summary>
    public class AnimateSlideInFromLeftProperty : AnimateBaseProperty<AnimateSlideInFromLeftProperty>
    {
        protected override async void DoAnimation(FrameworkElement element, VivusVisibility value)
        {
            if (value == VivusVisibility.Visible)
                // Animate in
                await element.SlideAndFadeInFromLeft(FirstLoad ? 0 : .3f, false);
            else
                // Animate out
                await element.SlideAndFadeOutToLeft(FirstLoad ? 0 : .3f, false);
        }
    }
}
