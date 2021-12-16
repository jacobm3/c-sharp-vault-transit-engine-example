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
    public class CertGenerationController : ControllerBase
    {
        private readonly ILogger<CertGenerationController> _logger;
        private readonly IVaultWrapper _vaultWrapper;

        public CertGenerationController(ILogger<CertGenerationController> logger, IVaultWrapper vaultWrapper)
        {
            _logger = logger;
            _vaultWrapper = vaultWrapper;
        }

        [HttpPost]
        public async Task<string> PostAsync(CertRequest req)
        {
            var cert = await _vaultWrapper.GenerateCertificate(req.RoleName, req.CommonName);

            return cert;
        }

    }
}
