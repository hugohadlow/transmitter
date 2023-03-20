using System.Security.Cryptography;
using System.Text;
using Transmitter.Models;

namespace Transmitter.Tools
{
    public class Verifier
    {
        public bool verify(Message message)
        {
            var publicKey = new RSACryptoServiceProvider();
            publicKey.ImportRSAPublicKey(Convert.FromBase64String(message.Identity), out _);

            byte[] data = Encoding.Unicode.GetBytes(message.Payload);
            byte[] signature = Convert.FromBase64String(message.Signature);

            return publicKey.VerifyData(data, "SHA256", signature);
        }
    }
}
