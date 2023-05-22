using Newtonsoft.Json;
using Transmitter.Models;
using Transmitter.Tools;

namespace Transmitter.Stores
{
    public interface IKeyStore
    {
        IEnumerable<Key> GetKeys();
        Key GetKey(string publicKey);
        string GenerateKey(string nickname);
        void EditKey(string publicKey, string nickname);
        void DeleteKey(string publicKey);
    }

    public class KeyStore : IKeyStore
    {
        private readonly string keysLocation;

        protected Dictionary<string, Key> keys;

        public KeyStore(IConfiguration configuration) {
            keysLocation = configuration["Keys:Location"];

            if (File.Exists(keysLocation + "/keys.json"))
            {
                var json = File.ReadAllText(keysLocation + "/keys.json");
                var keyList = JsonConvert.DeserializeObject<List<Key>>(json);
                keys = keyList.ToDictionary(x=>x.PublicKey, x=>x);
            }
            else
            {
                keys = new Dictionary<string, Key>();
            }
        }

        public IEnumerable<Key> GetKeys()
        {
            return keys.Values;
        }

        public Key GetKey(string publicKey)
        {
            return keys[publicKey];
        }

        public void AddKey(Key key)
        {
            keys.Add(key.PublicKey, key);
            WriteKeyPairs();
        }

        public string GenerateKey(string nickname)
        {
            var key = KeyHelper.Generate(nickname);
            keys.Add(key.PublicKey, key);
            WriteKeyPairs();
            return key.PublicKey;
        }

        public void EditKey(string publicKey, string nickname)
        {
            var key = keys[publicKey];
            key.Name = nickname;
            WriteKeyPairs();
        }

        public void DeleteKey(string publicKey)
        {
            var key = keys.Remove(publicKey);
            WriteKeyPairs();
        }

        public void WriteKeyPairs()
        {
            var json = JsonConvert.SerializeObject(keys.Values);
            Directory.CreateDirectory(keysLocation);
            File.WriteAllText(keysLocation + "/keys.json", json);
        }
    }
}
