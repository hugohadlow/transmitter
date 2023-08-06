using Microsoft.AspNetCore.Mvc;
using Transmitter.Models;
using Transmitter.Stores;

namespace Transmitter.ViewControllers
{
    [ApiController]
    [Route("KeyManager")]
    public class KeyManagerController : Controller
    {
        private readonly ILogger<KeyManagerController> _logger;
        private readonly IKeyStore<SigningKey> signingKeyStore;

        public KeyManagerController(ILogger<KeyManagerController> logger, IKeyStore<SigningKey> signingKeyStore)
        {
            _logger = logger;
            this.signingKeyStore = signingKeyStore;
        }

        [HttpGet(Name = "KeyManager")]
        public IActionResult KeyManager()
        {
            var keys = signingKeyStore.GetKeys();
            return View(keys);
        }

        [HttpPost]
        public IActionResult GenerateKey([FromForm] IFormCollection body)
        {
            signingKeyStore.GenerateKey(body["nickname"]);
            return RedirectToRoute("KeyManager");
        }
    }
}