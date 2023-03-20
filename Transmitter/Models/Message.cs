using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Transmitter.Models
{
    public class Message
    {
        [Required]
        public string Identity { get; }
        [Required]
        public string Signature { get; }
        [Required]
        public string Payload { get; }

        //public string? HashFunction;

        /*
         * Constructor that takes a pre-computed signature required for serialization.
         * We need to be able to construct other people's messages, before testing them for validity.
         * (Should we test for validity immediately to prevent the construction of invalid messages,
         * or should we allow other people's invalid messages to be constructed, 
         * and handle this elsewhere?
        */
        public Message(string identity, string signature, string payload)
        {
            Identity = identity;
            Signature = signature;
            Payload = payload;

            //Should verify message here?
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