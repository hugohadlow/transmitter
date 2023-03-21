using System.Security.Cryptography;
using System.Text;

namespace Transmitter.Tools
{
    public class Signer
    {
        public string Sign(RSACryptoServiceProvider key, string payload, string hashFunction = "SHA256")
        {
            byte[] signature = key.SignData(Encoding.Unicode.GetBytes(payload), hashFunction);
            return Convert.ToBase64String(signature);
        }
    }
}