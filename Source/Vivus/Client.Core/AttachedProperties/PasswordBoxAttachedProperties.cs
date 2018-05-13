﻿namespace Vivus.Client.Core.AttachedProperties
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Represents the MonitorPassword attached property for a <see cref="PasswordBox"/>.
    /// </summary>
    public class MonitorPasswordProperty : BaseAttachedProperty<MonitorPasswordProperty, bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            // Get the caller
            PasswordBox passwordBox = sender as PasswordBox;

            // Make sure it is a PasswordBox
            if (passwordBox == null)
                return;

            // Remove any previous events
            passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;

            // If the caller set MonitorPassword to true...
            if ((bool)e.NewValue)
            {
                // Set default value
                HasTextProperty.SetValue(passwordBox);

                // Start listening out for password changes
                passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
            }
        }

        /// <summary>
        /// Fired when the password box password value changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // Set the attached HasText property
            HasTextProperty.SetValue(sender as PasswordBox);
        }
    }

    /// <summary>
    /// Represents the HasText attached property for a <see cref="PasswordBox"/>.
    /// </summary>
    public class HasTextProperty : BaseAttachedProperty<HasTextProperty, bool>
    {
        /// <summary>
        /// Sets the HasText property based on if the caller <see cref="PasswordBox"/> has any text.
        /// </summary>
        /// <param name="d"></param>
        public static void SetValue(DependencyObject d)
        {
            SetValue(d, (d as PasswordBox).SecurePassword.Length > 0);
        }
    }
}