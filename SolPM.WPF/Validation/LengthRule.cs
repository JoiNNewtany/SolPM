using System;
using System.Globalization;
using System.Windows.Controls;

namespace SolPM.WPF.Validation
{
    public class LengthRule : ValidationRule
    {
        public int Min { get; set; }
        public int Max { get; set; } = 0;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int length;

            try
            {
                length = value.ToString().Length;
            }
            catch (Exception e)
            {
                return new ValidationResult(false, e.Message);
            }

            if (length < Min || length > Max)
            {
                if (Min == 0 && Max != 0)
                {
                    return new ValidationResult(false, $"The value must be shorter than {Max} characters.");
                }
                if (Max == 0 && Min != 0)
                {
                    return new ValidationResult(false, $"The value must be longer than {Min} characters.");
                }
                else
                {
                    return new ValidationResult(false, $"The value must be between {Min} and {Max} characters.");
                }
            }

            return ValidationResult.ValidResult;
        }
    }
}