using Microsoft.AspNetCore.Mvc;
using Transmitter.Stores;

namespace Transmitter.Controllers
{
    [ApiController]
    [Route("api/keys")]
    public class KeysController : ControllerBase
    {
        private readonly ILogger<KeysController> _logger;
        private readonly IKeyStore keyStore;

        public KeysController(ILogger<KeysController> logger, IKeyStore keyStore)
        {
            _logger = logger;
            this.keyStore = keyStore;
        }

        [HttpPost(Name = "GenerateKey")]
        public string GenerateKey(string nickname)
        {
            return keyStore.GenerateKey(nickname);
        }

        [HttpPut(Name = "EditKey")]
        public void EditKey(string publicKey, string nickname)
        {
            keyStore.EditKey(publicKey, nickname);
        }
    }
}