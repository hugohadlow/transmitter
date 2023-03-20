using Transmitter.Models;
using Transmitter.Tools;

namespace TransmitterTest
{
    public class TestSignerVerifier
    {
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

            Verifier verifier = new Verifier();
            Assert.True(verifier.verify(message));
        }
    }
}