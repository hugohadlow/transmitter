using System.Security.Cryptography;

namespace Transmitter.Tools
{
    public class KeyHelper
    {
        public static RSACryptoServiceProvider Generate()
        {
            return new RSACryptoServiceProvider(2048) { PersistKeyInCsp = false };
        }

        public static string PublicKey(RSACryptoServiceProvider keyPair)
        {
            return Convert.ToBase64String(keyPair.ExportRSAPublicKey());
        }
    }
}
