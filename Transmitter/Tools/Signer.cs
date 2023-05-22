using System.Security.Cryptography;
using System.Text;
using Transmitter.Models;

namespace Transmitter.Tools
{
    public class Signer
    {
        public string Sign(Key key, string payload, string hashFunction = "SHA256")
        {
            RSACryptoServiceProvider rsa = KeyHelper.RSAFromKey(key);
            byte[] signature = rsa.SignData(Encoding.Unicode.GetBytes(payload), hashFunction);
            return Convert.ToBase64String(signature);
        }
    }
}