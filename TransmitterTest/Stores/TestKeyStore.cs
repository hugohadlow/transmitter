using Microsoft.Extensions.Configuration;
using Transmitter.Stores;
using Transmitter.Tools;

namespace TransmitterTest.Stores
{
    public class TestKeyStore
    {
        IConfiguration configuration = TestUtils.InitConfiguration();

        private void DeleteAllKeys()
        {
            var keysLocation = configuration["Keys:Location"];
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
            var keyPair = KeyHelper.Generate("nickname");

            KeyStore keyStore1 = new KeyStore(configuration);
            keyStore1.AddKey(keyPair);
            keyStore1.WriteKeyPairs();

            //Use new KeyStore to check persistence to disk.
            KeyStore keyStore2 = new KeyStore(configuration);
            Assert.AreEqual(1, keyStore2.GetKeys().Count());
            Assert.AreEqual(
                keyPair.PublicKey, 
                keyStore2.GetKeys().First().PublicKey);
        }
    }
}