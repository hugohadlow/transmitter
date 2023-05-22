using System.Security.Cryptography;
using Transmitter.Models;

namespace Transmitter.Tools
{
    public class KeyHelper
    {
        public static Key Generate(string nickname)
        {
            var rsa = new RSACryptoServiceProvider(2048) { PersistKeyInCsp = false };
            return new Key(
                Convert.ToBase64String(rsa.ExportCspBlob(false)),
                Convert.ToBase64String(rsa.ExportCspBlob(true)), 
                nickname);
        }

        public static RSACryptoServiceProvider RSAFromKey(Key key)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.ImportCspBlob(Convert.FromBase64String(key.PublicKey));
            rsa.ImportCspBlob(Convert.FromBase64String(key.PrivateKey));
            return rsa;
        }

        public static RSACryptoServiceProvider RSAFromPublicKey(string publicKey)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.ImportCspBlob(Convert.FromBase64String(publicKey));
            return rsa;
        }
    }
}
