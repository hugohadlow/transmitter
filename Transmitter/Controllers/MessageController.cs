using Microsoft.AspNetCore.Mvc;
using Transmitter.Models;
using Transmitter.Stores;

namespace Transmitter.Controllers
{
    [ApiController]
    [Route("messages")]
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

    [ApiController]
    [Route("message")]
    public class MessageController : ControllerBase
    {
        private readonly ILogger<MessageController> _logger;
        private readonly IMessageStore messageStore;

        public MessageController(ILogger<MessageController> logger, IMessageStore messageStore)
        {
            _logger = logger;
            this.messageStore = messageStore;
        }

        [HttpPut(Name = "PutMessage")]
        public void Put(Message message)
        {
            //Need to add validation
            messageStore.AddMessage(message);
        }
    }
}