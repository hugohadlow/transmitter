using System.Security.Cryptography;
using Transmitter.Models;

namespace Transmitter.Tools
{
    public class KeyHelper
    {
        //Use base32 for safe filenames and urls

        public static RSACryptoServiceProvider RSAFromKey(Key key)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.ImportCspBlob(Base32Encoding.ToBytes(key.PublicKey));
            rsa.ImportCspBlob(Base32Encoding.ToBytes(key.PrivateKey));
            return rsa;
        }

        public static RSACryptoServiceProvider RSAFromPublicKey(string publicKey)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.ImportCspBlob(Base32Encoding.ToBytes(publicKey));
            return rsa;
        }
    }
}
