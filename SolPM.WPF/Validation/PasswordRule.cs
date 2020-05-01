using System;
using System.Globalization;
using System.Security;
using System.Windows.Controls;
using SolPM.Core.Helpers;

namespace SolPM.WPF.Validation
{
    /// <summary>
    /// Compares a password of the PasswordBox with another password.
    /// </summary>
    public class PasswordRule : ValidationRule
    {
        /// <summary>
        /// PasswordBox's password is compared to this password.
        /// </summary>
        public SecureString ValidationPassword { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!(value is SecureString))
            {
                return new ValidationResult(false, "This value is not a SecureString");
            }

            if (null == ValidationPassword || null == value)
            {
                return new ValidationResult(false, "Password is required");
            }

            if (!SecureStringExtension.Equals(value as SecureString, ValidationPassword))
            {
                return new ValidationResult(false, "Passwords do not match");
            }

            return ValidationResult.ValidResult;
        }
    }
}