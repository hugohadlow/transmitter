using Microsoft.AspNetCore.Mvc;
using Transmitter.Models;
using Transmitter.Stores;

namespace Transmitter.Controllers
{
    [ApiController]
    [Route("subscriptions")]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ILogger<SubscriptionsController> _logger;
        private readonly IMessageStore messageStore;

        public SubscriptionsController(ILogger<SubscriptionsController> logger, IMessageStore messageStore)
        {
            _logger = logger;
            this.messageStore = messageStore;
        }

        [HttpGet(Name = "GetSubscriptions")]
        public IEnumerable<Subscription> Get()
        {
            return messageStore.GetSubscriptions();
        }
    }
}