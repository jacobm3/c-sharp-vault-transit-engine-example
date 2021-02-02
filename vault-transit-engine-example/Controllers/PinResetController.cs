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
            // Encrypt with Vault Transit Engine

            //var keyName = "test_key";

            //var context = "context1";
            //var plainText = "raja";
            //var encodedPlainText = Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
            //var encodedContext = Convert.ToBase64String(Encoding.UTF8.GetBytes(context));

            //var encryptOptions = new EncryptRequestOptions
            //{
            //    Base64EncodedPlainText = encodedPlainText,
            //    Base64EncodedContext = encodedContext,
            //};

            //Secret<EncryptionResponse> encryptionResponse = await _authenticatedVaultClient.V1.Secrets.Transit.EncryptAsync(keyName, encryptOptions);
            //string cipherText = encryptionResponse.Data.CipherText;

            var vaultAddress = System.Environment.GetEnvironmentVariable("VAULT_ADDR");
            var vaultAuthMethod = System.Environment.GetEnvironmentVariable("VAULT_AUTH_METHOD");

            //if (vaultAuthMethod == "token")
            //{

            //    vaultClient = new VaultClient(vaultClientSettings);
            //}


            var keyName = "test_key";

            var context = "context1";
            var plainText = "raja";
            var encodedPlainText = Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
            var encodedContext = Convert.ToBase64String(Encoding.UTF8.GetBytes(context));

            var encryptOptions = new EncryptRequestOptions
            {
                Base64EncodedPlainText = encodedPlainText,
                Base64EncodedContext = encodedContext,
            };

            Secret<EncryptionResponse> encryptionResponse = await vaultClient.V1.Secrets.Transit.EncryptAsync(keyName, encryptOptions);
            string cipherText = encryptionResponse.Data.CipherText;




            return cipherText;
        }

    }
}
