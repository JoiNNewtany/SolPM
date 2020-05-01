using System;
using System.Globalization;
using System.IO;
using System.Windows.Controls;

namespace SolPM.WPF.Validation
{
    public class OpenFilePathRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string path;

            try
            {
                path = value.ToString();
            }
            catch (Exception e)
            {
                return new ValidationResult(false, e.Message);
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                return new ValidationResult(false, "Please provide a file path.");
            }

            if (!File.Exists(path))
            {
                return new ValidationResult(false, "Specified file does not exist.");
            }

            return ValidationResult.ValidResult;
        }
    }
}