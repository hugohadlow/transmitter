using Microsoft.AspNetCore.Mvc;
using Transmitter.Stores;

namespace Transmitter.ViewControllers
{
    [ApiController]
    [Route("Subscription/{publicKey}")]
    public class SubscriptionController : Controller
    {
        private readonly ILogger<SubscriptionController> _logger;
        private readonly IMessageStore messageStore;

        public SubscriptionController(ILogger<SubscriptionController> logger, IMessageStore messageStore)
        {
            _logger = logger;
            this.messageStore = messageStore;
        }

        [HttpGet]
        public IActionResult Subscription(string publicKey)
        {
            var messages = messageStore.GetMessages(publicKey);
            return View(messages);
        }
    }
}