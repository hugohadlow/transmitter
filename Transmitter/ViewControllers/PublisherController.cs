using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using Transmitter.Models;
using Transmitter.Stores;
using Transmitter.Tools;

namespace Transmitter.ViewControllers
{
    [ApiController]
    public class PublisherController : Controller
    {
        private readonly ILogger<PublisherController> _logger;
        private readonly IKeyStore<SigningKey> signingKeyStore;
        private readonly IMessageStore messageStore;

        public PublisherController(ILogger<PublisherController> logger, IKeyStore<SigningKey> signingKeyStore, IMessageStore messageStore)
        {
            _logger = logger;
            this.signingKeyStore = signingKeyStore;
            this.messageStore = messageStore;
        }

        private IDictionary<String, String> GetIdentities()
        {
            return signingKeyStore.GetKeys().ToDictionary(x => x.PublicKey, x => x.Name);
        }

        [HttpGet]
        [Route("Publisher", Name = "PublisherQuestionMark")]
        public IActionResult PublisherQuestionMark(string? publicKey)
        {
            return Publisher(publicKey);
        }

        [HttpGet]
        [Route("Publisher/{publicKey}", Name = "PublisherSlash")]
        public IActionResult PublisherSlash(string publicKey)
        {
            return Publisher(publicKey);
        }

        private IActionResult Publisher(string publicKey)
        {
            dynamic model = new ExpandoObject();
            if (publicKey != null)
            {
                var key = signingKeyStore.GetKey(publicKey);
                model.PublicKey = publicKey;
                model.Nickname = key.Name;
            }

            model.Identities = GetIdentities();
            return View("/Views/Shared/Publisher.cshtml", model);
        }

        [HttpPost]
        [Route("Publisher", Name = "Publisher")]
        public IActionResult NewMessage([FromForm] IFormCollection body)
        {
            var publicKey = body["publicKey"];
            var payload = body["payload"];
            var key = signingKeyStore.GetKey(publicKey);
            var signature = Signer.Sign(key, payload);
            var message = new Message(key.PublicKey, signature, payload);
            messageStore.AddMessage(message);
            return RedirectToRoute("PublisherQuestionMark", 
                new RouteValueDictionary { { "publicKey", publicKey } });
        }
    }
}