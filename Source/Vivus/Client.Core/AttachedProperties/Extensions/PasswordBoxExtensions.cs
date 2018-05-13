namespace Vivus.Client.Core.AttachedProperties
{
    using System.Windows;
    using System.Windows.Controls;

    public static class PasswordBoxExtensions
    {
        public static DependencyProperty IsDirtyEnabledProperty = DependencyProperty.RegisterAttached("IsDirtyEnabled",
             typeof(bool), typeof(PasswordBoxExtensions), new PropertyMetadata(false, OnIsDirtyEnabledChanged));
        public static bool GetIsDirtyEnabled(PasswordBox target) { return (bool)target.GetValue(IsDirtyEnabledProperty); }
        public static void SetIsDirtyEnabled(PasswordBox target, bool value) { target.SetValue(IsDirtyEnabledProperty, value); }

        public static DependencyProperty ShowErrorTemplateProperty = DependencyProperty.RegisterAttached("ShowErrorTemplate",
                 typeof(bool), typeof(PasswordBoxExtensions), new PropertyMetadata(false));
        public static bool GetShowErrorTemplate(PasswordBox target) { return (bool)target.GetValue(ShowErrorTemplateProperty); }
        public static void SetShowErrorTemplate(PasswordBox target, bool value) { target.SetValue(ShowErrorTemplateProperty, value); }

        private static void OnIsDirtyEnabledChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            PasswordBox pb = (PasswordBox)dependencyObject;

            if (pb != null)
                pb.LostFocus += delegate
                {
                    if (pb.GetBindingExpression(PasswordBox.CacheModeProperty) != null)
                    {
                        //Validation.ClearInvalid(pb.GetBindingExpression(PasswordBox.CacheModeProperty));
                        pb.GetBindingExpression(PasswordBox.CacheModeProperty).UpdateTarget();
                    }

                    if (!(bool)pb.GetValue(ShowErrorTemplateProperty))
                        pb.SetValue(ShowErrorTemplateProperty, true);
                };
        }
    }
}
