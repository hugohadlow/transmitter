using Microsoft.AspNetCore.Mvc;
using Transmitter.Models;

namespace Transmitter.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly ILogger<MessageController> _logger;

        public MessageController(ILogger<MessageController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetMessage")]
        public IEnumerable<Message> Get()
        {
            return new List<Message>(){ new Message("identityA", "signatureA", "payloadA")};
        }
    }
}