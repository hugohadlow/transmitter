using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Transmitter.Models
{
    public class Microblog
    {
        [Required]
        public string Type { get; } = "microblog";
        [Required]
        public string Message { get; }
        public string? QuoteID { get; }
        public string? ReplyID { get; }

        public Microblog(string message, string? replyID = null, string? quoteID = null)
        {
            Message = message;
            QuoteID = quoteID;
            ReplyID = replyID;
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