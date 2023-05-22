using Transmitter.Models;
using Transmitter.Tools;
using Newtonsoft.Json;

namespace Transmitter.Stores
{
    public interface IMessageStore
    {
        IEnumerable<Models.Subscription> GetSubscriptions();
        IEnumerable<Models.Message> GetMessages(string identity);
        void AddMessage(Message message);
    }

    public class MessageStore : IMessageStore
    {
        private class Subscription //Messages are grouped by subscription.
        {
            private readonly string identity;
            public string Identity
            {
                get { return identity; }
            }

            private readonly string directory;
            public string Directory
            {
                get { return directory; }
            }

            private List<string> messageFilenames = new List<string>();
            private List<Message> messages = new List<Message>();

            public Subscription(string identity, string directory)
            {
                this.identity = identity;
                this.directory = directory;
                LoadMessages();
            }

            private void LoadMessages()
            {
                var fullDirectory = ArchiveLocation + "/" + directory;
                System.IO.Directory.CreateDirectory(fullDirectory);
                
                foreach (var file in System.IO.Directory.EnumerateFiles(fullDirectory))
                {
                    messageFilenames.Add(Path.GetFileNameWithoutExtension(file));
                    string jsonString = File.ReadAllText(file);
                    Message message = JsonConvert.DeserializeObject<Message>(jsonString)!;
                    messages.Add(message);
                }
            }

            public void AddMessage(Message message)
            {
                //Need to validate message



                if (messages.Any(x =>
                    x.Identity.Equals(message.Identity) &&
                        x.Payload.Equals(message.Payload)))
                    return; //Don't store the same message twice.

                //Use signature to generate filename for message
                string shortName = message.Signature.Substring(0, 10);
                for (int i = 11; messageFilenames.Contains(shortName); i++)
                {
                    shortName = message.Signature.Substring(0, i);
                }

                string jsonString = JsonConvert.SerializeObject(message);
                var fullDirectory = ArchiveLocation + "/" + directory;
                System.IO.Directory.CreateDirectory(fullDirectory);
                File.WriteAllText(fullDirectory + "/" + shortName + ".json", jsonString);
                messageFilenames.Add(shortName);

                messages.Add(message);
            }

            public IEnumerable<Message> GetMessages()
            {
                return messages;
            }
        }

        public static string ArchiveLocation { get; private set; }

        //Keyed by identity
        private Dictionary<string, Subscription> subscriptions = new Dictionary<string, Subscription>();

        public MessageStore(IConfiguration configuration)
        {
            ArchiveLocation = configuration["Archive:Location"];
            Directory.CreateDirectory(ArchiveLocation);
            var subscriptionsFile = ArchiveLocation + "/subscriptions.json";
            if (File.Exists(subscriptionsFile)) {

                var json = File.ReadAllText(subscriptionsFile);
                var identityToDirectory = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                subscriptions = identityToDirectory.ToDictionary(x => x.Key, x => new Subscription(x.Key, x.Value));
            }
        }

        public void AddMessage(Message message)
        {
            var identity = message.Identity;

            if (!HasSubscription(identity)){
                AddSubscription(identity);
            }

            var subscription = subscriptions[identity];
            subscription.AddMessage(message);
        }

        public void AddSubscription(string identity)
        {
            //Generate unique directory name
            string shortName = identity.Substring(0, 10);
            for (int i = 11; subscriptions.Any(x=>x.Value.Directory.Equals(shortName)); i++)
            {
                shortName = identity.Substring(0, i);
            }

            subscriptions.Add(identity, new Subscription(identity, shortName));
            WriteSubscriptions();
        }

        public IEnumerable<Models.Subscription> GetSubscriptions()
        {
            return subscriptions.Keys.Select(x=>new Models.Subscription(x));
        }

        private bool HasSubscription(string identity)
        {
            return subscriptions.ContainsKey(identity);
        }

        public IEnumerable<Message> GetMessages(string identity)
        {
            if (subscriptions.ContainsKey(identity))
                return subscriptions[identity].GetMessages();
            else
                return Enumerable.Empty<Message>();
        }

        private void WriteSubscriptions()
        {
            //Don't write messages, only identity and directory
            var json = JsonConvert.SerializeObject(subscriptions.ToDictionary(x => x.Key, x => x.Value.Directory));
            Directory.CreateDirectory(ArchiveLocation);
            File.WriteAllText(ArchiveLocation + "/subscriptions.json", json);
        }
    }
}
