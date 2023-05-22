using Microsoft.AspNetCore.Mvc;
using Transmitter.Stores;

namespace Transmitter.ViewControllers
{
    [ApiController]
    [Route("KeyManager")]
    public class KeyManagerController : Controller
    {
        private readonly ILogger<KeyManagerController> _logger;
        private readonly IKeyStore keyStore;

        public KeyManagerController(ILogger<KeyManagerController> logger, IKeyStore keyStore)
        {
            _logger = logger;
            this.keyStore = keyStore;
        }

        [HttpGet(Name = "KeyManager")]
        public IActionResult KeyManager()
        {
            var keys = keyStore.GetKeys();
            return View(keys);
        }

        [HttpPost]
        public IActionResult GenerateKey([FromForm] IFormCollection body)
        {
            keyStore.GenerateKey(body["nickname"]);
            return RedirectToRoute("KeyManager");
        }
    }
}