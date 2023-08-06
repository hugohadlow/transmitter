using Microsoft.Extensions.Configuration;
using Transmitter.Models;
using Transmitter.Stores;
using Transmitter.Tools;

namespace TransmitterTest
{
    public class TestSignerVerifier
    {
        IConfiguration configuration = TestUtils.InitConfiguration();
        KeyStore<SigningKey> keyStore;

        [SetUp]
        public void Setup()
        {
            keyStore = new KeyStore<SigningKey>(configuration);
        }

        [Test]
        public void SignAndVerifyMessage_DefaultHash()
        {
            var publicKey = keyStore.GenerateKey("nickname");
            var keyPair = keyStore.GetKey(publicKey);
            string payload = "payload";
            var signature = Signer.Sign(keyPair, payload);
            Message message = new Message(keyPair.PublicKey, signature, payload);
            Console.WriteLine(message);

            Verifier verifier = new Verifier(configuration);
            Assert.True(verifier.verify(message));
        }

        [Test]
        public void SignAndVerifyMessage_SHA512()
        {
            var publicKey = keyStore.GenerateKey("nickname");
            var keyPair = keyStore.GetKey(publicKey);
            string payload = "payload";
            var signature = Signer.Sign(keyPair, payload, "SHA512");
            Message message = new Message(keyPair.PublicKey, signature, payload, "SHA512");
            Console.WriteLine(message);

            Verifier verifier = new Verifier(configuration);
            Assert.True(verifier.verify(message));
        }


        [Test]
        public void SignAndVerifyMessage_SHA384()
        {
            var publicKey = keyStore.GenerateKey("nickname");
            var keyPair = keyStore.GetKey(publicKey);
            string payload = "payload";
            var signature = Signer.Sign(keyPair, payload, "SHA384");
            Message message = new Message(keyPair.PublicKey, signature, payload, "SHA384");
            Console.WriteLine(message);

            Verifier verifier = new Verifier(configuration);
            Assert.True(verifier.verify(message));
        }
    }
}