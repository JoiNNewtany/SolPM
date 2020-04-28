/*
 * Originally written by Hirotada Kobayashi
 * Source: https://gist.github.com/pierre3/6840538
 */

using Microsoft.Xaml.Behaviors;
using SolPM.Core.Helpers;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Controls;

namespace SolPM.WPF.Behaviors
{
    public class SecurePasswordBindingBehavior : Behavior<PasswordBox>
    {
        public SecureString SecurePassword
        {
            get { return (SecureString)GetValue(SecurePasswordProperty); }
            set { SetValue(SecurePasswordProperty, value); }
        }

        public static readonly DependencyProperty SecurePasswordProperty =
            DependencyProperty.Register("SecurePassword",
            typeof(SecureString),
            typeof(SecurePasswordBindingBehavior),
            new PropertyMetadata(new SecureString(), SecurePasswordPropertyChanged));

        private static void SecurePasswordPropertyChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var behavior = d as SecurePasswordBindingBehavior;
            if (d == null)
            { return; }

            var passwordBox = behavior.AssociatedObject as PasswordBox;
            if (passwordBox == null)
            { return; }

            var newPassword = e.NewValue as SecureString;
            if (newPassword == null)
            { return; }

            var oldPassword = e.OldValue as SecureString;
            if (newPassword.Equals(oldPassword))
            { return; }

            var bstr = Marshal.SecureStringToBSTR(newPassword);
            try
            {
                passwordBox.SecurePassword.CopyFromBSTR(bstr, newPassword.Length);
            }
            finally
            {
                Marshal.ZeroFreeBSTR(bstr);
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PasswordChanged += PasswordBox_PasswordChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PasswordChanged -= PasswordBox_PasswordChanged;
            base.OnDetaching();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            SecurePassword = AssociatedObject.SecurePassword.Copy();
        }
    }
}