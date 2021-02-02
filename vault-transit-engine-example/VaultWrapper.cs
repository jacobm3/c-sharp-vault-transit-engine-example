using System;
using System.Text;
using VaultSharp;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.Kubernetes;
using VaultSharp.V1.AuthMethods.Token;
using VaultSharp.V1.Commons;
using VaultSharp.V1.SecretsEngines.Transit;

namespace vault_transit_engine_example
{
    public class VaultWrapper : IVaultWrapper
    {
        VaultClient vaultClient { get; set; }
        
        public VaultWrapper()
        {
            var vaultAddress = System.Environment.GetEnvironmentVariable("VAULT_ADDR");
            var vaultAuthMethod = System.Environment.GetEnvironmentVariable("VAULT_AUTH_METHOD");

            IAuthMethodInfo authMethod = null;

            if (vaultAuthMethod == "token") {
                var vaultToken = System.Environment.GetEnvironmentVariable("VAULT_TOKEN");
                authMethod = new TokenAuthMethodInfo(vaultToken);
            } else if (vaultAuthMethod == "kubernetes")
            {
                string vaultRole = System.Environment.GetEnvironmentVariable("VAULT_ROLE");
                string jwt = System.IO.File.ReadAllText("/var/run/secrets/tokens/vault-token");

                authMethod = new KubernetesAuthMethodInfo(vaultRole, jwt);
            }

            var vaultClientSettings = new VaultClientSettings(vaultAddress, authMethod);

            vaultClient = new VaultClient(vaultClientSettings);
        }

        public async System.Threading.Tasks.Task<string> EncryptAsync(string field, string transitKeyName, int transitKeyVersion)
        {
            var encodedPlainText = Convert.ToBase64String(Encoding.UTF8.GetBytes(field));

            var encryptOptions = new EncryptRequestOptions
            {
                Base64EncodedPlainText = encodedPlainText,
                KeyVersion = transitKeyVersion,
            };

            Secret<EncryptionResponse> encryptionResponse = await vaultClient.V1.Secrets.Transit.EncryptAsync(transitKeyName, encryptOptions);

            return encryptionResponse.Data.CipherText;
        }
    }
}
