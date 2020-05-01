using System;
using System.Globalization;
using System.Security;
using System.Windows.Controls;

namespace SolPM.WPF.Validation
{
    public class EmptyRule : ValidationRule
    {
        private bool IsNullable<T>(T obj)
        {
            if (obj == null) return true;
            Type type = typeof(T);
            if (!type.IsValueType) return true; // ref-type
            if (Nullable.GetUnderlyingType(type) != null) return true; // Nullable<T>
            return false; // value-type
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!IsNullable(value))
            {
                return new ValidationResult(false, "Value must be of a nullable type.");
            }

            if (value is null)
            {
                return new ValidationResult(false, "This field is required.");
            }

            if (value is SecureString && 0 >= (value as SecureString).Length)
            {
                return new ValidationResult(false, "You must enter a password.");
            }

            if (value is string && string.IsNullOrWhiteSpace(value as string))
            {
                return new ValidationResult(false, "This field is required.");
            }

            return ValidationResult.ValidResult;
        }
    }
}