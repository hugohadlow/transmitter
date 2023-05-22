using Microsoft.AspNetCore.Mvc;
using Transmitter.Models;
using Transmitter.Stores;

namespace Transmitter.Controllers
{
    [ApiController]
    [Route("api/messages")]
    public class MessagesController : ControllerBase
    {
        private readonly ILogger<MessagesController> _logger;
        private readonly IMessageStore messageStore;

        public MessagesController(ILogger<MessagesController> logger, IMessageStore messageStore)
        {
            _logger = logger;
            this.messageStore = messageStore;
        }

        [HttpGet("{identity}", Name = "GetMessages")]
        public IEnumerable<Message> Get(string identity)
        {
            return messageStore.GetMessages(identity);
        }
    }
}