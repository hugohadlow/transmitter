using Microsoft.Extensions.Configuration;
using Transmitter.Models;
using Transmitter.Stores;

namespace TransmitterTest.Stores
{
    public class TestSigningKeyStore
    {
        IConfiguration configuration = TestUtils.InitConfiguration();

        private void DeleteAllKeys()
        {
            var keysLocation = configuration["Keys:SigningKey:Location"];
            if (Directory.Exists(keysLocation))
                Directory.Delete(keysLocation, true);
        }

        [SetUp]
        public void Setup()
        {
            DeleteAllKeys();
        }

        [Test]
        public void TestWriteKeys()
        {
            KeyStore<SigningKey> keyStore1 = new KeyStore<SigningKey>(configuration);
            var publicKey = keyStore1.GenerateKey("nickname");
            var keyPair = keyStore1.GetKey(publicKey);
            keyStore1.WriteKeyPairs();

            //Use new KeyStore to check persistence to disk.
            KeyStore<SigningKey> keyStore2 = new KeyStore<SigningKey>(configuration);
            Assert.AreEqual(1, keyStore2.GetKeys().Count());
            Assert.AreEqual(
                keyPair.PublicKey, 
                keyStore2.GetKeys().First().PublicKey);
        }
    }
}