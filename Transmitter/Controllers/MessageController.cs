using Microsoft.AspNetCore.Mvc;
using Transmitter.Models;
using Transmitter.Stores;

namespace Transmitter.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly ILogger<MessageController> _logger;
        private readonly IMessageStore messageStore;

        public MessageController(ILogger<MessageController> logger, IMessageStore messageStore)
        {
            _logger = logger;
            this.messageStore = messageStore;
        }

        [HttpGet(Name = "GetMessage")]
        public IEnumerable<Message> Get()
        {
            return new List<Message>(){ new Message("identityA", "signatureA", "payloadA")};
        }
    }
}