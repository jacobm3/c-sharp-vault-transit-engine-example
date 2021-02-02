using System;
namespace vault_transit_engine_example.Models
{
    public class PinReset
    {
        public string CardExpirationDate { get; set; }
        public string CardNumber { get; set; }
        public string PinNumber { get; set; }

        public PinReset()
        {
        }
    }
}
