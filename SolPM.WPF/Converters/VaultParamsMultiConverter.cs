using SolPM.Core.Models;
using SolPM.Core.ViewModels.Parameters;
using System;
using System.Globalization;
using System.Security;
using System.Windows.Data;

namespace SolPM.WPF.Converters
{
    // No idea how to write these
    public class VaultParamsMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            VaultParams vaultParams = new VaultParams();

            if (values[0] is string filePath)
            {
                vaultParams.FilePath = filePath;
            }

            if (values[1] is string name)
            {
                vaultParams.Name = name;
            }

            if (values[2] is string description)
            {
                vaultParams.Description = description;
            }

            vaultParams.EncryptionInfo = new EncryptionInfo();

            if (values[3] is int index)
            {
                // Algorithm combobox index
                switch (index)
                {
                    case 0:
                        vaultParams.EncryptionInfo.SelectedAlgorithm = Algorithm.AES_256;
                        break;

                    case 1:
                        vaultParams.EncryptionInfo.SelectedAlgorithm = Algorithm.Twofish_256;
                        break;

                    default:
                        break;
                }
            }

            if (values[4] is double deriviationRounds)
            {
                vaultParams.EncryptionInfo.DeriviationRounds = System.Convert.ToInt32(Math.Floor(deriviationRounds));
            }

            if (values[5] is SecureString password)
            {
                vaultParams.Password = password;
            }

            return vaultParams;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            // No conversions needed
            return null;
        }
    }
}