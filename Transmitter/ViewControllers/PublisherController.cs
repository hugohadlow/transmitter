using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        [Route("Publisher/{publicKey}", Name="PublisherID")]
        public IActionResult Publisher(string publicKey)
        {
            var key = signingKeyStore.GetKey(publicKey);
            return View(key);
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
            return RedirectToRoute("PublisherID", 
                new RouteValueDictionary { { "publicKey", publicKey } });
        }
    }
}