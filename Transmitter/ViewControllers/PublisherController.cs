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
        private readonly IKeyStore keyStore;
        private readonly IMessageStore messageStore;

        public PublisherController(ILogger<PublisherController> logger, IKeyStore keyStore, IMessageStore messageStore)
        {
            _logger = logger;
            this.keyStore = keyStore;
            this.messageStore = messageStore;
        }

        [HttpGet]
        [Route("Publisher/{publicKey}", Name="PublisherID")]
        public IActionResult Publisher(string publicKey)
        {
            var key = keyStore.GetKey(publicKey);
            return View(key);
        }

        [HttpPost]
        [Route("Publisher", Name = "Publisher")]
        public IActionResult NewMessage([FromForm] IFormCollection body)
        {
            var publicKey = body["publicKey"];
            var payload = body["payload"];
            var key = keyStore.GetKey(publicKey);
            var signature = Signer.Sign(key, payload);
            var message = new Message(key.PublicKey, signature, payload);
            messageStore.AddMessage(message);
            return RedirectToRoute("PublisherID", 
                new RouteValueDictionary { { "publicKey", publicKey } });
        }
    }
}