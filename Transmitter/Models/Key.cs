using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Transmitter.Models
{
    public class Key
    {
        [Required]
        public string PublicKey { get; }
        [Required]
        public string PrivateKey { get; }
        [Required]
        public string Name { get; set; }

        public Key(string publicKey, string privateKey, string name)
        {
            PublicKey = publicKey;
            PrivateKey = privateKey;
            Name = name;
        }

        override public string ToString()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }
    }
}