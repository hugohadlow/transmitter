using Microsoft.Extensions.Configuration;
using Transmitter.Models;
using Transmitter.Store;
using Transmitter.Tools;

namespace TransmitterTest.Stores
{
    public class TestMessageStore
    {
        private void DeleteAllSubscriptions()
        {
            var archiveLocation = configuration["Archive:Location"];
            if (Directory.Exists(archiveLocation))
                Directory.Delete(archiveLocation, true);
        }

        IConfiguration configuration = TestUtils.InitConfiguration();

        [SetUp]
        public void Setup()
        {
            DeleteAllSubscriptions();
        }

        [TearDown]
        public void TearDown()
        {
            //DeleteAllSubscriptions();
        }

        [Test]
        public void TestWriteSubscriptions()
        {
            var identity = Convert.ToBase64String(Guid.NewGuid().ToByteArray()); //Identity must be Base64
            var messageStore1 = new MessageStore(configuration);
            messageStore1.AddSubscription(identity);

            //Use new message store to check persistence to disk.
            var messageStore2 = new MessageStore(configuration);
            var subscriptions = messageStore2.GetSubscriptions();
            Assert.AreEqual(1, subscriptions.Count());
            Assert.AreEqual(identity, subscriptions.First());
        }


        [Test]
        public void TestWriteMessage()
        {
            var keyPair = KeyHelper.Generate();
            string payload = "payload";
            Signer signer = new Signer();
            var signature = signer.Sign(keyPair, payload);
            var identity = KeyHelper.PublicKey(keyPair);
            Message message = new Message(identity, signature, payload);

            var messageStore1 = new MessageStore(configuration);
            messageStore1.AddMessage(message);

            //Use new message store to check persistence to disk.
            var messageStore2 = new MessageStore(configuration);
            var subscriptions = messageStore2.GetSubscriptions();
            Assert.AreEqual(1, subscriptions.Count());
            var subscription = subscriptions.First();
            Assert.AreEqual(identity, subscription);
            var messages = messageStore2.GetMessages(identity);
            Assert.AreEqual(1, messages.Count());
            var storedMessage = messages.First();
            Assert.AreEqual(identity, storedMessage.Identity);
            Assert.AreEqual(payload, storedMessage.Payload);
            Assert.AreEqual(signature, storedMessage.Signature);

        }
    }
}