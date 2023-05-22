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
            var keyPair = KeyHelper.Generate("nickname");
            string payload = "payload";
            var signature = Signer.Sign(keyPair, payload);
            Message message = new Message(keyPair.PublicKey, signature, payload);
            Console.WriteLine(message);

            Verifier verifier = new Verifier(configuration);
            Assert.True(verifier.verify(message));
        }

        [Test]
        public void SignAndVerifyMessageSHA512()
        {
            var keyPair = KeyHelper.Generate("nickname");
            string payload = "payload";
            var signature = Signer.Sign(keyPair, payload, "SHA512");
            Message message = new Message(keyPair.PublicKey, signature, payload, "SHA512");
            Console.WriteLine(message);

            Verifier verifier = new Verifier(configuration);
            Assert.True(verifier.verify(message));
        }
    }
}