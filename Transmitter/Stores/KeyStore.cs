using Newtonsoft.Json;
using System.Security.Cryptography;

namespace Transmitter.Stores
{
    public class KeyStore
    {
        private readonly string keysLocation;

        public List<RSACryptoServiceProvider> keyPairs;

        public KeyStore(IConfiguration configuration) {
            keysLocation = configuration["Keys:Location"];

            if (File.Exists(keysLocation + "/keys.json"))
            {
                var json = File.ReadAllText(keysLocation + "/keys.json");
                var keyPairsXml = JsonConvert.DeserializeObject<List<String>>(json);
                keyPairs = keyPairsXml.Select(x =>
                {                    
                    var keyPair = new RSACryptoServiceProvider();
                    keyPair.FromXmlString(x);
                    return keyPair;
                }).ToList();
            }
            else
            {
                keyPairs = new List<RSACryptoServiceProvider>();
            }
        }

        public void AddKeyPair(RSACryptoServiceProvider keyPair)
        {
            keyPairs.Add(keyPair);
        }

        public void WriteKeyPairs()
        {
            var keyPairsXml = keyPairs.Select(x => x.ToXmlString(true));
            var json = JsonConvert.SerializeObject(keyPairsXml);
            Directory.CreateDirectory(keysLocation);
            File.WriteAllText(keysLocation + "/keys.json", json);
        }
    }
}
