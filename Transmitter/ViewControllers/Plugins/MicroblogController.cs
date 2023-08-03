using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System.Text.Json;
using Transmitter.Stores;
using Transmitter.Models;
using Transmitter.Tools;

namespace Transmitter.ViewControllers
{
    [ApiController]
    public class MicroblogController : Controller
    {
        private readonly ILogger<MicroblogController> _logger;
        private readonly IKeyStore<SigningKey> signingKeyStore;
        private readonly IMessageStore messageStore;

        public MicroblogController(ILogger<MicroblogController> logger, IKeyStore<SigningKey> signingKeyStore, IMessageStore messageStore)
        {
            _logger = logger;
            this.signingKeyStore = signingKeyStore;
            this.messageStore = messageStore;
        }

        private IEnumerable<Message> GetMicroblogs(IEnumerable<Message> messages)
        {
            messages = messages.Where(x =>
            {
                try
                {
                    var obj = JsonSerializer.Deserialize<JsonElement>(x.Payload);
                    obj.TryGetProperty("Type", out JsonElement value);
                    return value.ValueEquals("microblog");
                }
                catch (Exception)
                {
                    return false;
                }
            });
            return messages;
        }

        private IDictionary<String, String> GetIdentities()
        {
            return signingKeyStore.GetKeys().ToDictionary(x=>x.PublicKey, x=>x.Name);
        }

        [HttpGet]
        [Route("Microblog", Name="Microblog")]
        public IActionResult MicroblogQuestionMark(string? publicKey, string? replyId, string? quoteId)
        {
            return Microblog(publicKey, replyId, quoteId);
        }

        [HttpGet]
        [Route("Microblog/{publicKey}", Name="MicroblogSlash")]
        public IActionResult MicroblogSlash(string publicKey)
        {
            return Microblog(publicKey);
        }

        private IActionResult Microblog(string? publicKey = null, string? replyId = null, string? quoteId = null)
        {
            dynamic model = new ExpandoObject();
            IEnumerable<Message> messages;

            if (publicKey != null)
            {
                model.PublicKey = publicKey;
                var key = signingKeyStore.GetKey(publicKey);
                model.Nickname = key.Name;
            }
            if (replyId != null)
            {
                model.ReplyId = replyId;
            }
            else if (quoteId != null)
            {
                model.QuoteId = quoteId;
            }
            messages = messageStore.GetMessages();
            messages = GetMicroblogs(messages);
            model.Messages = messages;            
            model.Identities = GetIdentities();
            return View("/Views/Plugins/Microblog.cshtml", model);
        }

        [HttpPost]
        [Route("Microblog", Name="MicroblogPublish")]
        public IActionResult NewMessage([FromForm] IFormCollection body)
        {
            var publicKey = body["publicKey"];
            var payload = body["payload"];
            var replyId = body["replyId"];
            var quoteId = body["quoteId"];
            var microblog = new Microblog(payload, replyId, quoteId).ToString();
            var key = signingKeyStore.GetKey(publicKey);
            var signature = Signer.Sign(key, microblog);
            var message = new Message(key.PublicKey, signature, microblog);
            messageStore.AddMessage(message);
            return RedirectToRoute("Microblog",
                new RouteValueDictionary { { "publicKey", publicKey } });
        }
    }
}