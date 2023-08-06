using System.Security.Cryptography;
using System.Text;
using Transmitter.Models;

namespace Transmitter.Tools
{
    public class Signer
    {
        public static string Sign(SigningKey key, string payload, string hashFunction = "SHA512")
        {
            RSACryptoServiceProvider rsa = KeyHelper.RSAFromKey(key);
            byte[] signature = rsa.SignData(Encoding.Unicode.GetBytes(payload), hashFunction);
            return Base32Encoding.ToString(signature);
        }
    }
}