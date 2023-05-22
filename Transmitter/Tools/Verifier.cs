using System.Diagnostics.Eventing.Reader;
using System.Security.Cryptography;
using System.Text;
using Transmitter.Models;

namespace Transmitter.Tools
{
    public class Verifier
    {
        private readonly string defaultHashFunction;
        public Verifier(IConfiguration configuration)
        {
            defaultHashFunction = configuration["Encryption:HashFunction"];
        }

        public bool verify(Message message)
        {
            var publicKey = KeyHelper.RSAFromPublicKey(message.Identity);

            string hashFunction = message.HashFunction ?? defaultHashFunction;

            byte[] data = Encoding.Unicode.GetBytes(message.Payload);
            byte[] signature = Convert.FromBase64String(message.Signature);

            return publicKey.VerifyData(data, hashFunction, signature);
        }
    }
}
