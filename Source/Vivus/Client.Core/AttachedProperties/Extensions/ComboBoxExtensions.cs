namespace Vivus.Client.Core.AttachedProperties
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Represents a collection of extensions for the <see cref="ComboBox"/>.
    /// </summary>
    public static class ComboBoxExtensions
    {
        public static DependencyProperty IsDirtyEnabledProperty = DependencyProperty.RegisterAttached("IsDirtyEnabled",
             typeof(bool), typeof(ComboBoxExtensions), new PropertyMetadata(false, OnIsDirtyEnabledChanged));
        public static bool GetIsDirtyEnabled(ComboBox target) { return (bool)target.GetValue(IsDirtyEnabledProperty); }
        public static void SetIsDirtyEnabled(ComboBox target, bool value) { target.SetValue(IsDirtyEnabledProperty, value); }

        public static DependencyProperty ShowErrorTemplateProperty = DependencyProperty.RegisterAttached("ShowErrorTemplate",
                 typeof(bool), typeof(ComboBoxExtensions), new PropertyMetadata(false));
        public static bool GetShowErrorTemplate(ComboBox target) { return (bool)target.GetValue(ShowErrorTemplateProperty); }
        public static void SetShowErrorTemplate(ComboBox target, bool value) { target.SetValue(ShowErrorTemplateProperty, value); }

        private static void OnIsDirtyEnabledChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            ComboBox cb = (ComboBox)dependencyObject;

            if (cb != null)
            {
                cb.DropDownClosed += delegate
                {
                    if (!(bool)cb.GetValue(ShowErrorTemplateProperty))
                        cb.SetValue(ShowErrorTemplateProperty, true);
                };
            }
        }
    }
}
