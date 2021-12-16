using System;
using System.Text;
using VaultSharp;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.Kubernetes;
using VaultSharp.V1.AuthMethods.Token;
using VaultSharp.V1.Commons;
using VaultSharp.V1.SecretsEngines.Transit;
using VaultSharp.V1.SecretsEngines.PKI;

namespace vault_transit_engine_example
{
    public class VaultWrapper : IVaultWrapper
    {
        VaultClient vaultClient { get; set; }
        
        public VaultWrapper()
        {
            var vaultAddress = System.Environment.GetEnvironmentVariable("VAULT_ADDR");
            var vaultAuthMethod = System.Environment.GetEnvironmentVariable("VAULT_AUTH_METHOD");
            var vaultNamespace = System.Environment.GetEnvironmentVariable("VAULT_NAMESPACE");

            IAuthMethodInfo authMethod = null;

            if (vaultAuthMethod == "token") {
                var vaultToken = System.Environment.GetEnvironmentVariable("VAULT_TOKEN");
                authMethod = new TokenAuthMethodInfo(vaultToken);
            } else if (vaultAuthMethod == "kubernetes")
            {
                string vaultRole = System.Environment.GetEnvironmentVariable("VAULT_ROLE");
                string jwt = System.IO.File.ReadAllText("/var/run/secrets/kubernetes.io/serviceaccount/token");

                authMethod = new KubernetesAuthMethodInfo(vaultRole, jwt);
            }

            var vaultClientSettings = new VaultClientSettings(vaultAddress, authMethod);

            if (vaultNamespace != "")
            {
                vaultClientSettings.Namespace = vaultNamespace;
            }

            vaultClient = new VaultClient(vaultClientSettings);
        }

        public async System.Threading.Tasks.Task<string> GenerateCertificate (string certName, string commonName)
        {
            var certificateCredentialsRequestOptions = new CertificateCredentialsRequestOptions {CommonName = commonName};
            Secret<CertificateCredentials> certSecret = await vaultClient.V1.Secrets.PKI.GetCredentialsAsync(certName, certificateCredentialsRequestOptions);

            string privateKeyContent = certSecret.Data.PrivateKeyContent;

            return privateKeyContent;
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
