using System.Security.Cryptography;
using System.Text;

namespace Transmitter.Tools
{
    public class Signer
    {
        public string Sign(RSACryptoServiceProvider key, string payload)
        {
            byte[] signature = key.SignData(Encoding.Unicode.GetBytes(payload), "SHA256");
            return Convert.ToBase64String(signature);
        }
    }
}
