using Microsoft.AspNetCore.Mvc;
using Transmitter.Stores;

namespace Transmitter.ViewControllers
{
    [ApiController]
    [Route("DeleteKey")]
    public class DeleteKeyController : Controller
    {
        private readonly ILogger<Controller> _logger;
        private readonly IKeyStore keyStore;

        public DeleteKeyController(ILogger<Controller> logger, IKeyStore keyStore)
        {
            _logger = logger;
            this.keyStore = keyStore;
        }

        [HttpGet(Name = "DeleteKey")]
        public IActionResult DeleteKey(string publicKey)
        {
            var key = keyStore.GetKey(publicKey);
            return View(key);
        }

        [HttpPost]
        public IActionResult Delete([FromForm] IFormCollection body)
        {
            keyStore.DeleteKey(body["publicKey"]);
            return RedirectToRoute("KeyManager");
        }

    }
}