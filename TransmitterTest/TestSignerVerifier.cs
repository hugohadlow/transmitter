using Microsoft.Extensions.Configuration;
using Transmitter.Models;
using Transmitter.Tools;

namespace TransmitterTest
{
    public class TestSignerVerifier
    {
        IConfiguration configuration = TestUtils.InitConfiguration();

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SignAndVerifyMessage()
        {
            var keyPair = KeyHelper.Generate();
            string payload = "payload";
            Signer signer = new Signer();
            var signature = signer.Sign(keyPair, payload);
            Message message = new Message(KeyHelper.PublicKey(keyPair), signature, payload);
            Console.WriteLine(message);

            Verifier verifier = new Verifier(configuration);
            Assert.True(verifier.verify(message));
        }

        [Test]
        public void SignAndVerifyMessageSHA512()
        {
            var keyPair = KeyHelper.Generate();
            string payload = "payload";
            Signer signer = new Signer();
            var signature = signer.Sign(keyPair, payload, "SHA512");
            Message message = new Message(KeyHelper.PublicKey(keyPair), signature, payload, "SHA512");
            Console.WriteLine(message);

            Verifier verifier = new Verifier(configuration);
            Assert.True(verifier.verify(message));
        }
    }
}