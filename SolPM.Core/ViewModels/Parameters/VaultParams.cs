using SolPM.Core.Models;
using System.Security;

namespace SolPM.Core.ViewModels.Parameters
{
    public class VaultParams
    {
        public string FilePath;
        public string Name;
        public string Description;
        public EncryptionInfo EncryptionInfo;
        public SecureString Password;
    }
}