using Microsoft.AspNetCore.Mvc;
using Transmitter.Stores;

namespace Transmitter.ViewControllers
{
    [ApiController]
    [Route("Subscriptions")]
    public class SubscriptionsController : Controller
    {
        private readonly ILogger<SubscriptionsController> _logger;
        private readonly IMessageStore messageStore;

        public SubscriptionsController(ILogger<SubscriptionsController> logger, IMessageStore messageStore)
        {
            _logger = logger;
            this.messageStore = messageStore;
        }

        [HttpGet]
        public IActionResult Subscriptions()
        {
            var subscriptions = messageStore.GetSubscriptions();
            return View(subscriptions);
        }
    }
}