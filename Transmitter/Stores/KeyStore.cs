using Newtonsoft.Json;
using System.Security.Cryptography;
using Transmitter.Models;
using Transmitter.Tools;

namespace Transmitter.Stores
{
    public interface IKeyStore<T > where T : Key
    {
        IEnumerable<T> GetKeys();
        T GetKey(string publicKey);
        string GenerateKey(string nickname);
        void EditKey(string publicKey, string nickname);
        void DeleteKey(string publicKey);
    }

    public class KeyStore<T> : IKeyStore<T> where T : Key
    {
        private readonly string keysLocation;
        private readonly int keyLength;

        protected Dictionary<string, T> keys;

        public KeyStore(IConfiguration configuration) {
            string path = "Keys:" + typeof(T).Name;
            keysLocation = configuration[path + ":Location"];
            keyLength = Convert.ToInt32(configuration[path + ":KeyLength"]);
            if (keyLength == 0) throw new ArgumentException("Key length cannot be 0");

                if (File.Exists(keysLocation + "/keys.json"))
            {
                var json = File.ReadAllText(keysLocation + "/keys.json");
                var keyList = JsonConvert.DeserializeObject<List<T>>(json);
                keys = keyList.ToDictionary(x=>x.PublicKey, x=>x);
            }
            else
            {
                keys = new Dictionary<string, T>();
            }
        }

        public IEnumerable<T> GetKeys()
        {
            return keys.Values;
        }

        public T GetKey(string publicKey)
        {
            return keys[publicKey];
        }

        public string GenerateKey(string nickname)
        {
            var rsa = new RSACryptoServiceProvider(keyLength) { PersistKeyInCsp = false };
            var key = (T)Activator.CreateInstance(typeof(T), new object[] {
                    Base32Encoding.ToString(rsa.ExportCspBlob(false)),
                    Base32Encoding.ToString(rsa.ExportCspBlob(true)),
                    nickname 
            });

            keys.Add(key.PublicKey, (T)key);
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
