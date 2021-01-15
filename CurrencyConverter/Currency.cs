using System;
using Newtonsoft.Json;

namespace CurrencyConverter
{
    public class Currency
    {
        [JsonProperty("r030")]
        public int R030 { get; set; }
        
        [JsonProperty("txt")]
        public string Txt { get; set; }
        
        [JsonProperty("rate")]
        public decimal Rate { get; set; }
        
        [JsonProperty("cc")]
        public string Cc { get; set; }

        [JsonProperty("exchangedate")]
        public string ExchangeDate { get; set; }
    }
}
