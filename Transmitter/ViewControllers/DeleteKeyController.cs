using Microsoft.AspNetCore.Mvc;
using Transmitter.Models;
using Transmitter.Stores;

namespace Transmitter.ViewControllers
{
    [ApiController]
    [Route("DeleteKey")]
    public class DeleteKeyController : Controller
    {
        private readonly ILogger<Controller> _logger;
        private readonly IKeyStore<SigningKey> signingKeyStore;

        public DeleteKeyController(ILogger<Controller> logger, IKeyStore<SigningKey> signingKeyStore)
        {
            _logger = logger;
            this.signingKeyStore = signingKeyStore;
        }

        [HttpGet(Name = "DeleteKey")]
        public IActionResult DeleteKey(string publicKey)
        {
            var key = signingKeyStore.GetKey(publicKey);
            return View(key);
        }

        [HttpPost]
        public IActionResult Delete([FromForm] IFormCollection body)
        {
            signingKeyStore.DeleteKey(body["publicKey"]);
            return RedirectToRoute("KeyManager");
        }

    }
}