using Newtonsoft.Json;
using System.Collections.Generic;

namespace CurrencyAPITest
{
    public class GetExchangeCode
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        // Mapping the 'symbols' part of the JSON to a Dictionary
        [JsonProperty("symbols")]
        public Dictionary<string, string>? Code { get; set; }
    }
}
