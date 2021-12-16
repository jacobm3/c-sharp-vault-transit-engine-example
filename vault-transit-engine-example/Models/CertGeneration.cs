using System;
namespace vault_transit_engine_example.Models
{
    public class CertRequest
    {
        public string RoleName { get; set; }
        public string CommonName { get; set; }

        public CertRequest()
        {
        }
    }
}
