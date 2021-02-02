using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using vault_transit_engine_example.Models;
using VaultSharp;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.Token;
using VaultSharp.V1.Commons;
using VaultSharp.V1.SecretsEngines.Transit;

namespace vault_transit_engine_example.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PinResetController : ControllerBase
    {
        private readonly ILogger<PinResetController> _logger;
        private readonly IVaultWrapper _vaultWrapper;

        public PinResetController(ILogger<PinResetController> logger, IVaultWrapper vaultWrapper)
        {
            _logger = logger;
            _vaultWrapper = vaultWrapper;
        }

        [HttpPost]
        public async Task<string> PostAsync(PinReset pinReset)
        {
            Request.Headers.TryGetValue("encryptionKeyId", out var encryptionKeyId);
            Request.Headers.TryGetValue("encryptionKeyVersion", out var encryptionKeyVersion);

            var transitKey = encryptionKeyId.First();
            var transitKeyVersion = encryptionKeyVersion.First();


            var cipherText = await _vaultWrapper.EncryptAsync(pinReset.CardNumber, transitKey, Int32.Parse(transitKeyVersion));

            return cipherText;
        }

    }
}
