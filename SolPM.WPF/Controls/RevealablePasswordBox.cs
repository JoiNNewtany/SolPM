using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace SolPM.WPF.Controls
{
    public class RevealablePasswordBox : Control
    {
        static RevealablePasswordBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RevealablePasswordBox),
                new FrameworkPropertyMetadata(typeof(RevealablePasswordBox)));
        }

        public RevealablePasswordBox()
        {
            CommandManager.AddPreviewExecutedHandler(this, PreviewExecutedHandler);
            _maskTimer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 1) };
            _maskTimer.Tick += (sender, args) => MaskAllDisplayText();
            SecurePassword = new SecureString();
        }

        #region Properties

        /// <summary>
        /// Dependency property to hold watermark for RevealablePasswordBox.
        /// </summary>
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("SecurePassword", typeof(SecureString),
                typeof(RevealablePasswordBox), new UIPropertyMetadata(new SecureString(),
                    new PropertyChangedCallback(OnPasswordChanged)));

        /// <summary>
        /// Gets or sets dependency Property implementation for SecurePassword.
        /// </summary>
        public SecureString SecurePassword
        {
            get
            {
                return (SecureString)GetValue(PasswordProperty);
            }

            set
            {
                SetValue(PasswordProperty, value);
                if (null != PwdContainer)
                {
                    PwdContainer.Text = new string('*', SecurePassword.Length);
                }
            }
        }

        /// <summary>
        /// Private member holding mask visibile timer.
        /// </summary>
        private readonly DispatcherTimer _maskTimer;

        private Button revealButton;

        private Button RevealButton
        {
            get
            {
                return revealButton;
            }

            set
            {
                if (revealButton != null)
                {
                    revealButton.PreviewMouseDown -= OnRevealButtonMouseDown;
                        //new MouseButtonEventHandler(OnRevealButtonMouseDown);
                    revealButton.PreviewMouseUp -= OnRevealButtonMouseUp;
                        //new MouseButtonEventHandler(OnRevealButtonMouseUp);
                }

                revealButton = value;

                if (revealButton != null)
                {
                    revealButton.PreviewMouseDown += OnRevealButtonMouseDown;
                        //new MouseButtonEventHandler(OnRevealButtonMouseDown);
                    revealButton.PreviewMouseUp += OnRevealButtonMouseUp;
                        //new MouseButtonEventHandler(OnRevealButtonMouseUp);
                }
            }
        }

        private TextBox pwdContainer;

        private TextBox PwdContainer
        {
            get
            {
                return pwdContainer;
            }

            set
            {
                if (pwdContainer != null)
                {
                    pwdContainer.PreviewTextInput -=
                        new TextCompositionEventHandler(OnPreviewTextInput);
                    pwdContainer.PreviewKeyDown -=
                        new KeyEventHandler(OnPreviewKeyDown);
                }

                pwdContainer = value;

                if (pwdContainer != null)
                {
                    pwdContainer.PreviewTextInput +=
                        new TextCompositionEventHandler(OnPreviewTextInput);
                    pwdContainer.PreviewKeyDown +=
                        new KeyEventHandler(OnPreviewKeyDown);
                }
            }
        }

        #endregion Properties

        #region Events

        public override void OnApplyTemplate()
        {
            RevealButton = GetTemplateChild("RevealPwdButton") as Button;
            PwdContainer = GetTemplateChild("PwdContainer") as TextBox;
        }

        private static void OnPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((RevealablePasswordBox)d).OnPasswordChanged(e);
        }

        protected virtual void OnPasswordChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is SecureString secStr && 
                null != secStr &&
                null != PwdContainer)
            {
                PwdContainer.Text = new string('*', secStr.Length);
            }
        }

        /// <summary>
        /// Method to handle PreviewExecutedHandler events.
        /// </summary>
        private static void PreviewExecutedHandler(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
        {
            if (executedRoutedEventArgs.Command == ApplicationCommands.Copy ||
                executedRoutedEventArgs.Command == ApplicationCommands.Cut ||
                executedRoutedEventArgs.Command == ApplicationCommands.Paste)
            {
                executedRoutedEventArgs.Handled = true;
            }
        }

        private void OnRevealButtonMouseDown(object sender, MouseButtonEventArgs e)
        {
            RevealPassword();
            e.Handled = true;
        }

        private void OnRevealButtonMouseUp(object sender, MouseButtonEventArgs e)
        {
            MaskAllDisplayText();
            e.Handled = true;
        }

        /// <summary>
        /// Method to handle PreviewTextInput events.
        /// </summary>
        private void OnPreviewTextInput(object sender, TextCompositionEventArgs textCompositionEventArgs)
        {
            AddToSecureString(textCompositionEventArgs.Text);
            textCompositionEventArgs.Handled = true;
        }

        /// <summary>
        /// Method to handle PreviewKeyDown events.
        /// </summary>
        private void OnPreviewKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
            Key pressedKey = keyEventArgs.Key == Key.System ? keyEventArgs.SystemKey : keyEventArgs.Key;
            switch (pressedKey)
            {
                case Key.Space:
                    AddToSecureString(" ");
                    keyEventArgs.Handled = true;
                    break;

                case Key.Back:
                case Key.Delete:
                    if (PwdContainer.SelectionLength > 0)
                    {
                        RemoveFromSecureString(PwdContainer.SelectionStart, PwdContainer.SelectionLength);
                    }
                    else if (pressedKey == Key.Delete && PwdContainer.CaretIndex < PwdContainer.Text.Length)
                    {
                        RemoveFromSecureString(PwdContainer.CaretIndex, 1);
                    }
                    else if (pressedKey == Key.Back && PwdContainer.CaretIndex > 0)
                    {
                        int caretIndex = PwdContainer.CaretIndex;
                        if (PwdContainer.CaretIndex > 0 && PwdContainer.CaretIndex < PwdContainer.Text.Length)
                            caretIndex = caretIndex - 1;
                        RemoveFromSecureString(PwdContainer.CaretIndex - 1, 1);
                        PwdContainer.CaretIndex = caretIndex;
                    }

                    keyEventArgs.Handled = true;
                    break;
            }
        }

        #endregion Events

        #region Methods

        /// <summary>
        /// Method to add new text into SecureString and process visual output.
        /// </summary>
        private void AddToSecureString(string text)
        {
            if (PwdContainer.SelectionLength > 0)
            {
                RemoveFromSecureString(PwdContainer.SelectionStart, PwdContainer.SelectionLength);
            }

            foreach (char c in text)
            {
                int caretIndex = PwdContainer.CaretIndex;
                SecurePassword.InsertAt(caretIndex, c);
                MaskAllDisplayText();
                if (caretIndex == PwdContainer.Text.Length)
                {
                    _maskTimer.Stop();
                    _maskTimer.Start();
                    PwdContainer.Text = PwdContainer.Text.Insert(caretIndex++, c.ToString());
                }
                else
                {
                    PwdContainer.Text = PwdContainer.Text.Insert(caretIndex++, "●");
                }
                PwdContainer.CaretIndex = caretIndex;
            }
        }

        /// <summary>
        /// Method to remove text from SecureString and process visual output.
        /// </summary>
        private void RemoveFromSecureString(int startIndex, int trimLength)
        {
            int caretIndex = PwdContainer.CaretIndex;
            for (int i = 0; i < trimLength; ++i)
            {
                SecurePassword.RemoveAt(startIndex);
            }

            PwdContainer.Text = PwdContainer.Text.Remove(startIndex, trimLength);
            PwdContainer.CaretIndex = caretIndex;
        }

        private void RevealPassword()
        {
            char[] bytes = new char[SecurePassword.Length];
            IntPtr ptr = IntPtr.Zero;

            try
            {
                ptr = Marshal.SecureStringToBSTR(SecurePassword);
                bytes = new char[SecurePassword.Length];
                Marshal.Copy(ptr, bytes, 0, SecurePassword.Length);
            }
            finally
            {
                if (ptr != IntPtr.Zero)
                    Marshal.ZeroFreeBSTR(ptr);
            }

            StringBuilder builder = new StringBuilder();
            foreach (char value in bytes)
            {
                builder.Append(value);
            }

            PwdContainer.Text = builder.ToString();
            Array.Clear(bytes, 0, bytes.Length);
            builder.Clear();
        }

        private void MaskAllDisplayText()
        {
            _maskTimer.Stop();
            int caretIndex = PwdContainer.CaretIndex;
            PwdContainer.Text = new string('●', PwdContainer.Text.Length);
            PwdContainer.CaretIndex = caretIndex;
        }

        #endregion Methods
    }
}